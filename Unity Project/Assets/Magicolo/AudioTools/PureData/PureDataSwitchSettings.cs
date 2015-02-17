using System.Reflection;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSwitchSettings {
		
		GameObject stateHolderObject;
		public GameObject StateHolderObject {
			get {
				if (stateHolderObjectId != 0 || !pureData.generalSettings.ApplicationPlaying) {
					stateHolderObject = pureData.references.GetObjectWithId<GameObject>(stateHolderObjectId);
					
					if (!pureData.generalSettings.ApplicationPlaying) {
						stateHolderObjectId = stateHolderObject == null ? 0 : stateHolderObjectId;
					}
				}
				return stateHolderObject;
			}
			set {
				if (stateHolderObject != value) {
					stateHolderObject = value;
					pureData.references.RemoveReference(stateHolderObjectId);
				
					if (stateHolderObject == null) {
						stateHolderObjectId = 0;
					}
					else {
						stateHolderObjectId = pureData.references.AddReference(stateHolderObject);
					}
				}
			}
		}
		public int stateHolderObjectId;
		
		Component stateHolderComponent;
		public Component StateHolderComponent {
			get {
				if (stateHolderComponentId != 0 || !pureData.generalSettings.ApplicationPlaying) {
					stateHolderComponent = pureData.references.GetObjectWithId<Component>(stateHolderComponentId);
					
					if (!pureData.generalSettings.ApplicationPlaying) {
						stateHolderComponentId = stateHolderComponent == null ? 0 : stateHolderComponentId;
					}
				}
				return stateHolderComponent;
			}
			set {
				if (stateHolderComponent != value) {
					stateHolderComponent = value;
					pureData.references.RemoveReference(stateHolderComponentId);
				
					if (stateHolderComponent == null) {
						stateHolderComponentId = 0;
					}
					else {
						stateHolderComponentId = pureData.references.AddReference(stateHolderComponent);
						StateHolderObject = stateHolderComponent.gameObject;
					}
				}
			}
		}
		public int stateHolderComponentId;
		
		public Object StateHolder { 
			get {
				if (stateHolderComponentId == 0) {
					return StateHolderObject;
				}
				
				return StateHolderComponent;
			}
			set {
				if (value == null) {
					StateHolderObject = null;
					StateHolderComponent = null;
				}
				else if (value as GameObject != null) {
					StateHolderObject = value as GameObject;
				}
				else if (value as Component != null) {
					StateHolderComponent = value as Component;
				}
			}
		}
		
		public string statePath;
		public string stateName;
		public int stateIndex;
		public PureData pureData;
		
		MemberInfo stateInfo;
		MemberInfo StateInfo {
			get {
				if (stateInfo == null) {
					stateInfo = StateHolder.GetMemberInfo(statePath);
				}
				return stateInfo;
			}
		}
		
		public PureDataSwitchSettings(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		public int GetCurrentStateIndex() {
			return StateInfo == null ? int.MinValue : (int)StateInfo.GetValue(StateHolder);
		}
	}
}