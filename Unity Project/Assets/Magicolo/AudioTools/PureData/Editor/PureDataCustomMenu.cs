using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Magicolo.AudioTools {
	public static class PureDataCustomMenus {
	
		[MenuItem("Magicolo's Tools/Create/Pure Data")]
		static void CreatePureData() {
			GameObject gameObject;
			PureData existingPureData = Object.FindObjectOfType<PureData>();
		
			if (existingPureData == null) {
				gameObject = new GameObject();
				gameObject.name = "PureData";
				gameObject.AddComponent<PureData>();
				PureDataPluginManager.CheckPlugins();
				Undo.RegisterCreatedObjectUndo(gameObject, "Pure Data Created");
			}
			else {
				gameObject = existingPureData.gameObject;
			}
			
			Selection.activeGameObject = gameObject;
		}
	}
}