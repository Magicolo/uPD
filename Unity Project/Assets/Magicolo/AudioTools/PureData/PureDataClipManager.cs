using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Collections;
using LibPDBinding;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataClipManager : PureDataIdManager<PureDataClip> {

		public string containerPath;
		public List<PureDataClip> clips;
		public PureData pureData;
		
		Dictionary<string, PureDataClip> nameClipDict;
		Dictionary<string, int> nameOccurenceDict;

		public PureDataClipManager(PureData pureData) {
			this.pureData = pureData;
			
			UpdateClipContainer();
		}
					
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			foreach (PureDataClip clip in clips) {
				clip.Initialize(pureData);
			}
		}

		public void Start() {
			InitializeClips();
			BuildDicts();
		}
		
		public void InitializeClips() {
			ResetUniqueIds(clips);
			
			foreach (PureDataClip clip in clips) {
				if (pureData.infoManager.GetInfo(clip.Name).loadOnAwake) {
					clip.Load();
				}
			}
		}
		
		public void UpdateClips() {
			clips = new List<PureDataClip>();
			
			foreach (PureDataInfo info in pureData.infoManager.GetAllClipInfos()) {
				PureDataClip clip = new PureDataClip(info, pureData);
				clips.Add(clip);
			}
		}

		public void UpdateClipContainer() {
			#if !UNITY_WEBPLAYER
			if (!SetContainerPath()) {
				return;
			}
			
			UpdateClips();
			
			ThreadPool.QueueUserWorkItem(new WaitCallback(WriteToClipContainer));
			#endif
		}
		
		public void WriteToClipContainer(object state) {
			#if !UNITY_WEBPLAYER
			List<string> text = new List<string>();
			
			text.Add("#N canvas 200 300 450 300 10;");
			foreach (PureDataClip clip in clips) {
				text.Add(string.Format("#X obj 0 0 uaudioclip {0};", clip.Name));
			}
			
			File.WriteAllLines(containerPath, text.ToArray());
			#endif
		}
		
		public bool SetContainerPath() {
			#if !UNITY_WEBPLAYER
			if (string.IsNullOrEmpty(containerPath) || !File.Exists(containerPath) || !HelperFunctions.PathIsRelativeTo(containerPath, Application.streamingAssetsPath)) {
				containerPath = Path.GetFullPath(HelperFunctions.GetAssetPath("uaudioclipcontainer.pd"));
			}
			
			if (!File.Exists(containerPath)) {
				Logger.LogError("Can not find uaudioclipcontainer.pd patch.");
				return false;
			}
			#endif
			
			return true;
		}
		
		public void Load(string soundName) {
			GetClip(soundName).Load();
		}
				
		public void Unload(string soundName) {
			GetClip(soundName).Unload();
		}
		
		public void UnloadUnused() {
			foreach (string clipName in nameOccurenceDict.Keys) {
				if (nameOccurenceDict[clipName] == 0) {
					Unload(clipName);
				}
			}
		}
		
		public void Activate(PureDataClip clip) {
			nameOccurenceDict[clip.Name] += 1;
		}
		
		public void Deactivate(PureDataClip clip) {
			nameOccurenceDict[clip.Name] -= 1;
		}
		
		public void BuildDicts() {
			nameClipDict = new Dictionary<string, PureDataClip>();
			nameOccurenceDict = new Dictionary<string, int>();
			
			foreach (PureDataClip clip in clips) {
				nameClipDict[clip.Name] = clip;
				nameOccurenceDict[clip.Name] = 0;
			}
		}
		
		public PureDataClip GetClip(string soundName) {
			PureDataClip clip = null;
			
			try {
				clip = nameClipDict[soundName];
			}
			catch {
				Logger.LogError(string.Format("Clip named {0} was not found.", soundName));
			}
			
			return clip;
		}

		public List<PureDataClip> GetAllClips() {
			return clips;
		}
	}
}
