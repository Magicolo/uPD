using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public abstract class PureDataSpatializerDoppler : PureDataSpatializerBase {

		public abstract float DopplerLevel {
			get;
			set;
		}
		
		public abstract string DopplerSendName {
			get;
		}
		
		protected bool dopplerSkipped;
		protected bool dopplerInitialized;
		protected float lastDistance;
		
		protected PureDataSpatializerDoppler(PureData pureData)
			: base(pureData) {
		}
			
		public override void Spatialize(bool initialize = false) {
			base.Spatialize(initialize);
			
			if (Source == null || Source == pureData.listener) {
				SendDefaultDoppler();
				return;
			}
			
			dopplerInitialized = !initialize && dopplerInitialized;
			
			Doppler();
		}
		
		public virtual void Doppler() {
			if (skipped || !pureData.generalSettings.IsMainThread()) {
				SendSkippedDoppler();
				spatialize = true;
				return;
			}
			
			if (!dopplerInitialized) {
				lastDistance = distance;
			}
			
			float doppler = (pureData.generalSettings.speedOfSound + (lastDistance - distance) * DopplerLevel / Time.deltaTime) / pureData.generalSettings.speedOfSound;
			lastDistance = distance;
			SendDoppler(doppler);
			
			dopplerInitialized = true;
		}

		public virtual void SendDoppler(float doppler) {
			pureData.communicator.Send(DopplerSendName, doppler, dopplerInitialized ? 100 : 0);
		}
		
		public virtual void SendSkippedDoppler() {
			pureData.communicator.Send(DopplerSendName, 0, 0);
		}
		
		public virtual void SendDefaultDoppler() {
			pureData.communicator.Send(DopplerSendName, 1, 0);
		}
	}
}
