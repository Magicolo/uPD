using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataInfoManager {
		
		public List<PureDataInfo> infos = new List<PureDataInfo>();
		public PureData pureData;
		
		Dictionary<string, PureDataInfo> nameInfoDict;
		
		public PureDataInfoManager(PureData pureData) {
			this.pureData = pureData;
			
			UpdateInfos();
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}

		public void Start() {
			BuildInfoDict();
		}
		
		public void UpdateInfos() {
			foreach (AudioClip clip in Resources.LoadAll<AudioClip>("")) {
				PureDataInfo info = infos.Find(i => i.Name == clip.name);
				
				if (info == null) {
					infos.Add(new PureDataInfo(clip, pureData));
				}
				else {
					info.Update(clip);
				}
			}
		}

		public void BuildInfoDict() {
			nameInfoDict = new Dictionary<string, PureDataInfo>();
			
			foreach (PureDataInfo info in infos) {
				nameInfoDict[info.Name] = info;
			}
		}

		public PureDataInfo GetInfo(string clipName) {
			PureDataInfo info = null;

			try {
				info = pureData.generalSettings.ApplicationPlaying ? nameInfoDict[clipName] : infos.Find(i => i.Name == clipName);
			}
			catch {
				Logger.LogError(string.Format("Info named {0} was not found.", clipName));
			}
			
			return info;
		}
		
		public int GetInfoIndex(string clipName) {
			return infos.IndexOf(GetInfo(clipName));
		}
		
		public int GetInfoIndex(PureDataInfo info) {
			return infos.IndexOf(info);
		}
		
		public void SetInfo(string clipName, PureDataInfo info) {
			if (pureData.generalSettings.ApplicationPlaying) {
				nameInfoDict[clipName] = info;
			}
			else {
				infos[GetInfoIndex(clipName)] = info;
			}
		}
		
		public List<PureDataInfo> GetAllClipInfos() {
			return infos;
		}
		
		public static void Switch(PureDataInfoManager source, PureDataInfoManager target) {
			source.infos = target.infos;
			
			source.Start();
		}
	}
}
