using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataContainer : INamable {
		
		public enum Types {
			MixContainer,
			RandomContainer,
			SwitchContainer
		}

		[SerializeField]
		string name;
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		public Types type;
		
		// Switch Container
		public PureDataSwitchSettings switchSettings;
		
		public List<PureDataSubContainer> subContainers = new List<PureDataSubContainer>();
		public int idCounter;
		public List<int> childrenIds = new List<int>();
		public PureData pureData;
		
		Dictionary<int, PureDataSubContainer> idDict;
		Dictionary<int, PureDataSubContainer> IdDict {
			get {
				if (!pureData.generalSettings.ApplicationPlaying || idDict == null) {
					BuildIDDict();
				}
				return idDict;
			}
		}
		
		public PureDataContainer(string name, PureData pureData) {
			this.name = name;
			this.pureData = pureData;
			
			switchSettings = new PureDataSwitchSettings(pureData);
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			switchSettings.Initialize(pureData);
			
			foreach (PureDataSubContainer subContainer in subContainers) {
				subContainer.Initialize(pureData);
			}
		}
		
		public void BuildIDDict() {
			idDict = new Dictionary<int, PureDataSubContainer>();
			
			foreach (PureDataSubContainer subContainer in subContainers) {
				idDict[subContainer.id] = subContainer;
			}
		}
		
		public int GetUniqueID() {
			idCounter += 1;
			return idCounter;
		}

		public PureDataSubContainer GetSubContainerWithID(int id) {
			if (!pureData.generalSettings.ApplicationPlaying && !IdDict.ContainsKey(id)) {
				return null;
			}
			
			PureDataSubContainer subContainer = null;
			
			try {
				subContainer = IdDict[id];
			}
			catch {
				Logger.LogError(string.Format("SubContainer with id {0} was not found.", id));
			}
			
			return subContainer;
		}

		public PureDataSubContainer[] IdsToSubContainers(List<int> ids) {
			List<PureDataSubContainer> childrenSubContainers = new List<PureDataSubContainer>();
			
			for (int i = 0; i < ids.Count; i++) {
				PureDataSubContainer childSubContainer = GetSubContainerWithID(ids[i]);
				if (childSubContainer != null) {
					childrenSubContainers.Add(childSubContainer);
				}
			}
			return childrenSubContainers.ToArray();
		}
	}
}