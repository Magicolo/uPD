using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public static class ScriptableObjectExtensions {

		public static T Clone<T>(this T scriptable) where T : ScriptableObject {
			T clone = default(T);
			
			#if UNITY_EDITOR
			clone = ScriptableObject.CreateInstance<T>();
			UnityEditor.SerializedObject cloneSerialized = new UnityEditor.SerializedObject(clone);
			UnityEditor.SerializedObject scriptableSerialized = new UnityEditor.SerializedObject(scriptable);
			UnityEditor.SerializedProperty scriptableIterator = scriptableSerialized.GetIterator();
		
			while (scriptableIterator.NextVisible(true)) {
				cloneSerialized.FindProperty(scriptableIterator.propertyPath).SetValue(scriptableIterator.GetValue());
			}
			#endif
			
			return clone;
		}
	}
}
