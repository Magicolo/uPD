using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataHierarchyManager {
		
		public PureDataSetup[] setups;
		public AudioClip[] currentClips;
		public AudioClip[] clips;
		public GameObject[] children;
		public List<GameObject> folderStructure = new List<GameObject>();
		public List<string> paths;
		
		public PureData pureData;
		
		public PureDataHierarchyManager(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Start() {
			if (!Application.isEditor) {
				pureData.DestroyChildren();
			}
		}
		
		public void UpdateHierarchy() {
			if (pureData == null || Application.isPlaying) {
				return;
			}
			
			if (CheckForChanges()) {
				pureData.infoManager.UpdateInfos();
				pureData.clipManager.UpdateClipContainer();
				UpdateAudioSetups();
				SetCurrentAudioClips();
				CreateHierarchy();
				RemoveEmptyFolders();
				SortChildren();
				CleanUp();
			}
			
			FreezeTransforms();
		}

		public void UpdateAudioSetups() {
			foreach (PureDataSetup setup in setups) {
				setup.UpdateSetup();
			}
		}
		
		public void SetCurrentAudioClips() {
			clips = Resources.LoadAll<AudioClip>("");
			currentClips = new AudioClip[setups.Length];
			children = pureData.gameObject.GetChildrenRecursive();
			
			for (int i = 0; i < setups.Length && i < children.Length; i++) {
				if (setups[i] != null) {
					currentClips[i] = setups[i].Clip;
					children[i] = setups[i].gameObject;
				}
			}
		}
		
		public void CreateHierarchy() {
			#if UNITY_EDITOR
			foreach (AudioClip clip in clips) {
//				string clipPath = UnityEditor.AssetDatabase.GetAssetPath(clip).GetRange("Assets/Resources/".Length);
				string clipPath = HelperFunctions.GetResourcesPath(clip);
				string clipDirectory = Path.GetDirectoryName(clipPath);
				GameObject parent = GetOrAddFolder(clipDirectory);
				GameObject child = GameObject.Find(clip.name);
				
				if (child == null) {
					child = new GameObject(clip.name);
					PureDataSetup setup = child.GetOrAddComponent<PureDataSetup>();
					
					setup.name = clip.name;
					setup.Info = pureData.infoManager.GetInfo(clip.name);
					setup.pureData = pureData;
					setup.FreezeTransform();
				}
				child.transform.parent = parent.transform;
				child.transform.Reset();
			}
			#endif
		}
		
		public GameObject GetOrAddFolder(string directory) {
			string[] folderNames = directory.Split(Path.AltDirectorySeparatorChar);
			GameObject parent = pureData.gameObject;
			GameObject folder = pureData.gameObject;
			
			foreach (string folderName in folderNames) {
				if (string.IsNullOrEmpty(folderName)) {
					continue;
				}
				
				folder = parent.FindChild(folderName);
				if (folder == null) {
					folder = new GameObject(folderName);
					folder.transform.parent = parent.transform;
					folderStructure.Add(folder);
				}
				parent = folder;
			}
			return parent;
		}

		public void RemoveEmptyFolders() {
			foreach (GameObject folder in folderStructure.ToArray()) {
				if (folder != null) {
					if (folder.transform.childCount == 0) {
						RemoveEmptyFolder(folder);
					}
				}
			}
		}
		
		public void RemoveEmptyFolder(GameObject folder) {
			Transform parent = folder.transform.parent;
			
			if (parent != null && parent.childCount == 1 && parent != pureData.transform) {
				folderStructure.Remove(folder);
				RemoveEmptyFolder(folder.transform.parent.gameObject);
			}
			else {
				folderStructure.Remove(folder);
				folder.Remove();
			}
		}

		public void SortChildren() {
			pureData.SortChildrenRecursive();
		}

		public bool CheckForChanges() {
			bool hasChanged = false;
			setups = pureData.GetComponentsInChildren<PureDataSetup>();
			
			#if !UNITY_WEBPLAYER && UNITY_EDITOR
			string[] resourcesPaths = HelperFunctions.GetFolderPaths("Resources");
			List<string> audioFiles = new List<string>();
			
			foreach (string resourcesPath in resourcesPaths) {
				string[] audioExtensions = { ".wav", ".mp3", ".ogg", ".aiff" };
				string[] files = Directory.GetFiles(resourcesPath, "*.*", SearchOption.AllDirectories);
			
				foreach (string file in files) {
					if (audioExtensions.Contains(Path.GetExtension(file).ToLower())) {
						audioFiles.Add(file);
					}
				}
			}
			
			hasChanged = paths == null || paths.Count != audioFiles.Count || setups.Length != audioFiles.Count || !paths.ContentEquals(audioFiles);
			paths = audioFiles;
			#endif
			
			return hasChanged;
		}
		
		public void FreezeTransforms() {
			pureData.transform.hideFlags = HideFlags.HideInInspector;
			pureData.transform.Reset();
			
			foreach (PureDataSetup setup in setups == null || setups.Length == 0 ? pureData.GetComponentsInChildren<PureDataSetup>() : setups) {
				if (setup != null) {
					setup.FreezeTransform();
				}
			}
			
			foreach (GameObject gameObject in folderStructure) {
				if (gameObject != null) {
					gameObject.transform.hideFlags = HideFlags.HideInInspector;
					gameObject.transform.Reset();
				}
			}
		}

		public void CleanUp() {
			setups = null;
			currentClips = null;
			clips = null;
			Resources.UnloadUnusedAssets();
		}
	}
}
