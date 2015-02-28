using UnityEngine;
using System.Collections;

namespace Magicolo.EditorTools {
	[System.Serializable]
	public class EditorHelper {

		public Object[] selection;
		
		[SerializeField]
		protected bool repaintDummy;
		
		[SerializeField]
		protected bool repaint;
		
		public virtual void Update() {
			Unsubscribe();
			Subscribe();
		}
		
		public virtual void Subscribe() {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;
			UnityEditor.EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
			UnityEditor.EditorApplication.modifierKeysChanged += OnModifierKeysChanged;
			UnityEditor.EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
			UnityEditor.EditorApplication.projectWindowChanged += OnProjectWindowChanged;
			UnityEditor.EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
			UnityEditor.EditorApplication.searchChanged += OnSearchChanged;
			UnityEditor.EditorApplication.update += OnUpdate;
			#endif
		}
		
		public virtual void Unsubscribe() {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.hierarchyWindowChanged -= OnHierarchyWindowChanged;
			UnityEditor.EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemGUI;
			UnityEditor.EditorApplication.modifierKeysChanged -= OnModifierKeysChanged;
			UnityEditor.EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
			UnityEditor.EditorApplication.projectWindowChanged -= OnProjectWindowChanged;
			UnityEditor.EditorApplication.projectWindowItemOnGUI -= OnProjectWindowItemGUI;
			UnityEditor.EditorApplication.searchChanged -= OnSearchChanged;
			UnityEditor.EditorApplication.update -= OnUpdate;
			UnityEditor.EditorApplication.update -= CheckForSelectionChanges;
			#endif
		}
		
		public virtual void OnHierarchyWindowChanged() {
		}
		
		public virtual void OnHierarchyWindowItemGUI(int instanceId, Rect selectionrect) {
		}
		
		public virtual void OnModifierKeysChanged() {
		}
		
		public virtual void OnPlaymodeStateChanged() {
			Update();
		}
		
		public virtual void OnProjectWindowChanged() {
		}
		
		public virtual void OnProjectWindowItemGUI(string guid, Rect selectionRect) {
		}
		
		public virtual void OnSearchChanged() {
		}
		
		public virtual void OnSelectionChanged() {
		}
		
		public virtual void OnUpdate() {
			CheckForSelectionChanges();
		}

		void CheckForSelectionChanges() {
			#if UNITY_EDITOR
			bool changed = false;
			Object[] currentSelection = UnityEditor.Selection.objects;
			
			if (selection == null || selection.Length != currentSelection.Length) {
				changed = true;
			}
			else {
				for (int i = 0; i < selection.Length; i++) {
					if (selection[i] != currentSelection[i]){
						changed = true;
						break;
					}
				}
			}
			
			if (changed) {
				selection = UnityEditor.Selection.objects;
				OnSelectionChanged();
				UnityEditor.Selection.objects = selection;
			}
			#endif
		}
	}
}
