using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataListReceiver : PureDataReceiver {
		
		public readonly ListReceiveCallback listReceiver;
		public readonly Queue<object[]> queuedLists = new Queue<object[]>();
		
		public PureDataListReceiver(string sendName, ListReceiveCallback listReceiver, bool asynchronous, PureData pureData)
			: base(sendName, asynchronous, pureData) {
			
			this.listReceiver = listReceiver;
		}
								
		public void Receive(object[] values) {
			try {
				listReceiver(values);
			}
			catch {
				pureData.communicator.Release(this);
			}
		}

		public void Enqueue(object[] values) {
			queuedLists.Enqueue(values);
		}
		
		public override void Dequeue() {
			if (queuedLists.Count > 0) {
				Receive(queuedLists.Dequeue());
			}
		}
	}
}
