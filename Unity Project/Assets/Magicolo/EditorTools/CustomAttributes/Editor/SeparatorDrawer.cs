using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Magicolo.EditorTools {
	[CustomPropertyDrawer(typeof(SeparatorAttribute))]
	public class SeparatorDrawer : DecoratorDrawer {

		public override void OnGUI(Rect position) {
			position.y += 4;
			EditorGUI.LabelField(position, "", new GUIStyle("RL DragHandle"));
		}
	}
}
