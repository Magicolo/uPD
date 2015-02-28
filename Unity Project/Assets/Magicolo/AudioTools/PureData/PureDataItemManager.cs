using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using LibPDBinding;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataItemManager {
		
		public PureData pureData;
		
		public PureDataItemManager(PureData pureData) {
			this.pureData = pureData;
		}

		public PureDataSourceItem Play(string soundName, object source, float delay, params PureDataOption[] options) {
			PureDataSourceItem item = GetSourceItem(soundName, source);
			item.ApplyOptions(options);
			item.Play(delay);
			return item;
		}

		public PureDataContainerItem PlayContainer(string containerName, object source, float delay, params PureDataOption[] options) {
			PureDataContainerItemInternal item = GetContainerItem(containerName, source);
			item.ApplyOptions(options);
			item.Play(delay);
			return item;
		}

		public PureDataSequenceItem PlaySequence(string sequenceName, object source, float delay, params PureDataOption[] options) {
			PureDataSequenceItem item = GetSequenceItem(sequenceName, source);
			item.ApplyOptions(options);
			item.Play(delay);
			return item;
		}
		
		public PureDataSourceItem GetSourceItem(string soundName, object source) {
			return new PureDataSourceItem(pureData.sourceManager.GetSource(soundName, source), pureData);
		}
			
		public PureDataSourceItem GetSourceItem(PureDataSubContainer subContainer, object source) {
			PureDataSourceItem sourceAudioItem = null;
			
			switch (subContainer.type) {
				default:
					sourceAudioItem = GetSourceItem(subContainer.infoName, source);
					sourceAudioItem.ApplyOptions(subContainer.options);
					break;
			}
			return sourceAudioItem;
		}
			
		public PureDataContainerItemInternal GetContainerItem(string containerName, object source) {
			return GetContainerItem(pureData.containerManager.GetContainer(containerName), source);
		}
		
		public PureDataContainerItemInternal GetContainerItem(PureDataContainer container, object source) {
			PureDataContainerItemInternal multipleItem;
			
			switch (container.type) {
				case PureDataContainer.Types.RandomContainer:
					multipleItem = GetRandomItem(container, container.childrenIds, source);
					break;
				case PureDataContainer.Types.SwitchContainer:
					multipleItem = GetSwitchItem(container, container.childrenIds, source);
					break;
				default:
					multipleItem = GetMixItem(container, container.childrenIds, source);
					break;
			}
			return multipleItem;
		}
		
		public PureDataContainerItemInternal GetContainerItem(PureDataContainer container, PureDataSubContainer subContainer, object source) {
			PureDataContainerItemInternal multipleAudioItem = null;
			
			switch (subContainer.type) {
				case PureDataSubContainer.Types.RandomContainer:
					multipleAudioItem = GetRandomItem(container, subContainer.childrenIds, source);
					break;
				case PureDataSubContainer.Types.SwitchContainer:
					multipleAudioItem = GetSwitchItem(container, subContainer.childrenIds, source);
					break;
				default:
					multipleAudioItem = GetMixItem(container, subContainer.childrenIds, source);
					break;
			}
			return multipleAudioItem;
		}
		
		public PureDataSourceOrContainerItem GetSubContainerItem(PureDataContainer container, PureDataSubContainer subContainer, object source) {
			PureDataSourceOrContainerItem item = null;
			
			if (subContainer.IsSource) {
				item = GetSourceItem(subContainer, source);
			}
			else {
				item = GetContainerItem(container, subContainer, source);
			}
			return item;
		}

		public PureDataContainerItemInternal GetMixItem(PureDataContainer container, List<int> childrenIds, object source) {
			PureDataContainerItemInternal mixAudioItem = new PureDataContainerItemInternal(container.Name, pureData);
			
			foreach (int childrenId in childrenIds) {
				PureDataSourceOrContainerItem childItem = GetSubContainerItem(container, container.GetSubContainerWithID(childrenId), source);
				if (childItem != null) {
					mixAudioItem.AddItem(childItem);
				}
			}
			
			return mixAudioItem;
		}
		
		public PureDataContainerItemInternal GetRandomItem(PureDataContainer container, List<int> childrenIds, object source) {
			PureDataContainerItemInternal randomAudioItem = new PureDataContainerItemInternal(container.Name, pureData);
			List<PureDataSubContainer> childcontainers = new List<PureDataSubContainer>();
			List<float> weights = new List<float>();
			
			for (int i = 0; i < childrenIds.Count; i++) {
				PureDataSubContainer childContainer = container.GetSubContainerWithID(childrenIds[i]);
				if (childContainer != null) {
					childcontainers.Add(childContainer);
					weights.Add(childContainer.weight);
				}
			}
			
			PureDataSubContainer randomChildContainer = HelperFunctions.WeightedRandom(childcontainers, weights);
			if (randomAudioItem != null) {
				PureDataSourceOrContainerItem childAudioItem = GetSubContainerItem(container, randomChildContainer, source);
				if (childAudioItem != null) {
					randomAudioItem.AddItem(childAudioItem);
				}
			}
			
			return randomAudioItem;
		}
		
		public PureDataContainerItemInternal GetSwitchItem(PureDataContainer container, List<int> childrenIds, object source) {
			PureDataContainerItemInternal switchAudioItem = new PureDataContainerItemInternal(container.Name, pureData);
			int stateIndex = int.MinValue;
			PureDataSubContainer[] childrenSubContainers = container.IdsToSubContainers(childrenIds);
			
			if (childrenSubContainers[0].parentId == 0) {
				stateIndex = container.switchSettings.GetCurrentStateIndex();
			}
			else {
				PureDataSubContainer parentSubContainer = container.GetSubContainerWithID(childrenSubContainers[0].parentId);
				stateIndex = parentSubContainer.switchSettings.GetCurrentStateIndex();
			}
			
			if (stateIndex != int.MinValue) {
				foreach (PureDataSubContainer childSubContainer in childrenSubContainers) {
					if (childSubContainer.switchSettings.stateIndex == stateIndex) {
						PureDataSourceOrContainerItem childAudioItem = GetSubContainerItem(container, childSubContainer, source);
						
						if (childAudioItem != null) {
							switchAudioItem.AddItem(childAudioItem);
						}
					}
				}
			}
			
			return switchAudioItem;
		}
	
		public PureDataSequenceItem GetSequenceItem(string sequenceName, object source) {
			return new PureDataSequenceItem(pureData.sequenceManager.GetSequence(sequenceName, source), pureData);
		}
	}
}
