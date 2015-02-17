using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSymbolReceiver : PureDataReceiver {
		
		public readonly SymbolReceiveCallback symbolReceiver;
		public readonly Queue<string> queuedSymbols = new Queue<string>();
		
		public PureDataSymbolReceiver(string sendName, SymbolReceiveCallback symbolReceiver, bool asynchronous, PureData pureData)
			: base(sendName, asynchronous, pureData) {
			
			this.symbolReceiver = symbolReceiver;
		}
						
		public void Receive(string value) {
			try {
				symbolReceiver(value);
			}
			catch {
				pureData.communicator.Release(this);
			}
		}

		public void Enqueue(string value) {
			queuedSymbols.Enqueue(value);
		}
		
		public override void Dequeue() {
			if (queuedSymbols.Count > 0) {
				Receive(queuedSymbols.Dequeue());
			}
		}
	}
}
