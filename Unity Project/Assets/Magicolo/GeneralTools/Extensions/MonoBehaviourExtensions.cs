using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class MonoBehaviourExtensions {

		public static void SetExecutionOrder(this MonoBehaviour behaviour, int order) {
			#if UNITY_EDITOR
			foreach (UnityEditor.MonoScript s in UnityEditor.MonoImporter.GetAllRuntimeMonoScripts()) {
				if (s.name == behaviour.GetType().Name) {
					if (UnityEditor.MonoImporter.GetExecutionOrder(s) != order) {
						UnityEditor.MonoImporter.SetExecutionOrder(s, order);
					}
				}
			}
			#endif
		}
		
		public static void SetTransformHasChanged(this MonoBehaviour behaviour, bool hasChanged) {
			behaviour.StartCoroutine(SetHasChanged(behaviour.transform, hasChanged));
		}
		
		public static void SetTransformHasChanged(this MonoBehaviour behaviour, Transform transform, bool hasChanged) {
			behaviour.StartCoroutine(SetHasChanged(transform, hasChanged));
		}
	
		static IEnumerator SetHasChanged(Transform transform, bool hasChanged) {
			yield return new WaitForEndOfFrame();
			
			transform.hasChanged = hasChanged;
		}

	}
}
