using UnityEngine;
using UnityEditor;

namespace Magicolo.EditorTools {
	[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
	public class MinMaxSliderDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			Begin(position, property, label);
		
			float x = property.FindPropertyRelative("x").floatValue;
			float y = property.FindPropertyRelative("y").floatValue;
			float min = 0;
			float max = 0;
			string minLabel = ((MinMaxSliderAttribute)attribute).minLabel;
			string maxLabel = ((MinMaxSliderAttribute)attribute).maxLabel;
		
			if (property.FindPropertyRelative("z") != null) min = property.FindPropertyRelative("z").floatValue;
			else min = ((MinMaxSliderAttribute)attribute).min;
			if (property.FindPropertyRelative("w") != null) max = property.FindPropertyRelative("w").floatValue;
			else max = ((MinMaxSliderAttribute)attribute).max;
		
			EditorGUI.indentLevel = 0;
			float width = currentPosition.width;
		
			currentPosition.width = width / 4;
			if (!noFieldLabel && !string.IsNullOrEmpty(minLabel) && width / 8 >= 16) {
				currentPosition.width = Mathf.Min(minLabel.GetWidth(EditorStyles.standardFont) + 4, width / 8);
				EditorGUI.LabelField(currentPosition, minLabel);
				currentPosition.x += currentPosition.width;
				currentPosition.width = width / 4 - currentPosition.width;
				x = EditorGUI.FloatField(currentPosition, x);
			}
			else x = EditorGUI.FloatField(currentPosition, x);
			currentPosition.x += currentPosition.width + 2;
		
			currentPosition.width = width / 2;
			EditorGUI.MinMaxSlider(currentPosition, ref x, ref y, min, max);
		
			currentPosition.x += currentPosition.width + 2;
			currentPosition.width = width / 4;
			if (!noFieldLabel && !string.IsNullOrEmpty(maxLabel) && width / 8 >= 16) {
				float labelWidth = Mathf.Min(maxLabel.GetWidth(EditorStyles.standardFont) + 4, width / 8);
				currentPosition.width = width / 4 - labelWidth;
				GUIStyle style = new GUIStyle(EditorStyles.label);
				style.alignment = TextAnchor.MiddleRight;
				y = EditorGUI.FloatField(currentPosition, y);
				currentPosition.x += currentPosition.width;
				currentPosition.width = labelWidth;
				EditorGUI.LabelField(currentPosition, maxLabel, style);
			
			}
			else y = EditorGUI.FloatField(currentPosition, y);
		
			property.FindPropertyRelative("x").floatValue = Mathf.Clamp(x, min, y);
			property.FindPropertyRelative("y").floatValue = Mathf.Clamp(y, x, max);
		
			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUIUtility.singleLineHeight;
		}
	}
}
