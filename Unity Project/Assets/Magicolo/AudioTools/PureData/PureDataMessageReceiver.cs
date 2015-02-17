using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataMessageReceiver : PureDataReceiver {
		
		public readonly MessageReceiveCallback messageReceiver;
		public readonly Queue<string> queuedMessages = new Queue<string>();
		public readonly Queue<object[]> queuedValues = new Queue<object[]>();
		
		public PureDataMessageReceiver(string sendName, MessageReceiveCallback messageReceiver, bool asynchronous, PureData pureData)
			: base(sendName, asynchronous, pureData) {
			
			this.messageReceiver = messageReceiver;
		}
										
		public void Receive(string message, object[] values) {
			try {
				messageReceiver(message, values);
			}
			catch {
				pureData.communicator.Release(this);
			}
		}

		public void Enqueue(string message, object[] values) {
			queuedMessages.Enqueue(message);
			queuedValues.Enqueue(values);
		}
		
		public override void Dequeue() {
			if (queuedMessages.Count > 0) {
				Receive(queuedMessages.Dequeue(), queuedValues.Dequeue());
			}
		}
	}
}
