using UnityEngine;
using UnityEditor;

namespace Magicolo.EditorTools {
	[CustomPropertyDrawer(typeof(MinAttribute))]
	public class MinDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			drawPrefixLabel = false;
			
			Begin(position, property, label);
			
			float min = ((MinAttribute)attribute).min;
		
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(currentPosition, property, label, true);
			if (EditorGUI.EndChangeCheck()) {
				property.Min(min);
			}
			
			End();
		}
	}
}