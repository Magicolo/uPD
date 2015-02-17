using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataBangReceiver : PureDataReceiver {

		public readonly BangReceiveCallback bangReceiver;
		public int queuedBangs;
		
		public PureDataBangReceiver(string sendName, BangReceiveCallback bangReceiver, bool asynchronous, PureData pureData)
			: base(sendName, asynchronous, pureData) {
			
			this.bangReceiver = bangReceiver;
		}
				
		public void Receive() {
			try {
				bangReceiver();
			}
			catch {
				pureData.communicator.Release(this);
			}
		}

		public void Enqueue() {
			queuedBangs += 1;
		}
		
		public override void Dequeue() {
			if (queuedBangs > 0) {
				Receive();
				queuedBangs -= 1;
			}
		}
	}
}
