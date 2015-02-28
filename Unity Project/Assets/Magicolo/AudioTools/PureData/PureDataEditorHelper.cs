using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo.EditorTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataEditorHelper : EditorHelper {
		
		public Texture pureDataIcon;
		public IEnumerator routine;
		public AudioSource previewAudioSource;
		public float previewStopTime;
		public bool editorPaused;
		public PureData pureData;

		[SerializeField]
		bool updateHierarchy;
		
		public PureDataEditorHelper(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			Update();
		}

		public void Stop() {
			Unsubscribe();
		}
		
		public override void OnHierarchyWindowItemGUI(int instanceId, Rect selectionrect) {
			base.OnHierarchyWindowItemGUI(instanceId, selectionrect);
			
			#if UNITY_EDITOR
			GameObject gameObject = UnityEditor.EditorUtility.InstanceIDToObject(instanceId) as GameObject;
			
			if (gameObject == null || pureData == null) {
				return;
			}
			
			if (gameObject == pureData.gameObject){
				ShowPureDataIcon(gameObject, selectionrect);
			}
			else if (pureData.hierarchyManager.folderStructure.Contains(gameObject)){
				ShowFolderButton(gameObject, selectionrect);
			}
			else {
				PureDataSetup setup = gameObject.GetComponent<PureDataSetup>();
				
				if (setup != null && setup.Info != null){
					ShowPreviewButton(setup, gameObject, selectionrect);
				}
			}
			#endif
		}

		public override void OnPlaymodeStateChanged() {
			base.OnPlaymodeStateChanged();
			
			if (previewAudioSource != null) {
				previewAudioSource.gameObject.Remove();
			}
			
			routine = null;
			
			#if UNITY_EDITOR
			editorPaused = UnityEditor.EditorApplication.isPaused;
			#endif
		}
		
		public override void OnProjectWindowChanged() {
			base.OnProjectWindowChanged();
			
			if (pureData != null && pureData.hierarchyManager != null) {
				updateHierarchy = true;
			}
		}

		public override void OnSelectionChanged() {
			base.OnSelectionChanged();
			
			if (pureData != null && pureData.hierarchyManager != null && selection.Length == 1 && selection[0] == pureData.gameObject) {
				updateHierarchy = true;
			}
		}
		
		public override void OnUpdate() {
			base.OnUpdate();
			
			if (routine != null && !routine.MoveNext()) {
				routine = null;
			}
			
			if (updateHierarchy) {
				pureData.hierarchyManager.UpdateHierarchy();
				updateHierarchy = false;
			}
			
			if (repaint) {
				RepaintInspector();
			}
		}

		public void RepaintInspector() {
			#if UNITY_EDITOR
			if (pureData.generalSettings.IsMainThread()) {
				UnityEditor.SerializedObject pureDataSerialized = new UnityEditor.SerializedObject(pureData);
				UnityEditor.SerializedProperty repaintDummyProperty = pureDataSerialized.FindProperty("editorHelper").FindPropertyRelative("repaintDummy");
				repaintDummyProperty.SetValue(!repaintDummy);
			}
			else {
				repaint = true;
			}
			#endif
		}
		
		void ShowPureDataIcon(GameObject gameObject, Rect selectionrect) {
			pureDataIcon = pureDataIcon ?? HelperFunctions.LoadAssetInFolder<Texture>("pd.png", "Magicolo/AudioTools/PureData");
			float width = selectionrect.width;
			selectionrect.width = 16;
			selectionrect.height = 16;
			
			if (pureDataIcon != null) {
				selectionrect.x = width - 4 + gameObject.GetHierarchyDepth() * 14;
				GUI.DrawTexture(selectionrect, pureDataIcon);
			}
		}

		void ShowFolderButton(GameObject gameObject, Rect selectionrect) {
			#if UNITY_EDITOR
			Texture folderIcon = UnityEditor.EditorGUIUtility.IconContent("Project").image;
			float width = selectionrect.width;
			selectionrect.width = 30;
			selectionrect.height = 16;
			selectionrect.x = width - 2 + gameObject.GetHierarchyDepth() * 14;
			selectionrect.height += 1;
			GUIStyle style = new GUIStyle("MiniToolbarButtonLeft");
			style.fixedHeight += 1;
			style.contentOffset = new Vector2(-4, 0);
			style.clipping = TextClipping.Overflow;
			
			if (GUI.Button(selectionrect, folderIcon, style)) {
				List<Object> selectedGameObjects = new List<Object>(UnityEditor.Selection.objects);
				
				if (selection.Contains(gameObject)) {
					selectedGameObjects.Remove(gameObject);
				}
				
				selectedGameObjects.AddRange(gameObject.GetChildrenRecursive());
				foreach (GameObject folder in pureData.hierarchyManager.folderStructure) {
					if (selectedGameObjects.Contains(folder)) {
						selectedGameObjects.Remove(folder);
					}
				}
				
				UnityEditor.Selection.objects = selectedGameObjects.ToArray();
			}
			#endif
		}
		
		void ShowPreviewButton(PureDataSetup setup, GameObject gameObject, Rect selectionrect) {
			#if UNITY_EDITOR
			Texture previewIcon = UnityEditor.EditorGUIUtility.ObjectContent(null, typeof(AudioSource)).image;
			PureDataInfo info = setup.Info;
			float width = selectionrect.width;
			selectionrect.width = 30;
			selectionrect.height = 16;
			selectionrect.x = width - 2 + gameObject.GetHierarchyDepth() * 14;
			selectionrect.height += 1;
			GUIStyle style = new GUIStyle("MiniToolbarButtonLeft");
			style.fixedHeight += 1;
			style.contentOffset = new Vector2(-4, 0);
			style.clipping = TextClipping.Overflow;
					
			if (GUI.Button(selectionrect, previewIcon, style)) {
				UnityEditor.Selection.activeObject = gameObject;
						
				if (previewAudioSource != null) {
					previewAudioSource.gameObject.Remove();
				}
						
				previewAudioSource = setup.Clip.PlayOnListener();
				if (previewAudioSource != null) {
					previewAudioSource.volume = info.volume + info.volume * Random.Range(-info.randomVolume, info.randomVolume);
					previewAudioSource.pitch = info.pitch + info.pitch * Random.Range(-info.randomPitch, info.randomPitch);
					previewAudioSource.time = previewAudioSource.pitch >= 0 ? previewAudioSource.clip.length * info.playRangeStart : previewAudioSource.clip.length * Mathf.Min(info.playRangeEnd, 0.99999F);
					previewStopTime = previewAudioSource.pitch >= 0 ? previewAudioSource.clip.length * info.playRangeEnd : previewAudioSource.clip.length * info.playRangeStart;
					routine = DestroyAfterPlaying(previewAudioSource);
				}
			}
			else if (Event.current.isMouse && Event.current.type == EventType.mouseDown) {
				if (previewAudioSource != null) {
					previewAudioSource.gameObject.Remove();
				}
				routine = null;
			}
			#endif
		}
		
		IEnumerator DestroyAfterPlaying(AudioSource audioSource) {
			while (audioSource != null && audioSource.isPlaying && audioSource.pitch >= 0 ? audioSource.time < previewStopTime : audioSource.time > previewStopTime) {
				yield return null;
			}
			
			if (audioSource != null) {
				audioSource.gameObject.Remove();
			}
		}
	}
}
