#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using System.Security.AccessControl;
using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class SerializedPropertyExtensions {

		public static SerializedProperty GetLastArrayElement(this SerializedProperty property) {
			return property.GetArrayElementAtIndex(property.arraySize - 1);
		}
		
		public static object GetValue(this SerializedProperty property) {
			switch (property.propertyType) {
				case SerializedPropertyType.Integer:
					return property.intValue;
				case SerializedPropertyType.Boolean:
					return property.boolValue;
				case SerializedPropertyType.Float:
					return property.floatValue;
				case SerializedPropertyType.String:
					return property.stringValue;
				case SerializedPropertyType.Color:
					return property.colorValue;
				case SerializedPropertyType.ObjectReference:
					return property.objectReferenceValue;
				case SerializedPropertyType.LayerMask:
					return property.intValue;
				case SerializedPropertyType.Enum:
					return property.enumValueIndex;
				case SerializedPropertyType.Vector2:
					return property.vector2Value;
				case SerializedPropertyType.Vector3:
					return property.vector3Value;
				case SerializedPropertyType.Vector4:
					return property.vector4Value;
				case SerializedPropertyType.Quaternion:
					return property.quaternionValue;
				case SerializedPropertyType.Rect:
					return property.rectValue;
				case SerializedPropertyType.ArraySize:
					return property.intValue;
				case SerializedPropertyType.Character:
					return property.intValue;
				case SerializedPropertyType.AnimationCurve:
					return property.animationCurveValue;
				case SerializedPropertyType.Bounds:
					return property.boundsValue;
			}
			return null;
		}
	
		public static T GetValue<T>(this SerializedProperty property) {
			return (T)property.GetValue();
		}
		
		public static object GetValue(this SerializedProperty arrayProperty, int index) {
			return arrayProperty.GetArrayElementAtIndex(index).GetValue();
		}
		
		public static T GetValue<T>(this SerializedProperty arrayProperty, int index) {
			return (T)arrayProperty.GetValue(index);
		}
	
		public static object GetLastValue(this SerializedProperty arrayProperty) {
			return arrayProperty.GetLastArrayElement().GetValue();
		}
		
		public static T GetLastValue<T>(this SerializedProperty arrayProperty) {
			return (T)arrayProperty.GetLastValue();
		}
	
		public static object[] GetValues(this SerializedProperty arrayProperty){
			object[] values = new object[arrayProperty.arraySize];
			
			for (int i = 0; i < arrayProperty.arraySize; i++) {
				values[i] = arrayProperty.GetValue(i);
			}
			
			return values;
		}
		
		public static T[] GetValues<T>(this SerializedProperty arrayProperty){
			T[] values = new T[arrayProperty.arraySize];
			
			for (int i = 0; i < arrayProperty.arraySize; i++) {
				values[i] = arrayProperty.GetValue<T>(i);
			}
			
			return values;
		}
		
		public static void SetValue(this SerializedProperty property, object value) {
			switch (property.propertyType) {
				case SerializedPropertyType.Integer:
					property.intValue = value == null ? default(int) : (int)value;
					break;
				case SerializedPropertyType.Boolean:
					property.boolValue = value == null ? default(bool) : (bool)value;
					break;
				case SerializedPropertyType.Float:
					property.floatValue = value == null ? default(float) : (float)value;
					break;
				case SerializedPropertyType.String:
					property.stringValue = value == null ? default(string) : (string)value;
					break;
				case SerializedPropertyType.Color:
					property.colorValue = value == null ? default(Color) : (Color)value;
					break;
				case SerializedPropertyType.ObjectReference:
					property.objectReferenceValue = value as Object == null ? default(Object) : (Object)value;
					break;
				case SerializedPropertyType.LayerMask:
					property.intValue = value == null ? default(int) : (int)value;
					break;
				case SerializedPropertyType.Enum:
					property.enumValueIndex = value == null ? default(int) : (int)value;
					break;
				case SerializedPropertyType.Vector2:
					property.vector2Value = value == null ? default(Vector2) : (Vector2)value;
					break;
				case SerializedPropertyType.Vector3:
					property.vector3Value = value == null ? default(Vector3) : (Vector3)value;
					break;
				case SerializedPropertyType.Vector4:
					property.vector4Value = value == null ? default(Vector4) : (Vector4)value;
					break;
				case SerializedPropertyType.Quaternion:
					property.quaternionValue = value == null ? default(Quaternion) : (Quaternion)value;
					break;
				case SerializedPropertyType.Rect:
					property.rectValue = value == null ? default(Rect) : (Rect)value;
					break;
				case SerializedPropertyType.ArraySize:
					property.intValue = value == null ? default(int) : (int)value;
					break;
				case SerializedPropertyType.Character:
					property.intValue = value == null ? default(int) : (int)value;
					break;
				case SerializedPropertyType.AnimationCurve:
					property.animationCurveValue = value == null ? default(AnimationCurve) : (AnimationCurve)value;
					break;
				case SerializedPropertyType.Bounds:
					property.boundsValue = value == null ? default(Bounds) : (Bounds)value;
					break;
			}
			
			property.serializedObject.ApplyModifiedProperties();
		}

		public static void SetValue(this SerializedProperty arrayProperty, object value, int index) {
			arrayProperty.GetArrayElementAtIndex(index).SetValue(value);
		}
		
		public static void SetLastValue(this SerializedProperty arrayProperty, object value) {
			arrayProperty.GetLastArrayElement().SetValue(value);
		}
		
		public static void SetValues(this SerializedProperty arrayProperty, IList array) {
			arrayProperty.arraySize = array.Count;
			
			for (int i = 0; i < arrayProperty.arraySize; i++) {
				arrayProperty.GetArrayElementAtIndex(i).SetValue(array[i]);
			}
			
			arrayProperty.serializedObject.ApplyModifiedProperties();
		}
		
		public static GUIContent ToGUIContent(this SerializedProperty property){
			return new GUIContent(property.displayName, property.tooltip);
		}
		
		public static SerializedProperty GetParent(this SerializedProperty property) {
			string path = property.propertyPath;
			if (path.EndsWith("]")) {
				for (int i = 0; i < path.Length; i++) {
					if (path[path.Length - 1] != 'A') {
						path.Pop(path.Length - 1, out path);
					}
					else {
						break;
					}
				}
			}
			string[] pathSplit = path.Split('.');
			System.Array.Resize(ref pathSplit, pathSplit.Length - 1);
			string parentPath = pathSplit.Concat(".");
			return property.serializedObject.FindProperty(parentPath);
		}
	
		public static SerializedProperty[] GetChildren(this SerializedProperty property) {
			List<SerializedProperty> children = new List<SerializedProperty>();
		
			while (property.Next(true)) {
				children.Add(property);
			}
			property.Reset();
		
			return children.ToArray();
		}
	
		public static void Clamp(this SerializedProperty property, float min, float max) {
			switch (property.propertyType) {
				case SerializedPropertyType.Integer:
					property.intValue = (int)Mathf.Clamp(property.intValue, min, max);
					break;
				case SerializedPropertyType.Float:
					property.floatValue = Mathf.Clamp(property.floatValue, min, max);
					break;
				case SerializedPropertyType.Color:
					property.colorValue = new Color(Mathf.Clamp(property.colorValue.r, min, max), Mathf.Clamp(property.colorValue.g, min, max), Mathf.Clamp(property.colorValue.b, min, max), Mathf.Clamp(property.colorValue.a, min, max));
					break;
				case SerializedPropertyType.Enum:
					property.enumValueIndex = (int)Mathf.Clamp(property.enumValueIndex, min, max);
					break;
				case SerializedPropertyType.Vector2:
					property.vector2Value = new Vector2(Mathf.Clamp(property.vector2Value.x, min, max), Mathf.Clamp(property.vector2Value.y, min, max));
					break;
				case SerializedPropertyType.Vector3:
					property.vector3Value = new Vector3(Mathf.Clamp(property.vector3Value.x, min, max), Mathf.Clamp(property.vector3Value.y, min, max), Mathf.Clamp(property.vector3Value.z, min, max));
					break;
				case SerializedPropertyType.Vector4:
					property.vector4Value = new Vector4(Mathf.Clamp(property.vector4Value.x, min, max), Mathf.Clamp(property.vector4Value.y, min, max), Mathf.Clamp(property.vector4Value.z, min, max), Mathf.Clamp(property.vector4Value.w, min, max));
					break;
				case SerializedPropertyType.Quaternion:
					property.quaternionValue = new Quaternion(Mathf.Clamp(property.quaternionValue.x, min, max), Mathf.Clamp(property.quaternionValue.y, min, max), Mathf.Clamp(property.quaternionValue.z, min, max), Mathf.Clamp(property.quaternionValue.w, min, max));
					break;
				case SerializedPropertyType.Rect:
					property.rectValue = new Rect(Mathf.Clamp(property.rectValue.x, min, max), Mathf.Clamp(property.rectValue.y, min, max), Mathf.Clamp(property.rectValue.width, min, max), Mathf.Clamp(property.rectValue.height, min, max));
					break;
				case SerializedPropertyType.ArraySize:
					property.intValue = (int)Mathf.Clamp(property.intValue, min, max);
					break;
				case SerializedPropertyType.AnimationCurve:
					property.animationCurveValue = new AnimationCurve(property.animationCurveValue.Clamp(min, max, min, max).keys);
					break;
				case SerializedPropertyType.Bounds:
					property.boundsValue = new Bounds(new Vector3(Mathf.Clamp(property.boundsValue.center.x, min, max), Mathf.Clamp(property.boundsValue.center.y, min, max), Mathf.Clamp(property.boundsValue.center.z, min, max)), new Vector3(Mathf.Clamp(property.boundsValue.size.x, min, max), Mathf.Clamp(property.boundsValue.size.y, min, max), Mathf.Clamp(property.boundsValue.size.z, min, max)));
					break;
			}
		}

		public static void Min(this SerializedProperty property, float min) {
			switch (property.propertyType) {
				case SerializedPropertyType.Integer:
					property.intValue = (int)Mathf.Max(property.intValue, min);
					break;
				case SerializedPropertyType.Float:
					property.floatValue = Mathf.Max(property.floatValue, min);
					break;
				case SerializedPropertyType.Color:
					property.colorValue = new Color(Mathf.Max(property.colorValue.r, min), Mathf.Max(property.colorValue.g, min), Mathf.Max(property.colorValue.b, min), Mathf.Max(property.colorValue.a, min));
					break;
				case SerializedPropertyType.Enum:
					property.enumValueIndex = (int)Mathf.Max(property.enumValueIndex, min);
					break;
				case SerializedPropertyType.Vector2:
					property.vector2Value = new Vector2(Mathf.Max(property.vector2Value.x, min), Mathf.Max(property.vector2Value.y, min));
					break;
				case SerializedPropertyType.Vector3:
					property.vector3Value = new Vector3(Mathf.Max(property.vector3Value.x, min), Mathf.Max(property.vector3Value.y, min), Mathf.Max(property.vector3Value.z, min));
					break;
				case SerializedPropertyType.Vector4:
					property.vector4Value = new Vector4(Mathf.Max(property.vector4Value.x, min), Mathf.Max(property.vector4Value.y, min), Mathf.Max(property.vector4Value.z, min), Mathf.Max(property.vector4Value.w, min));
					break;
				case SerializedPropertyType.Quaternion:
					property.quaternionValue = new Quaternion(Mathf.Max(property.quaternionValue.x, min), Mathf.Max(property.quaternionValue.y, min), Mathf.Max(property.quaternionValue.z, min), Mathf.Max(property.quaternionValue.w, min));
					break;
				case SerializedPropertyType.Rect:
					property.rectValue = new Rect(Mathf.Max(property.rectValue.x, min), Mathf.Max(property.rectValue.y, min), Mathf.Max(property.rectValue.width, min), Mathf.Max(property.rectValue.height, min));
					break;
				case SerializedPropertyType.ArraySize:
					property.intValue = (int)Mathf.Max(property.intValue, min);
					break;
				case SerializedPropertyType.AnimationCurve:
					property.animationCurveValue = new AnimationCurve(property.animationCurveValue.Clamp(Mathf.Infinity, min, Mathf.Infinity, min).keys);
					break;
				case SerializedPropertyType.Bounds:
					property.boundsValue = new Bounds(new Vector3(Mathf.Max(property.boundsValue.center.x, min), Mathf.Max(property.boundsValue.center.y, min), Mathf.Max(property.boundsValue.center.z, min)), new Vector3(Mathf.Max(property.boundsValue.size.x, min), Mathf.Max(property.boundsValue.size.y, min), Mathf.Max(property.boundsValue.size.z, min)));
					break;
			}
		}
	
		public static void Max(this SerializedProperty property, float max) {
			switch (property.propertyType) {
				case SerializedPropertyType.Integer:
					property.intValue = (int)Mathf.Min(property.intValue, max);
					break;
				case SerializedPropertyType.Float:
					property.floatValue = Mathf.Min(property.floatValue, max);
					break;
				case SerializedPropertyType.Color:
					property.colorValue = new Color(Mathf.Min(property.colorValue.r, max), Mathf.Min(property.colorValue.g, max), Mathf.Min(property.colorValue.b, max), Mathf.Min(property.colorValue.a, max));
					break;
				case SerializedPropertyType.Enum:
					property.enumValueIndex = (int)Mathf.Min(property.enumValueIndex, max);
					break;
				case SerializedPropertyType.Vector2:
					property.vector2Value = new Vector2(Mathf.Min(property.vector2Value.x, max), Mathf.Min(property.vector2Value.y, max));
					break;
				case SerializedPropertyType.Vector3:
					property.vector3Value = new Vector3(Mathf.Min(property.vector3Value.x, max), Mathf.Min(property.vector3Value.y, max), Mathf.Min(property.vector3Value.z, max));
					break;
				case SerializedPropertyType.Vector4:
					property.vector4Value = new Vector4(Mathf.Min(property.vector4Value.x, max), Mathf.Min(property.vector4Value.y, max), Mathf.Min(property.vector4Value.z, max), Mathf.Min(property.vector4Value.w, max));
					break;
				case SerializedPropertyType.Quaternion:
					property.quaternionValue = new Quaternion(Mathf.Min(property.quaternionValue.x, max), Mathf.Min(property.quaternionValue.y, max), Mathf.Min(property.quaternionValue.z, max), Mathf.Min(property.quaternionValue.w, max));
					break;
				case SerializedPropertyType.Rect:
					property.rectValue = new Rect(Mathf.Min(property.rectValue.x, max), Mathf.Min(property.rectValue.y, max), Mathf.Min(property.rectValue.width, max), Mathf.Min(property.rectValue.height, max));
					break;
				case SerializedPropertyType.ArraySize:
					property.intValue = (int)Mathf.Min(property.intValue, max);
					break;
				case SerializedPropertyType.AnimationCurve:
					property.animationCurveValue = new AnimationCurve(property.animationCurveValue.Clamp(Mathf.Infinity, max, Mathf.Infinity, max).keys);
					break;
				case SerializedPropertyType.Bounds:
					property.boundsValue = new Bounds(new Vector3(Mathf.Min(property.boundsValue.center.x, max), Mathf.Min(property.boundsValue.center.y, max), Mathf.Min(property.boundsValue.center.z, max)), new Vector3(Mathf.Min(property.boundsValue.size.x, max), Mathf.Min(property.boundsValue.size.y, max), Mathf.Min(property.boundsValue.size.z, max)));
					break;
			}
		}
	
		public static bool Contains(this SerializedProperty arrayProperty, object value) {
			for (int i = 0; i < arrayProperty.arraySize; i++) {
				SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);
				
				if (object.Equals(elementProperty.GetValue(), value)) {
					return true;
				}
			}
			
			return false;
		}
		
		public static int IndexOf(this SerializedProperty arrayProperty, object value) {
			for (int i = 0; i < arrayProperty.arraySize; i++) {
				SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);
				
				if (object.Equals(elementProperty.GetValue(), value)) {
					return i;
				}
			}
			
			return -1;
		}
	
		public static bool TrueForAll<T>(this SerializedProperty arrayProperty, System.Predicate<T> match) {
			for (int i = 0; i < arrayProperty.arraySize; i++) {
				SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);
				
				if (!match(elementProperty.GetValue<T>())) {
					return false;
				}
			}
			
			return true;
		}
	
		public static void ForEach<T>(this SerializedProperty arrayProperty, System.Action<T> action){
			for (int i = 0; i < arrayProperty.arraySize; i++) {
				SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);
				action(elementProperty.GetValue<T>());
			}
		}
		
		public static void ForEachReversed<T>(this SerializedProperty arrayProperty, System.Action<T> action){
			for (int i = arrayProperty.arraySize - 1; i >= 0; i--) {
				SerializedProperty elementProperty = arrayProperty.GetArrayElementAtIndex(i);
				action(elementProperty.GetValue<T>());
			}
		}
	}
}
#endif
