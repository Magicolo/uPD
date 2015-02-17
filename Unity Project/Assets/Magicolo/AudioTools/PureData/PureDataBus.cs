using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataBus : INamable {

		[SerializeField, PropertyField]
		string name;
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 5)]
		float volume = 1;
		public float Volume {
			get {
				return volume;
			}
			set {
				volume = value;
				if (pureData.generalSettings.ApplicationPlaying) {
					pureData.communicator.Send("ubus_volume" + Name, volume, 10);
				}
			}
		}
		
		public PureData pureData;
		
		public PureDataBus(PureData pureData) {
			this.pureData = pureData;
		}

		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Update() {
			pureData.communicator.Send("ubus_receiveName" + Name, Name);
			pureData.communicator.Send("ubus_volume" + Name, volume, 0);
		}
				
		public void SetVolume(float targetVolume, float time = 0, float delay = 0) {
			volume = Mathf.Max(targetVolume, 0);
			time = Mathf.Max(time, 0.01F);
			delay = Mathf.Max(delay, 0);
			
			if (delay > 0) {
				pureData.communicator.Send("uresources_messagedelayer_ff", volume, time * 1000, "ubus_volume" + Name, delay * 1000);
			}
			else {
				pureData.communicator.Send("ubus_volume" + Name, volume, time * 1000);
			}
		}
	}
}
