using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataContainerItemInternal : PureDataContainerItem {

		public PureDataContainerItemInternal(string name, PureData pureData)
			: base(name, pureData) {
		}

		public void AddItem(PureDataSourceOrContainerItem item) {
			items.Add(item);
		}
		
		public void RemoveItem(PureDataSourceOrContainerItem item) {
			items.Remove(item);
		}
	
		protected override void ExecuteOnItems(Action<PureDataSourceOrContainerItem> action) {
			for (int i = items.Count - 1; i >= 0; i--) {
				PureDataSourceOrContainerItem item = items[i];
				
				if (item.State == PureDataStates.Stopped) {
					RemoveItem(item);
				}
				else {
					action(item);
				}
			}
		}
	}
}