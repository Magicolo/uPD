using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.EditorTools {
	public class CustomPropertyDrawerBase : PropertyDrawer {

		public Object target;
		public Object[] targets;
		public SerializedObject serializedObject;
		public Rect currentPosition;
		public float lineHeight;
			
		public virtual void Begin(Rect position, SerializedProperty property, GUIContent label) {
			currentPosition = position;
			serializedObject = property.serializedObject;
			target = serializedObject.targetObject;
			targets = serializedObject.targetObjects;
			lineHeight = EditorGUIUtility.singleLineHeight;
			
			EditorGUI.BeginChangeCheck();
		}
	
		public virtual void End() {
			serializedObject.ApplyModifiedProperties();
			
			if (EditorGUI.EndChangeCheck()) {
				EditorUtility.SetDirty(serializedObject.targetObject);
			}
		}
			
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}
	
		public void ToggleButton(SerializedProperty boolProperty, GUIContent trueLabel, GUIContent falseLabel) {
			Rect indentedPosition = EditorGUI.IndentedRect(currentPosition);
			boolProperty.SetValue(EditorGUI.Toggle(indentedPosition, boolProperty.GetValue<bool>(), new GUIStyle("button")));
			
			if (boolProperty.GetValue<bool>()) {
				Rect labelPosition = new Rect(indentedPosition.x + indentedPosition.width / 2 - trueLabel.text.GetWidth(EditorStyles.standardFont) / 2 - 16, indentedPosition.y, indentedPosition.width, indentedPosition.height);
				EditorGUI.LabelField(labelPosition, trueLabel);
			}
			else {
				Rect labelPosition = new Rect(indentedPosition.x + indentedPosition.width / 2 - falseLabel.text.GetWidth(EditorStyles.standardFont) / 2 - 16, indentedPosition.y, indentedPosition.width, indentedPosition.height);
				EditorGUI.LabelField(labelPosition, falseLabel);
			}
			
			currentPosition.y += currentPosition.height + 2;
		}
		
		public void PropertyField(SerializedProperty property, GUIContent label, bool includeChildren) {
			currentPosition.height = EditorGUI.GetPropertyHeight(property, label, includeChildren);
			EditorGUI.PropertyField(currentPosition, property, label, includeChildren);
			currentPosition.y += currentPosition.height + 2;
		}
		
		public void PropertyField(SerializedProperty property, GUIContent label) {
			PropertyField(property, label, true);
		}
		
		public void PropertyField(SerializedProperty property) {
			PropertyField(property, property.displayName.ToGUIContent(), true);
		}
	}
}

