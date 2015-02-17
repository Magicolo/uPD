using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Collections;
using LibPDBinding;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSourceManager : PureDataIdManager<PureDataSource> {

		public string containerPath;
		public PureData pureData;
		
		List<PureDataSource> activeSources;
		List<PureDataSource> inactiveSources;
		List<PureDataSource> sourcesToDeactivate;
	
		public PureDataSourceManager(PureData pureData) {
			this.pureData = pureData;
			
			UpdateSourceContainer();
		}
			
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}

		public void InitializeSources() {
			activeSources = new List<PureDataSource>(); // Must be initialized here or some unknown origin error occurs.
			inactiveSources = new List<PureDataSource>();
			sourcesToDeactivate = new List<PureDataSource>();
		
			for (int i = 0; i < pureData.generalSettings.MaxVoices; i++) {
				PureDataSource source = new PureDataSource(pureData);
				SetUniqueId(source);
				inactiveSources.Add(source);
			}
		}

		public void Start() {
			InitializeSources();
			pureData.communicator.Receive("uaudiosource_fadeout", FadeOutSourceReceiver, true);
			pureData.communicator.Receive("uaudiosource_stop", StopSourceReceiver, true);
		}
		
		public void Update() {
			for (int i = sourcesToDeactivate.Count - 1; i >= 0; i--) {
				PureDataSource source = sourcesToDeactivate.PopLast();
				activeSources.Remove(source);
				inactiveSources.Add(source);
				pureData.clipManager.Deactivate(source.Clip);
			}
			
			for (int i = activeSources.Count - 1; i >= 0; i--) {
				activeSources[i].Update();
			}
		}
		
		public void UpdateSourceContainer() {
			#if !UNITY_WEBPLAYER
			if (!SetContainerPath()) {
				return;
			}
			
			ThreadPool.QueueUserWorkItem(new WaitCallback(WriteToSourceContainer));
			#endif
		}
		
		public void WriteToSourceContainer(object state) {
			#if !UNITY_WEBPLAYER
			List<string> text = new List<string>();
			
			text.Add("#N canvas 200 300 450 300 10;");
			for (int i = 1; i <= pureData.generalSettings.MaxVoices; i++) {
				text.Add(string.Format("#X obj 0 0 uaudiosource {0};", i));
			}
			
			File.WriteAllLines(containerPath, text.ToArray());
			#endif
		}
		
		public void Activate(PureDataSource source) {
			inactiveSources.Remove(source);
			activeSources.Add(source);
			
			pureData.clipManager.Activate(source.Clip);
		}
					
		public void Deactivate(PureDataSource source) {
			sourcesToDeactivate.Add(source);
		}
		
		public bool SetContainerPath() {
			#if !UNITY_WEBPLAYER
			if (string.IsNullOrEmpty(containerPath) || !File.Exists(containerPath) || !HelperFunctions.PathIsRelativeTo(containerPath, Application.streamingAssetsPath)) {
				containerPath = Path.GetFullPath(HelperFunctions.GetAssetPath("uaudiosourcecontainer.pd"));
			}
			
			if (!File.Exists(containerPath)) {
				Logger.LogError("Can not find uaudiosourcecontainer.pd patch.");
				return false;
			}
			#endif
			
			return true;
		}
		
		public PureDataSource GetSource(string soundName, object source) {
			PureDataSource audioSource = null;
			
			if (inactiveSources.Count > 0) {
				audioSource = inactiveSources.PopLast();
				audioSource.SetClip(pureData.clipManager.GetClip(soundName));
				audioSource.SetSource(source);
			}
			else {
				Logger.LogError("No available source was found.");
			}
			
			return audioSource;
		}
	
		public void StopAll(float delay) {
			activeSources.ForEach(source => source.Stop(delay));
		}
		
		public void StopAllImmediate() {
			activeSources.ForEach(source => source.StopImmediate());
		}
		
		public void FadeOutSourceReceiver(float sourceId) {
			GetIdentifiableWithId((int)sourceId).Stop();
		}
		
		public void StopSourceReceiver(float sourceId) {
			GetIdentifiableWithId((int)sourceId).StopImmediate();
		}
	}
}