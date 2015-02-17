using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataContainerItemInternal : PureDataContainerItem {

		string name;
		public override string Name {
			get {
				return name;
			}
		}

		PureDataStates state;
		public override PureDataStates State {
			get {
				if (items.Count == 0 || items.TrueForAll(i => i.State == PureDataStates.Stopped)) {
					state = PureDataStates.Stopped;
				}
				else if (items.TrueForAll(i => i.State == PureDataStates.Waiting)) {
					state = PureDataStates.Waiting;
				}
				else if (items.TrueForAll(i => i.State == PureDataStates.Paused)) {
					state = PureDataStates.Paused;
				}
				else {
					state = PureDataStates.Playing;
				}
				
				return state;
			}
		}
		
		public List<PureDataSourceOrContainerItem> items = new List<PureDataSourceOrContainerItem>();
		
		public PureDataContainerItemInternal(string name, PureData pureData)
			: base(pureData) {
			
			this.name = name;
		}

		public virtual void AddItem(PureDataSourceOrContainerItem item) {
			items.Add(item);
		}
		
		public virtual void RemoveItem(PureDataSourceOrContainerItem item) {
			items.Remove(item);
		}
		
		public virtual void ExecuteOnItems(System.Action<PureDataSourceOrContainerItem> action) {
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

		public override void Play(float delay) {
			ExecuteOnItems(item => item.Play(delay));
		}

		public override void Pause(float delay) {
			ExecuteOnItems(item => item.Pause(delay));
		}

		public override void Stop(float delay) {
			ExecuteOnItems(item => item.Stop(delay));
		}

		public override void StopImmediate() {
			ExecuteOnItems(item => item.StopImmediate());
		}

		public override void Load() {
			ExecuteOnItems(item => item.Load());
		}
			
		public override void Unload() {
			ExecuteOnItems(item => item.Unload());
		}
	
		public override PureDataContainerItem[] GetChildrenContainers() {
			List<PureDataContainerItem> containers = new List<PureDataContainerItem>();
			
			foreach (PureDataSourceOrContainerItem item in items) {
				if (item is PureDataContainerItem) {
					containers.Add(item as PureDataContainerItem);
				}
			}
			
			return containers.ToArray();
		}

		public override PureDataSourceItem[] GetChildrenSources() {
			List<PureDataSourceItem> sources = new List<PureDataSourceItem>();
			
			foreach (PureDataSourceOrContainerItem item in items) {
				if (item is PureDataSourceItem) {
					sources.Add(item as PureDataSourceItem);
				}
			}
			
			return sources.ToArray();
		}

		public override PureDataItem[] GetChildrenItems() {
			return items.ToArray();
		}

		public override void ApplyOptions(params PureDataOption[] options) {
			ExecuteOnItems(item => item.ApplyOptions(options));
		}
		
		public override string ToString() {
			return string.Format("{0}({1}, {2}, {3})", typeof(PureDataContainerItem).Name, Name, State, Logger.ObjectToString(items));
		}
	}
}