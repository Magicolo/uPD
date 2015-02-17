using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public abstract class PureDataSourceOrContainerItem : PureDataItem {

		protected PureDataSourceOrContainerItem(PureData pureData)
			: base(pureData) {
		}
		
		/// <summary>
		/// Pauses an item.
		/// </summary>
		/// <param name = "delay">The delay in seconds before pausing the item.</param>	
		public abstract void Pause(float delay);
	
		/// <summary>
		/// Pauses an item.
		/// </summary>
		public virtual void Pause() {
			Pause(0);
		}

		/// <summary>
		/// Loads the sounds attached to the item in memory.
		/// </summary>
		public abstract void Load();
	
		/// <summary>
		/// Unloads the sounds attached to the item in memory.
		/// </summary>
		/// <remarks>Unloading a sound that is playing will stop it and might generate clicks.</remarks>
		public abstract void Unload();
	}
}