using UnityEngine;
using UnityEditor;

namespace Magicolo.EditorTools {
	[CustomPropertyDrawer(typeof(DisableAttribute))]
	public class DisableDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			drawPrefixLabel = false;
			
			Begin(position, property, label);
		
			EditorGUI.BeginDisabledGroup(true);
			EditorGUI.PropertyField(currentPosition, property, label, true);
			EditorGUI.EndDisabledGroup();
			
			End();
		}
	}
}