using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo.AudioTools;

[System.Serializable]
public abstract class PureDataContainerItem : PureDataSourceOrContainerItem {

	protected PureDataContainerItem(PureData pureData)
		: base(pureData) {
	}
	
	public virtual PureDataContainerItem GetChildContainer(int containerIndex) {
		return GetChildrenContainers()[containerIndex];
	}
		
	public virtual PureDataContainerItem GetChildContainer(string containerName) {
		return Array.Find(GetChildrenContainers(), child => child.Name == containerName);
	}
		
	public abstract PureDataContainerItem[] GetChildrenContainers();
		
	public virtual PureDataSourceItem GetChildSource(int sourceIndex) {
		return GetChildrenSources()[sourceIndex];
	}
		
	public virtual PureDataSourceItem GetChildSource(string sourceName) {
		return Array.Find(GetChildrenSources(), child => child.Name == sourceName);
	}
		
	public abstract PureDataSourceItem[] GetChildrenSources();
		
	public virtual PureDataItem GetChildItem(int itemIndex) {
		return GetChildrenItems()[itemIndex];
	}
		
	public virtual PureDataItem GetChildItem(string itemName) {
		return Array.Find(GetChildrenItems(), child => child.Name == itemName);
	}
		
	public abstract PureDataItem[] GetChildrenItems();
}
