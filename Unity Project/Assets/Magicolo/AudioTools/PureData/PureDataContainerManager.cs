using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataContainerManager : ScriptableObject {

		public PureDataContainer[] containers = new PureDataContainer[0];
		public PureData pureData;
		
		Dictionary<string, PureDataContainer> containerDict;
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			foreach (PureDataContainer container in containers) {
				container.Initialize(pureData);
			}
		}
		
		public void Start() {
			BuildContainerDict();
		}
		
		public void BuildContainerDict() {
			containerDict = new Dictionary<string, PureDataContainer>();
			
			foreach (PureDataContainer container in containers) {
				containerDict[container.Name] = container;
			}
		}
		
		public PureDataContainer GetContainer(string containerName) {
			PureDataContainer container = null;
			
			try {
				container = containerDict[containerName];
			}
			catch {
				Logger.LogError(string.Format("Container named {0} was not found.", containerName));
			}
			
			return container;
		}
		
		public static PureDataContainerManager Create(string path) {
			return HelperFunctions.GetOrAddAssetOfType<PureDataContainerManager>("Containers", path);
		}
	}
}
