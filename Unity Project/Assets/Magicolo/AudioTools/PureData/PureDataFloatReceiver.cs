using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataFloatReceiver : PureDataReceiver {
		
		public readonly FloatReceiveCallback floatReceiver;
		public readonly Queue<float> queuedFloats = new Queue<float>();
		
		public PureDataFloatReceiver(string sendName, FloatReceiveCallback floatReceiver, bool asynchronous, PureData pureData)
			: base(sendName, asynchronous, pureData) {
			
			this.floatReceiver = floatReceiver;
		}
		
		public void Receive(float value) {
			try {
				floatReceiver(value);
			}
			catch {
				pureData.communicator.Release(this);
			}
		}

		public void Enqueue(float value) {
			queuedFloats.Enqueue(value);
		}
		
		public override void Dequeue() {
			if (queuedFloats.Count > 0) {
				Receive(queuedFloats.Dequeue());
			}
		}
	}
}
