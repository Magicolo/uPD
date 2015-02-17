using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.EditorTools {
	[CustomPropertyDrawer(typeof(SingleLineVectorAttribute))]
	public class SingleLineVectorDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			Begin(position, property, label);
		
			float x = property.FindPropertyRelative("x").floatValue;
			float y = property.FindPropertyRelative("y").floatValue;
			float z = 0;
			float w = 0;
			string xName = ((SingleLineVectorAttribute)attribute).x;
			string yName = ((SingleLineVectorAttribute)attribute).y;
			string zName = ((SingleLineVectorAttribute)attribute).z;
			string wName = ((SingleLineVectorAttribute)attribute).w;
		
			int nbOfFields = 2;
			if (property.FindPropertyRelative("z") != null) {
				nbOfFields += 1;
				z = property.FindPropertyRelative("z").floatValue;
			}
			if (property.FindPropertyRelative("w") != null) {
				nbOfFields += 1;
				w = property.FindPropertyRelative("w").floatValue;
			}
		
			float width = currentPosition.width;
			float maxLabelWidth = width / (nbOfFields * 2) - 3;
			EditorGUI.indentLevel = 0;
		
			currentPosition.width /= nbOfFields;
		
			if (noFieldLabel) x = EditorGUI.FloatField(currentPosition, x);
			else {
				EditorGUIUtility.labelWidth = Mathf.Min(xName.GetWidth(EditorStyles.standardFont) + 8, maxLabelWidth);
				x = EditorGUI.FloatField(currentPosition, xName, x);
			}
			property.FindPropertyRelative("x").floatValue = x;
		
			currentPosition.x += currentPosition.width;
			if (noFieldLabel) y = EditorGUI.FloatField(currentPosition, y);
			else {
				EditorGUIUtility.labelWidth = Mathf.Min(yName.GetWidth(EditorStyles.standardFont) + 8, maxLabelWidth);
				y = EditorGUI.FloatField(currentPosition, yName, y);
			}
			property.FindPropertyRelative("y").floatValue = y;
		
			if (property.FindPropertyRelative("z") != null) {
				currentPosition.x += currentPosition.width;
				if (noFieldLabel) z = EditorGUI.FloatField(currentPosition, z);
				else {
					EditorGUIUtility.labelWidth = Mathf.Min(zName.GetWidth(EditorStyles.standardFont) + 8, maxLabelWidth);
					z = EditorGUI.FloatField(currentPosition, zName, z);
				}
				property.FindPropertyRelative("z").floatValue = z;
			}
			if (property.FindPropertyRelative("w") != null) {
				currentPosition.x += currentPosition.width;
				if (noFieldLabel) w = EditorGUI.FloatField(currentPosition, w);
				else {
					EditorGUIUtility.labelWidth = Mathf.Min(wName.GetWidth(EditorStyles.standardFont) + 8, maxLabelWidth);
					w = EditorGUI.FloatField(currentPosition, wName, w);
				}
				property.FindPropertyRelative("w").floatValue = w;
			}
		
			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight;
		}
	}
}