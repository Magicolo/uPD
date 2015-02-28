using System.IO;
using System.Threading;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataGeneralSettings : ScriptableObject {
		
		[SerializeField, PropertyField(typeof(MinAttribute))]
		int maxVoices = 100;
		public int MaxVoices {
			get {
				return maxVoices;
			}
			set {
				if (!ApplicationPlaying) {
					maxVoices = value;
					pureData.sourceManager.UpdateSourceContainer();
				}
			}
		}
		
		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 1)]
		float masterVolume = 1;
		public float MasterVolume {
			get {
				return masterVolume;
			}
			set {
				masterVolume = value;
				
				if (ApplicationPlaying) {
					SetMasterVolume(masterVolume);
				}
			}
		}
		
		bool applicationPlaying;
		public bool ApplicationPlaying {
			get {
				return applicationPlaying;
			}
		}

		bool applicationIsEditor;
		public bool ApplicationIsEditor {
			get {
				return applicationIsEditor;
			}
		}

		Thread mainThread;
		public Thread MainThread {
			get {
				return mainThread;
			}
		}
		
		public string patchesPath = "Patches";
		public string resourcesPath = "";
		public float speedOfSound = 343;
		public PureData pureData;

		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			InitializeSettings();
			
			if (!Application.isPlaying) {
				SetResourcesPath();
			}
		}
		
		public void InitializeSettings() {
			applicationPlaying = Application.isPlaying;
			applicationIsEditor = Application.isEditor;
			mainThread = Thread.CurrentThread;
		}
		
		public void SetDefaultValues() {
			patchesPath = "Patches";
			resourcesPath = "";
			maxVoices = 100;
			speedOfSound = 343;
			masterVolume = 1;
		}
		
		public void SetResourcesPath() {
			#if UNITY_EDITOR
			if (string.IsNullOrEmpty(resourcesPath) || !File.Exists(Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + resourcesPath)){
				resourcesPath = HelperFunctions.GetAssetPath("uresources.pd").GetRange("Assets/StreamingAssets/".Length);
			}
			#endif
		}
		
		public void SetMasterVolume(float targetVolume, float time = 0, float delay = 0) {
			masterVolume = Mathf.Clamp01(targetVolume);
			time = Mathf.Max(time, 0.01F);
			delay = Mathf.Max(delay, 0);
			
			pureData.communicator.SendDelayedMessage("umastervolume", delay, masterVolume, time * 1000);
			pureData.editorHelper.RepaintInspector();
		}

		public bool IsMainThread() {
			return Thread.CurrentThread == MainThread;
		}
		
		public static PureDataGeneralSettings Create(string path) {
			PureDataGeneralSettings generalSettings = HelperFunctions.GetOrAddAssetOfType<PureDataGeneralSettings>("General", path);
			generalSettings.SetDefaultValues();
			return generalSettings;
		}
	}
}
