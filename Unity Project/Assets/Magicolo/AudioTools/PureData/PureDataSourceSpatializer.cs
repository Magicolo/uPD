using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSourceSpatializer : PureDataSpatializerDoppler {

		object source;
		public override object Source {
			get {
				return source;
			}
			set {
				source = value;
				Spatialize(true);
				spatialize = true;
			}
		}

		public override PureDataVolumeRolloffModes VolumeRolloffMode {
			get {
				return audioSource.Info.volumeRolloffMode;
			}
			set {
				audioSource.Info.volumeRolloffMode = value;
				spatialize = true;
			}
		}

		public override float MinDistance {
			get {
				return audioSource.Info.minDistance;
			}
			set {
				audioSource.Info.minDistance = value;
				spatialize = true;
			}
		}

		public override float MaxDistance {
			get {
				return audioSource.Info.maxDistance;
			}
			set {
				audioSource.Info.maxDistance = value;
				spatialize = true;
			}
		}

		public override float PanLevel {
			get {
				return audioSource.Info.panLevel;
			}
			set {
				audioSource.Info.panLevel = value;
				spatialize = true;
			}
		}

		public override float DopplerLevel {
			get {
				return audioSource.Info.dopplerLevel;
			}
			set {
				audioSource.Info.dopplerLevel = value;
				spatialize = true;
			}
		}

		string panLeftSendName;
		public override string PanLeftSendName {
			get {
				return panLeftSendName;
			}
		}

		string panRightSendName;
		public override string PanRightSendName {
			get {
				return panRightSendName;
			}
		}

		string dopplerSendName;
		public override string DopplerSendName {
			get {
				return dopplerSendName;
			}
		}

		[System.NonSerialized] readonly PureDataSource audioSource;
		
		public PureDataSourceSpatializer(PureDataSource audioSource, PureData pureData)
			: base(pureData) {
			
			this.audioSource = audioSource;
			UpdateSendNames();
		}
	
		public void UpdateSendNames() {
			panLeftSendName = "uaudiosource_pan_left" + audioSource.Id;
			panRightSendName = "uaudiosource_pan_right" + audioSource.Id;
			dopplerSendName = "uaudiosource_doppler" + audioSource.Id;
		}
	}
}
