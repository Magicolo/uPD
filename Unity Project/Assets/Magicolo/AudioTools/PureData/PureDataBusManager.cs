using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataBusManager : ScriptableObject {

		public string mixerPath;
		public PureDataBus[] buses = new PureDataBus[0];
		
		PureData pureData;
		
		Dictionary<string, PureDataBus> nameBusDict;
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			foreach (PureDataBus bus in buses) {
				bus.Initialize(pureData);
			}
		}
		
		public void Start() {
			BuildBusDict();
		}
		
		public void Update() {
			foreach (PureDataBus bus in buses) {
				bus.Update();
			}
		}
		
		public void UpdateMixer() {
			#if !UNITY_WEBPLAYER
			if (!SetMixerPath()) {
				return;
			}
			
			ThreadPool.QueueUserWorkItem(new WaitCallback(WriteToMixer));
			#endif
		}

		public void WriteToMixer(object state) {
			#if !UNITY_WEBPLAYER
			List<string> text = new List<string>();
			List<string> connections = new List<string>();
			
			text.Add("#N canvas 200 300 1000 300 10;");
			text.Add("#X obj 0 50 loadbang;");
			
			for (int i = 0; i < buses.Length; i++) {
				text.Add(string.Format("#X symbolatom {0} 0 10 0 0 0 - ubus_receiveName{1} ubus_sendName{1};", i * 110, buses[i].Name));
				text.Add(string.Format("#X msg {0} 70 symbol {1};", i * 110, buses[i].Name));
				text.Add(string.Format("#X obj {0} 90 s ubus_receiveName{1};", i * 110, buses[i].Name));
				text.Add(string.Format("#X obj {0} 110 Internal/ubus~ {1};", i * 110, buses[i].Name));
				text.Add(string.Format("#X obj {0} 130 outlet~ {1}_left;", i * 110, buses[i].Name));
				text.Add(string.Format("#X obj {0} 130 outlet~ {1}_right;", i * 110 + 63, buses[i].Name));
				connections.Add(string.Format("#X connect {0} 0 {1} 0;", 0, i * 6 + 2)); // loadbang to message box
				connections.Add(string.Format("#X connect {0} 0 {1} 0;", i * 6 + 2, i * 6 + 3)); // message box to send
				connections.Add(string.Format("#X connect {0} 0 {1} 0;", i * 6 + 4, i * 6 + 5)); // ubus~ to left outlet~
				connections.Add(string.Format("#X connect {0} 1 {1} 0;", i * 6 + 4, i * 6 + 6)); // ubus~ to right outlet~
			}
			
			text.Add(string.Format("#X obj {0} 130 outlet;", buses.Length * 110));
			text.AddRange(connections);
			text.Add(string.Format("#X coords 0 -1 1 1 {0} 19 2 0 0;", buses.Length * 110 + 8));
			
			File.WriteAllLines(mixerPath, text.ToArray());
			#endif
		}
		
		public void SetBusVolume(string busName, float targetVolume, float time = 0, float delay = 0) {
			GetBus(busName).SetVolume(targetVolume, time, delay);
			pureData.editorHelper.RepaintInspector();
		}
		
		public bool SetMixerPath() {
			#if !UNITY_WEBPLAYER
			if (string.IsNullOrEmpty(mixerPath) || !File.Exists(mixerPath) || !HelperFunctions.PathIsRelativeTo(mixerPath, Application.streamingAssetsPath)) {
				mixerPath = Path.GetFullPath(HelperFunctions.GetAssetPath("umixer~.pd"));
			}
			
			if (!File.Exists(mixerPath)) {
				Logger.LogError("Can not find umixer~.pd patch.");
				return false;
			}
			#endif
			
			return true;
		}
		
		public void BuildBusDict() {
			nameBusDict = new Dictionary<string, PureDataBus>();
			
			foreach (PureDataBus bus in buses) {
				nameBusDict[bus.Name] = bus;
			}
		}
		
		public PureDataBus GetBus(string busName) {
			PureDataBus bus = null;
			
			try {
				bus = pureData.generalSettings.ApplicationPlaying ? nameBusDict[busName] : System.Array.Find(buses, b => b.Name == busName);
			}
			catch {
				Logger.LogError(string.Format("Bus named {0} was not found.", busName));
			}
			
			return bus;
		}
		
		public static PureDataBusManager Create(string path) {
			return HelperFunctions.GetOrAddAssetOfType<PureDataBusManager>("Buses", path);
		}
	}
}
