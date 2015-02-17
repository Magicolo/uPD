using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public abstract class PureDataReceiver {

		public readonly string sendName;
		public readonly bool asynchronous;
		public PureData pureData;
		
		protected PureDataReceiver(string sendName, bool asynchronous, PureData pureData) {
			this.sendName = sendName;
			this.asynchronous = asynchronous;
			this.pureData = pureData;
		}
		
		public abstract void Dequeue();
	}
}
