using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;
using LibPDBinding;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataPatchManager {

		public PureData pureData;
	
		Dictionary<string, int> patchIdDict = new Dictionary<string, int>();

		public PureDataPatchManager(PureData pureData) {
			this.pureData = pureData;
		}
	
		public void Open(params string[] patchesName) {
			foreach (string patchName in patchesName) {
				if (!patchIdDict.ContainsKey(Path.GetFileName(patchName))) {
					string path = GetPatchPath(patchName);
					patchIdDict[Path.GetFileName(patchName)] = LibPD.OpenPatch(path);
					pureData.communicator.Initialize();
					pureData.busManager.Update();
					pureData.spatializerManager.Update();
				}
			}
			
			LibPD.ComputeAudio(true);
		}
	
		public void Close(params string[] patchesName) {
			foreach (string patchName in patchesName) {
				if (patchIdDict.ContainsKey(patchName)) {
					LibPD.ClosePatch(patchIdDict[patchName]);
					patchIdDict.Remove(patchName);
				}
			}
		}
		
		public void CloseAll() {
			foreach (string key in new List<string>(patchIdDict.Keys)) {
				Close(key);
			}
		}

		public bool IsOpened(string patchName) {
			return patchIdDict.ContainsKey(patchName);
		}
		
		public string GetPatchPath(string patchName) {
			string path = Application.streamingAssetsPath + Path.AltDirectorySeparatorChar + pureData.generalSettings.patchesPath + Path.AltDirectorySeparatorChar + patchName + ".pd";
		
			#if UNITY_ANDROID && !UNITY_EDITOR
			string patchJar = Application.persistentDataPath + Path.AltDirectorySeparatorChar + patchName + ".pd";
			
			if (File.Exists(patchJar)) {
				Logger.Log(string.Format("Patch {0} already unpacked.", patchName));
				File.Delete(patchJar);
				
				if (File.Exists(patchJar)) {
					Logger.LogError(string.Format("Couldn't delete file at {0}.", patchJar));
				}
			}
			
			WWW dataStream = new WWW(path);
			
			// Hack to wait till download is done
			while (!dataStream.isDone) {
			}
			
			if (!string.IsNullOrEmpty(dataStream.error)) {
				Logger.LogError("### WWW ERROR IN DATA STREAM:" + dataStream.error);
			}
			
			File.WriteAllBytes(patchJar, dataStream.bytes);
			
			path = patchJar;
			#endif
		
			return path;
		}

		public void Start() {
			Open(pureData.generalSettings.resourcesPath.GetRange(pureData.generalSettings.patchesPath.Length + 1, '.'));
		}
		
		public void Stop() {
			CloseAll();
		}
	}
}