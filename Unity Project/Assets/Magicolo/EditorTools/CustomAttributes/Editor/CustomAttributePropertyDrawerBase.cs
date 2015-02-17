using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Magicolo.EditorTools {
	public class CustomAttributePropertyDrawerBase : CustomPropertyDrawerBase {
	
		public string prefixLabel;
		public bool noFieldLabel;
		public bool noPrefixLabel;
		public bool noIndex;
		public bool disableOnPlay;
		public bool disableOnStop;
		public int index;
		public Event currentEvent;
	
		public bool drawPrefixLabel = true;
		public float scrollbarThreshold;
		public GUIContent currentLabel = GUIContent.none;
	
		public SerializedProperty arrayProperty;
	
		static MethodInfo getPropertyDrawerMethod;
		public static MethodInfo GetPropertyDrawerMethod {
			get {
				if (getPropertyDrawerMethod == null) {
					foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
						foreach (Type type in assembly.GetTypes()) {
							if (type.Name == "ScriptAttributeUtility") {
								getPropertyDrawerMethod = type.GetMethod("GetDrawerTypeForType", ObjectExtensions.AllFlags);
							}
						}
					}
				}
				return getPropertyDrawerMethod;
			}
		}
	
		int indentLevel;
	
		public override void Begin(Rect position, SerializedProperty property, GUIContent label) {
			base.Begin(position, property, label);
			
			noFieldLabel = ((CustomAttributeBase)attribute).NoFieldLabel;
			noPrefixLabel = ((CustomAttributeBase)attribute).NoPrefixLabel;
			noIndex = ((CustomAttributeBase)attribute).NoIndex;
			prefixLabel = ((CustomAttributeBase)attribute).PrefixLabel;
			disableOnPlay = ((CustomAttributeBase)attribute).DisableOnPlay;
			disableOnStop = ((CustomAttributeBase)attribute).DisableOnStop;
			scrollbarThreshold = Screen.width - position.width > 19 ? 298 : 313;
			indentLevel = EditorGUI.indentLevel;
			currentEvent = Event.current;
			
			EditorGUI.BeginDisabledGroup((Application.isPlaying && disableOnPlay) || (!Application.isPlaying && disableOnStop));
		
			if (fieldInfo.FieldType.IsArray) {
				index = AttributeUtility.GetIndexFromLabel(label);
				arrayProperty = property.GetParent();
 			
				if (noIndex) {
					if (string.IsNullOrEmpty(prefixLabel)) {
						label.text = label.text.Substring(0, label.text.Length - 2);
					}
				}
				else if (!string.IsNullOrEmpty(prefixLabel)) {
					prefixLabel += " " + index;
				}
			}
		
		
			if (drawPrefixLabel) {
				if (!noPrefixLabel) {
					if (!string.IsNullOrEmpty(prefixLabel)) {
						label.text = prefixLabel;
					}
					position = EditorGUI.PrefixLabel(position, label);
				}
			}
			else {
				if (noPrefixLabel) label.text = "";
				else if (!string.IsNullOrEmpty(prefixLabel)) label.text = prefixLabel;
			}
			
			currentPosition = position;
			currentLabel = label;
		}
	
		public override void End() {
			base.End();
			
			EditorGUI.indentLevel = indentLevel;
			EditorGUI.EndDisabledGroup();
		}
	
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
	
		public PropertyDrawer GetPropertyDrawer(Type propertyAttributeType, params object[] arguments) {
			Type propertyDrawerType = GetPropertyDrawerMethod.Invoke(null, new object[] { propertyAttributeType }) as Type;
			if (propertyDrawerType != null) {
				PropertyAttribute propertyAttribute = Activator.CreateInstance(propertyAttributeType, arguments) as PropertyAttribute;
				PropertyDrawer propertyDrawer = Activator.CreateInstance(propertyDrawerType) as PropertyDrawer;
				propertyDrawer.SetValueToMember("m_Attribute", propertyAttribute);
				propertyDrawer.SetValueToMember("m_FieldInfo", fieldInfo);
				return propertyDrawer;
			}
			return null;
		}
	
		public PropertyDrawer GetPropertyDrawer(Attribute propertyAttribute, params object[] arguments) {
			return GetPropertyDrawer(propertyAttribute.GetType(), arguments);
		}
	}
}