using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSequenceSpatializer : PureDataSpatializerBase {

		[SerializeField]
		PureDataVolumeRolloffModes volumeRolloffMode;
		public override PureDataVolumeRolloffModes VolumeRolloffMode {
			get {
				return volumeRolloffMode;
			}
			set {
				volumeRolloffMode = value;
				spatialize = true;
			}
		}

		[SerializeField]
		[Min] float minDistance = 1;
		public override float MinDistance {
			get {
				return minDistance;
			}
			set {
				minDistance = value;
				spatialize = true;
			}
		}

		[SerializeField]
		[Min] float maxDistance = 500;
		public override float MaxDistance {
			get {
				return maxDistance;
			}
			set {
				maxDistance = value;
				spatialize = true;
			}
		}

		[SerializeField]
		[Range(0, 1)] float panLevel = 0.75F;
		public override float PanLevel {
			get {
				return panLevel;
			}
			set {
				panLevel = value;
				spatialize = true;
			}
		}

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

		public PureDataSequenceSpatializer(PureData pureData)
			: base(pureData) {
		}
	
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void UpdateSendNames(PureDataSequence sequence) {
			panLeftSendName = "usequence_pan_left" + sequence.Id;
			panRightSendName = "usequence_pan_right" + sequence.Id;
		}
		
		public override void SendPan(float panLeft, float panRight) {
			pureData.communicator.Send(PanLeftSendName, panLeft, 10);
			pureData.communicator.Send(PanRightSendName, panRight, 10);
		}
		
		public override void SendDefaultPan() {
			pureData.communicator.Send(PanLeftSendName, 1, 10);
			pureData.communicator.Send(PanRightSendName, 1, 10);
		}
		
		public override void SendSkippedPan() {
			pureData.communicator.Send(PanLeftSendName, 0, 10);
			pureData.communicator.Send(PanRightSendName, 0, 10);
		}
	}
}