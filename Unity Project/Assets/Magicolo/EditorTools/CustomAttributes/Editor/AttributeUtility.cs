#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Magicolo {
	public class AttributeUtility {
	
		public static float indentWidth = 0;
		public static Dictionary<string, int> toRemove = new Dictionary<string, int>();
		public static Dictionary<string, bool> pressedDict = new Dictionary<string, bool>();
	
		static int indentationDepth = 0;
		static int indentLevel = 0;
	
		public static int GetIndexFromLabel(GUIContent label) {
			string strIndex = "";
			for (int i = label.text.Length - 1; i >= 0; i--) {
				if (label.text[i] == 't') break;
				else strIndex += label.text[i];
			}
			strIndex = strIndex.Reverse();
			return int.Parse(strIndex);
		}
	
		public static Rect BeginIndentation(Rect position) {
			indentationDepth += 1;
			indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			indentWidth = indentLevel * 16;
			return (new Rect(position.x + indentWidth, position.y, position.width - indentWidth, position.height));
		}
	
		public static void EndIndentation() {
			indentationDepth -= 1;
			EditorGUI.indentLevel = indentLevel;
		}
	}
}
#endif