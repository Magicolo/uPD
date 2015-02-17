using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSubContainer : INamable, IShowable {

		public enum Types {
			AudioSource,
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
		
		[SerializeField]
		bool showing;
		public bool Showing {
			get {
				return showing;
			}
			set {
				showing = value;
			}
		}
		
		public bool IsSource {
			get {
				return type == Types.AudioSource;
			}
		}
		
		public bool IsContainer {
			get {
				return type == Types.MixContainer || type == Types.RandomContainer || type == Types.SwitchContainer;
			}
		}
		
		public Types type;
		
		// Audio Source
		public string infoName;
		
		PureDataSetup setup;
		public PureDataSetup Setup {
			get {
				if (setup == null && !string.IsNullOrEmpty(infoName)) {
					GameObject gameObject = pureData.gameObject.FindChildRecursive(infoName);
					setup = gameObject == null ? null : gameObject.GetComponent<PureDataSetup>();
				}
				return setup;
			}
			set {
				setup = value;
				if (setup != null) {
					infoName = setup.name;
				}
			}
		}
		
		// Random Container
		[Min(0)] public float weight = 1;
		
		// Switch Container
		public PureDataSwitchSettings switchSettings;

		public PureDataOption[] options;
		
		public int id;
		public int parentId;
		public List<int> childrenIds = new List<int>();
		public PureData pureData;
		
		public PureDataSubContainer(PureDataContainer container, int parentId, PureDataSubContainer subContainer, PureData pureData) {
			this.Copy(subContainer, "id", "parentId", "childrenIds");
			
			this.name = container.Name;
			this.id = container.GetUniqueID();
			this.parentId = parentId;
			this.pureData = pureData;
			
			switchSettings = new PureDataSwitchSettings(pureData);
			
			if (parentId == 0) {
				container.childrenIds.Add(id);
			}
			else {
				container.GetSubContainerWithID(parentId).childrenIds.Add(id);
			}
		}
		
		public PureDataSubContainer(PureDataContainer container, int parentId, PureDataSetup setup, PureData pureData) {
			this.name = container.Name;
			this.id = container.GetUniqueID();
			this.parentId = parentId;
			this.Setup = setup;
			this.pureData = pureData;
			
			switchSettings = new PureDataSwitchSettings(pureData);
			
			if (parentId == 0) {
				container.childrenIds.Add(id);
			}
			else {
				container.GetSubContainerWithID(parentId).childrenIds.Add(id);
			}
		}
		
		public PureDataSubContainer(PureDataContainer container, int parentId, PureData pureData) {
			this.name = container.Name;
			this.id = container.GetUniqueID();
			this.parentId = parentId;
			this.pureData = pureData;
			this.switchSettings = new PureDataSwitchSettings(pureData);
			
			if (parentId == 0) {
				container.childrenIds.Add(id);
			}
			else {
				container.GetSubContainerWithID(parentId).childrenIds.Add(id);
			}
		}

		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			switchSettings.Initialize(pureData);
		}
		
		public void Remove(PureDataContainer container) {
			if (parentId == 0) {
				container.childrenIds.Remove(id);
			}
			else {
				PureDataSubContainer parent = container.GetSubContainerWithID(parentId);
				if (parent != null) {
					parent.childrenIds.Remove(id);
				}
			}
			
			if (container.subContainers.Contains(this)) {
				container.subContainers.Remove(this);
			}
			
			foreach (int childrenId in childrenIds.ToArray()) {
				container.GetSubContainerWithID(childrenId).Remove(container);
			}
		}

		public override string ToString() {
			return string.Format("{0}({1}, {2})", GetType().Name, Name, id);
		}
	}
}
