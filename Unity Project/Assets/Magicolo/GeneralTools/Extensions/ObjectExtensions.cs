using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class ObjectExtensions {
	
		public const BindingFlags AllFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags AllPublicFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags AllNonPublicFlags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
		public const BindingFlags AllStaticFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;
		public const BindingFlags AllInstanceFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		public static MemberInfo GetMemberInfo(this object obj, string memberName) {
			FieldInfo field = obj.GetType().GetField(memberName, AllFlags);
			if (field != null) {
				return field;
			}
		
			PropertyInfo property = obj.GetType().GetProperty(memberName, AllFlags);
			if (property != null) {
				return property;
			}
			return null;
		}
	
		public static T GetValue<T>(this MemberInfo memberInfo, object obj) {
			return (T)memberInfo.GetValue(obj);
		}
		
		public static object GetValue(this MemberInfo memberInfo, object obj) {
			FieldInfo field = memberInfo as FieldInfo;
			if (field != null) {
				return field.GetValue(obj);
			}
		
			PropertyInfo property = memberInfo as PropertyInfo;
			if (property != null) {
				return property.GetValue(obj, null);
			}
		
			return null;
		}
			
		public static void SetValue(this MemberInfo memberInfo, object obj, object value) {
			FieldInfo field = memberInfo as FieldInfo;
			if (field != null) {
				field.SetValue(obj, value);
				return;
			}
		
			PropertyInfo property = memberInfo as PropertyInfo;
			if (property != null) {
				property.SetValue(obj, value, null);
				return;
			}
		}
		
		public static MemberInfo GetMemberInfoAtPath(this object obj, string memberPath) {
			string[] pathSplit = memberPath.Split('.');
		
			if (pathSplit.Length <= 1) {
				return obj.GetMemberInfo(pathSplit.Pop(out pathSplit));
			}
		
			object value = obj.GetValueFromMember(pathSplit.Pop(out pathSplit));
		
			while (pathSplit.Length > 1) {
				value = value.GetValueFromMember(pathSplit.Pop(out pathSplit));
			}
		
			return value.GetMemberInfo(pathSplit.Pop(out pathSplit));
		}
	
		public static T GetValueFromMember<T>(this object obj, string memberName) {
			return (T)obj.GetValueFromMember(memberName);
		}
	
		public static object GetValueFromMember(this object obj, string memberName) {
			if (obj is IList) {
				return ((IList)obj)[int.Parse(memberName)];
			}
		
			MemberInfo member = obj.GetMemberInfo(memberName);
			return member.GetValue(obj);
		}
	
		public static T GetValueFromMemberAtPath<T>(this object obj, string memberPath) {
			return (T)obj.GetValueFromMemberAtPath(memberPath);
		}
	
		public static object GetValueFromMemberAtPath(this object obj, string memberPath) {
			MemberInfo member = obj.GetMemberInfoAtPath(memberPath);
			string[] pathSplit = memberPath.Split('.');
		
			if (pathSplit.Length <= 1) {
				return obj.GetValueFromMember(pathSplit.Pop(out pathSplit));
			}
		
			int index;
			if (int.TryParse(pathSplit.Last(), out index)) {
				Array.Resize(ref pathSplit, pathSplit.Length - 1);
				return ((IList)obj.GetValueFromMemberAtPath(pathSplit.Concat(".")))[index];
			}
			Array.Resize(ref pathSplit, pathSplit.Length - 1);
		
			object container = obj.GetValueFromMemberAtPath(pathSplit.Concat("."));

			return member.GetValue(container);
		}
	
		public static void SetValueToMember(this object obj, string memberName, object value) {
			MemberInfo member = obj.GetMemberInfo(memberName);
		
			member.SetValue(obj, value);
		}
	
		public static void SetValueToMemberAtPath(this object obj, string memberPath, object value) {
			MemberInfo member = obj.GetMemberInfoAtPath(memberPath);
			string[] pathSplit = memberPath.Split('.');
		
			if (pathSplit.Length <= 1) {
				obj.SetValueToMember(memberPath, value);
				return;
			}
		
			Array.Resize(ref pathSplit, pathSplit.Length - 1);
			object container = obj.GetValueFromMemberAtPath(pathSplit.Concat("."));
		
			member.SetValue(container, value);
		}
	
		public static object InvokeMethod(this object obj, string methodName, params object[] arguments) {
			MethodInfo method = obj.GetType().GetMethod(methodName, AllFlags);
			if (method != null) {
				return method.Invoke(obj, arguments);
			}
			return null;
		}
	
		public static string[] GetFieldsPropertiesNames(this object obj, BindingFlags flags, params Type[] filter) {
			return obj.GetType().GetFieldsPropertiesNames(flags, filter);
		}
	
		public static string[] GetFieldsPropertiesNames(this object obj, params Type[] filter) {
			return obj.GetType().GetFieldsPropertiesNames(AllFlags, filter);
		}
	
		public static void Copy<T>(this T copyTo, T copyFrom, params string[] parametersToIgnore) where T : class {
			if (typeof(Component).IsAssignableFrom(typeof(T))) {
				List<string> parametersToIgnoreList = new List<string>(parametersToIgnore);
				parametersToIgnoreList.Add("name");
				parametersToIgnoreList.Add("tag");
				if (!(copyFrom is MonoBehaviour)) {
					parametersToIgnoreList.Add("mesh");
					parametersToIgnoreList.Add("material");
					parametersToIgnoreList.Add("materials");
				}
				parametersToIgnore = parametersToIgnoreList.ToArray();
			}
		
			foreach (FieldInfo fieldInfo in copyFrom.GetType().GetFields(AllFlags)) {
				if ((fieldInfo.IsPublic || fieldInfo.GetCustomAttributes(typeof(SerializeField), true).Length != 0) && !fieldInfo.IsLiteral && fieldInfo.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length == 0 && !parametersToIgnore.Contains(fieldInfo.Name)) {
					try {
						fieldInfo.SetValue(copyTo, fieldInfo.GetValue(copyFrom));
					}
					catch {
					}
				}
			}
			
			foreach (PropertyInfo propertyInfo in copyFrom.GetType().GetProperties(AllFlags)) {
				if (propertyInfo.CanWrite && propertyInfo.GetCustomAttributes(typeof(ObsoleteAttribute), true).Length == 0 && !parametersToIgnore.Contains(propertyInfo.Name)) {
					try {
						propertyInfo.SetValue(copyTo, propertyInfo.GetValue(copyFrom, null), null);
					}
					catch {
					}
				}
			}
		}

		public static void Remove(this UnityEngine.Object obj) {
			if (Application.isPlaying) {
				UnityEngine.Object.Destroy(obj);
			}
			else {
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}
		
		public static void DisconnectPrefab(this UnityEngine.Object obj) {
			#if UNITY_EDITOR
			UnityEditor.PrefabUtility.DisconnectPrefabInstance(obj);
			#endif
		}
	
		public static T[] SendMessageToObjectsOfType<T>(this UnityEngine.Object obj, string methodName, object value, bool sendToSelf, SendMessageOptions options = SendMessageOptions.DontRequireReceiver) where T : Component {
			List<T> objects = new List<T>();
			
			foreach (T element in UnityEngine.Object.FindObjectsOfType<T>()) {
				if (!sendToSelf && element == obj) {
					continue;
				}
				
				element.SendMessage(methodName, value, options);
				objects.Add(element);
			}
			
			return objects.ToArray();
		}
	
		public static T[] SendMessageToObjectsOfType<T>(this UnityEngine.Object obj, string methodName, bool sendToSelf = false, SendMessageOptions options = SendMessageOptions.DontRequireReceiver) where T : Component {
			return obj.SendMessageToObjectsOfType<T>(methodName, obj, sendToSelf, options);
		}

		public static string GetUniqueName(this UnityEngine.Object obj, string newName, string oldName, IList<UnityEngine.Object> array) {
			int suffix = 0;
			bool uniqueName = false;
			string currentName = "";
		
			while (!uniqueName) {
				uniqueName = true;
				currentName = newName;
				if (suffix > 0) currentName += suffix.ToString();
			
				foreach (UnityEngine.Object element in array) {
					if (element != null && element != obj && element.name == currentName && element.name != oldName) {
						uniqueName = false;
						break;
					}
				}
				suffix += 1;
			}
			return currentName;
		}
	
		public static string GetUniqueName(this UnityEngine.Object obj, string newName, string oldName, string emptyName, IList<UnityEngine.Object> array) {
			string name = obj.GetUniqueName(newName, oldName, array);
			if (string.IsNullOrEmpty(newName)) {
				name = obj.GetUniqueName(emptyName, oldName, array);
			}
			return name;
		}
	
		public static string GetUniqueName(this UnityEngine.Object obj, string newName, IList<UnityEngine.Object> array) {
			return obj.GetUniqueName(newName, obj.name, array);
		}
	
		public static void SetUniqueName(this UnityEngine.Object obj, string newName, string oldName, string emptyName, IList<UnityEngine.Object> array) {
			obj.name = obj.GetUniqueName(newName, oldName, emptyName, array);
		}
	
		public static void SetUniqueName(this UnityEngine.Object obj, string newName, string oldName, IList<UnityEngine.Object> array) {
			obj.name = obj.GetUniqueName(newName, oldName, array);
		}
	
		public static void SetUniqueName(this UnityEngine.Object obj, string newName, IList<UnityEngine.Object> array) {
			obj.name = obj.GetUniqueName(newName, obj.name, array);
		}
	}
}
