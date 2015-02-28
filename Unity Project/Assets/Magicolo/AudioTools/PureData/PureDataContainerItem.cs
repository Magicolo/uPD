using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo.AudioTools;

[System.Serializable]
public class PureDataContainerItem : PureDataSourceOrContainerItem {

	protected List<PureDataSourceOrContainerItem> items = new List<PureDataSourceOrContainerItem>();
	
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
		
	public PureDataContainerItem(string name, PureData pureData)
		: base(pureData) {
			
		this.name = name;
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
	
	public PureDataContainerItem[] GetChildrenContainers() {
		List<PureDataContainerItem> containers = new List<PureDataContainerItem>();
			
		foreach (PureDataSourceOrContainerItem item in items) {
			if (item is PureDataContainerItem) {
				containers.Add(item as PureDataContainerItem);
			}
		}
			
		return containers.ToArray();
	}

	public PureDataContainerItem GetChildContainer(int containerIndex) {
		return GetChildrenContainers()[containerIndex];
	}
		
	public PureDataContainerItem GetChildContainer(string containerName) {
		return Array.Find(GetChildrenContainers(), child => child.Name == containerName);
	}
	
	public PureDataSourceItem[] GetChildrenSources() {
		List<PureDataSourceItem> sources = new List<PureDataSourceItem>();
			
		foreach (PureDataSourceOrContainerItem item in items) {
			if (item is PureDataSourceItem) {
				sources.Add(item as PureDataSourceItem);
			}
		}
			
		return sources.ToArray();
	}
	
	public PureDataSourceItem GetChildSource(int sourceIndex) {
		return GetChildrenSources()[sourceIndex];
	}
		
	public PureDataSourceItem GetChildSource(string sourceName) {
		return Array.Find(GetChildrenSources(), child => child.Name == sourceName);
	}
	
	public PureDataItem[] GetChildrenItems() {
		return items.ToArray();
	}
	
	public PureDataItem GetChildItem(int itemIndex) {
		return GetChildrenItems()[itemIndex];
	}
		
	public PureDataItem GetChildItem(string itemName) {
		return Array.Find(GetChildrenItems(), child => child.Name == itemName);
	}
	
	public override void ApplyOptions(params PureDataOption[] options) {
		ExecuteOnItems(item => item.ApplyOptions(options));
	}
		
	public override string ToString() {
		return string.Format("{0}({1}, {2}, {3})", typeof(PureDataContainerItem).Name, Name, State, Logger.ObjectToString(items));
	}
	
	protected virtual void ExecuteOnItems(Action<PureDataSourceOrContainerItem> action) {
	}
}
