using System;
using System.Collections;
using System.Reflection;
using Magicolo.EditorTools;
using Magicolo.GeneralTools;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Magicolo.AudioTools {
	[CustomEditor(typeof(PureData))]
	public class PureDataEditor : CustomEditorBase {
		
		PureData pureData;
		PureDataGeneralSettings generalSettings;
		SerializedObject generalSettingsSerialized;
		
		#region Buses
		PureDataBusManager busManager;
		SerializedObject busManagerSerialized;
		SerializedProperty busesProperty;
		PureDataBus currentBus;
		SerializedProperty currentBusProperty;
		#endregion
		
		#region Spatializers
		PureDataSpatializerManager spatializerManager;
		SerializedObject spatializerManagerSerialized;
		SerializedProperty spatializersProperty;
		PureDataSpatializer currentSpatializer;
		SerializedProperty currentSpatializerProperty;
		#endregion
		
		#region Containers
		PureDataContainerManager containerManager;
		SerializedObject containerManagerSerialized;
		SerializedProperty containersProperty;
		PureDataContainer currentContainer;
		SerializedProperty currentContainerProperty;
		SerializedProperty subContainersProperty;
		PureDataSubContainer currentSubContainer;
		int currentSubContainerIndex;
		SerializedProperty currentSubContainerProperty;
		PureDataOption currentOption;
		SerializedProperty currentOptionProperty;
		#endregion
		
		#region Sequences
		PureDataSequenceManager sequenceManager;
		SerializedObject sequenceManagerSerialized;
		SerializedProperty sequencesProperty;
		PureDataSequence currentSequence;
		SerializedProperty currentSequenceProperty;
		SerializedProperty sequenceSpatializerProperty;
		SerializedProperty stepsProperty;
		SerializedProperty tracksProperty;
		PureDataSequenceTrack currentTrack;
		int currentTrackIndex;
		SerializedProperty currentTrackProperty;
		SerializedProperty trackStepsProperty;
		PureDataSequenceTrackStep currentTrackStep;
		Dictionary<PureDataSequence, SequenceSelectionData> sequenceSelections;
		SequenceSelectionData currentSequenceSelection;
		SerializedProperty sequencePatternsProperty;
		PureDataSequencePattern currentSequencePattern;
		SerializedProperty currentSequencePatternProperty;
		#endregion
		
		string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "w", "X", "Y", "Z" };
		
		public override void OnEnable() {
			base.OnEnable();
			
			pureData = (PureData)target;
			pureData.SetExecutionOrder(-15);
			pureData.hierarchyManager.FreezeTransforms();
		}
		
		public override void OnInspectorGUI() {
			pureData.InitializeSettings();
			pureData.hierarchyManager.FreezeTransforms();
			
			generalSettings = pureData.generalSettings;
			generalSettingsSerialized = new SerializedObject(generalSettings);
			busManager = pureData.busManager;
			busManagerSerialized = new SerializedObject(busManager);
			spatializerManager = pureData.spatializerManager;
			spatializerManagerSerialized = new SerializedObject(spatializerManager);
			containerManager = pureData.containerManager;
			containerManagerSerialized = new SerializedObject(containerManager);
			sequenceManager = pureData.sequenceManager;
			sequenceManagerSerialized = new SerializedObject(sequenceManager);
				
			Begin();
			
			ShowGeneralSettings();
			Separator();
			ShowBuses();
			ShowSpatializers();
			ShowContainers();
			ShowSequences();
			Separator();
			
			End();
			
			generalSettingsSerialized.ApplyModifiedProperties();
			busManagerSerialized.ApplyModifiedProperties();
			spatializerManagerSerialized.ApplyModifiedProperties();
			containerManagerSerialized.ApplyModifiedProperties();
			sequenceManagerSerialized.ApplyModifiedProperties();
		}

		void ShowGeneralSettings() {
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
			FolderPathButton(generalSettingsSerialized.FindProperty("patchesPath"), Application.streamingAssetsPath + "/", new GUIContent("Patches Path", "The path where the Pure Data patches are relative to Assets/StreamingAssets/."));
			EditorGUILayout.PropertyField(generalSettingsSerialized.FindProperty("maxVoices"), new GUIContent("Max Voices", "Sets the maximum simultaneous sources that can be played. The higher this value is, the longer it will take to initialize Pure Data."));
			
			EditorGUI.EndDisabledGroup();
			
			EditorGUILayout.PropertyField(generalSettingsSerialized.FindProperty("speedOfSound"), new GUIContent("Speed Of Sound", "Sets the speed of sound for doppler effects calculations."));
			EditorGUILayout.PropertyField(generalSettingsSerialized.FindProperty("masterVolume"), new GUIContent("Master Volume", "Controls the volume of all Pure Data sources."));
		}
		
		#region Buses
		void ShowBuses() {
			busesProperty = busManagerSerialized.FindProperty("buses");
			
			if (AddFoldOut(busesProperty, "Buses".ToGUIContent())) {
				busManager.buses[busManager.buses.Length - 1] = new PureDataBus(pureData);
				busManager.buses[busManager.buses.Length - 1].SetUniqueName("default", "", busManager.buses);
				busManager.UpdateMixer();
			}
			
			if (busesProperty.isExpanded) {
				if (busesProperty.arraySize > 0) {
					EditorGUILayout.HelpBox("Be sure to include exactly one [umixer~] object in your main Pure Data patch and to reload it each time you make modifications to the buses.", MessageType.Info);
				}
				
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < busesProperty.arraySize; i++) {
					currentBus = busManager.buses[i];
					currentBusProperty = busesProperty.GetArrayElementAtIndex(i);
					
					BeginBox();
					
					if (BusDeleteFoldout(i)) {
						busManager.UpdateMixer();
						break;
					}
					
					ShowBus();
					
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		bool BusDeleteFoldout(int index) {
			EditorGUILayout.BeginHorizontal();
			
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			
			Foldout(currentBusProperty, currentBus.Name.ToGUIContent(), style);
			if (!currentBusProperty.isExpanded) {
				EditorGUILayout.PropertyField(currentBusProperty.FindPropertyRelative("volume"), GUIContent.none);
			}
			bool pressed = DeleteButton(busesProperty, index);
			
			EditorGUILayout.EndHorizontal();
			
			if (!pressed) {
				Reorderable(busesProperty, index, true, OnBusReorder);
			}
			
			return pressed;
		}
		
		void ShowBus() {
			if (currentBusProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				EditorGUI.BeginChangeCheck();
				
				string busName = EditorGUILayout.TextField("Name", currentBus.Name);
				
				if (EditorGUI.EndChangeCheck()) {
					currentBus.SetUniqueName(busName, currentBus.Name, busManager.buses);
					busManager.UpdateMixer();
				}
				EditorGUI.EndDisabledGroup();
				
				EditorGUILayout.PropertyField(currentBusProperty.FindPropertyRelative("volume"));
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void OnBusReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			ReorderArray(arrayProperty, sourceIndex, targetIndex);
			busManager.UpdateMixer();
		}
		#endregion
		
		#region Spatializers
		void ShowSpatializers() {
			spatializersProperty = spatializerManagerSerialized.FindProperty("spatializers");
			
			if (AddFoldOut(spatializersProperty, "Spatializers".ToGUIContent())) {
				spatializerManager.spatializers[spatializerManager.spatializers.Length - 1] = new PureDataSpatializer(pureData);
				spatializerManager.spatializers[spatializerManager.spatializers.Length - 1].SetUniqueName("default", "", spatializerManager.spatializers);
			}
			
			if (spatializersProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < spatializersProperty.arraySize; i++) {
					currentSpatializer = spatializerManager.spatializers[i];
					currentSpatializerProperty = spatializersProperty.GetArrayElementAtIndex(i);
					
					BeginBox();
					
					GUIStyle style = new GUIStyle("foldout");
					style.fontStyle = FontStyle.Bold;
			
					if (DeleteFoldOut(spatializersProperty, i, currentSpatializer.Name.ToGUIContent(), style)) {
						break;
					}
					currentSpatializer.Showing = currentSpatializerProperty.isExpanded;
					
					ShowSpatializer();
					
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowSpatializer() {
			if (currentSpatializerProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				// Name
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				EditorGUI.BeginChangeCheck();
				
				string spatializerName = EditorGUILayout.TextField("Name", currentSpatializer.Name);
				
				if (EditorGUI.EndChangeCheck()) {
					currentSpatializer.SetUniqueName(spatializerName, currentSpatializer.Name, spatializerManager.spatializers);
				}
				EditorGUI.EndDisabledGroup();
				
				// Source
				EditorGUI.BeginChangeCheck();
				
				UnityEngine.Object source = EditorGUILayout.ObjectField("Source".ToGUIContent(), pureData.references.GetObjectWithId(currentSpatializer.SourceId), typeof(Transform), true);
				
				if (EditorGUI.EndChangeCheck()) {
					pureData.references.RemoveReference(currentSpatializer.SourceId);
					
					if (source == null) {
						currentSpatializer.SourceId = 0;
					}
					else {
						currentSpatializer.SourceId = pureData.references.AddReference(source);
					}
				}
				
				EditorGUILayout.PropertyField(currentSpatializerProperty.FindPropertyRelative("volumeRolloffMode"));
				EditorGUILayout.PropertyField(currentSpatializerProperty.FindPropertyRelative("minDistance"));
				EditorGUILayout.PropertyField(currentSpatializerProperty.FindPropertyRelative("maxDistance"));
				EditorGUILayout.PropertyField(currentSpatializerProperty.FindPropertyRelative("panLevel"));
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		#endregion
		
		#region Containers
		void ShowContainers() {
			containersProperty = containerManagerSerialized.FindProperty("containers");
		
			if (AddFoldOut(containersProperty, "Containers".ToGUIContent())) {
				containerManager.containers[containerManager.containers.Length - 1] = new PureDataContainer("", pureData);
				containerManager.containers[containerManager.containers.Length - 1].SetUniqueName("default", containerManager.containers);
			}
		
			if (containersProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				for (int i = 0; i < containerManager.containers.Length; i++) {
					currentContainer = containerManager.containers[i];
					currentContainerProperty = containersProperty.GetArrayElementAtIndex(i);
		
					BeginBox();
		
					if (DeleteFoldOut(containersProperty, i, currentContainer.Name.ToGUIContent(), GetContainerStyle(currentContainer.type.ConvertByName<PureDataSubContainer.Types>()))) {
						break;
					}
		
					ShowContainer();
		
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowContainer() {
			subContainersProperty = currentContainerProperty.FindPropertyRelative("subContainers");
		
			if (currentContainerProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				EditorGUI.BeginChangeCheck();
				string containerName = EditorGUILayout.TextField(currentContainer.Name);
				if (EditorGUI.EndChangeCheck()) {
					currentContainer.SetUniqueName(containerName, currentContainer.Name, "default", containerManager.containers);
				}
				EditorGUI.EndDisabledGroup();
				
				currentContainer.type = (PureDataContainer.Types)EditorGUILayout.EnumPopup(currentContainer.type);
		
				BeginBox();
		
				if (currentContainer.type == PureDataContainer.Types.SwitchContainer) {
					ShowSwitchContainerEnums(currentContainerProperty, currentContainer.switchSettings);
				}
		
				ShowSources();
		
				if (subContainersProperty.isExpanded) {
					EditorGUI.indentLevel += 1;
		
					ShowSubContainers(currentContainer.childrenIds);
		
					EditorGUI.indentLevel -= 1;
				}
		
				EndBox();
		
				Separator();
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowSources() {
			if (AddFoldOut<PureDataSetup>(subContainersProperty, currentContainer.childrenIds.Count, "Sources".ToGUIContent(), OnContainerSourceDropped)) {
				if (currentContainer.childrenIds.Count > 0) {
					currentContainer.subContainers[currentContainer.subContainers.Count - 1] = new PureDataSubContainer(currentContainer, 0, currentContainer.GetSubContainerWithID(currentContainer.childrenIds.Last()), pureData);
				}
				else {
					currentContainer.subContainers[currentContainer.subContainers.Count - 1] = new PureDataSubContainer(currentContainer, 0, pureData);
				}
				containerManagerSerialized.Update();
			}
		}
		
		void OnContainerSourceDropped(PureDataSetup droppedObject) {
			AddToArray(subContainersProperty);
			currentContainer.subContainers[currentContainer.subContainers.Count - 1] = new PureDataSubContainer(currentContainer, 0, droppedObject, pureData);
			containerManagerSerialized.Update();
		}
		
		void ShowSubContainers(List<int> ids) {
			for (int i = 0; i < ids.Count; i++) {
				currentSubContainer = currentContainer.GetSubContainerWithID(ids[i]);
				currentSubContainerIndex = currentContainer.subContainers.IndexOf(currentSubContainer);
				currentSubContainerProperty = subContainersProperty.GetArrayElementAtIndex(currentSubContainerIndex);
		
				if (DeleteFoldOut<PureDataSetup>(subContainersProperty, currentSubContainerIndex, currentSubContainer.Name.ToGUIContent(), GetContainerStyle(currentSubContainer.type), OnSubContainerDropped, OnSubContainerReorder)) {
					currentSubContainer.Remove(currentContainer);
					break;
				}
		
				ShowSubContainer();
			}
		}
		
		void OnSubContainerDropped(PureDataSetup droppedObject) {
			if (currentSubContainer.type == PureDataSubContainer.Types.AudioSource) {
				currentSubContainer.Setup = droppedObject;
				containerManagerSerialized.Update();
			}
			else {
				OnSubContainerSourceDropped(droppedObject);
			}
		}
		
		void OnSubContainerReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			PureDataSubContainer sourceSubContainer = currentContainer.subContainers[sourceIndex];
			PureDataSubContainer targetSubContainer = currentContainer.subContainers[targetIndex];
			List<int> sourceParentIds = sourceSubContainer.parentId == 0 ? currentContainer.childrenIds : currentContainer.GetSubContainerWithID(sourceSubContainer.parentId).childrenIds;
			List<int> targetParentIds = targetSubContainer.parentId == 0 ? currentContainer.childrenIds : currentContainer.GetSubContainerWithID(targetSubContainer.parentId).childrenIds;
		
			if (sourceParentIds == targetParentIds) {
				int sourceSubContainerIndex = sourceParentIds.IndexOf(sourceSubContainer.id);
				int targetSubContainerIndex = targetParentIds.IndexOf(targetSubContainer.id);
				sourceParentIds.Move(sourceSubContainerIndex, targetSubContainerIndex);
			}
			else {
				int targetSubContainerIndex = targetParentIds.IndexOf(targetSubContainer.id);
				targetParentIds.Insert(targetSubContainerIndex, sourceSubContainer.id);
				sourceParentIds.Remove(sourceSubContainer.id);
				sourceSubContainer.parentId = targetSubContainer.parentId;
			}
			containerManagerSerialized.Update();
		}
		
		void ShowSubContainer() {
			AdjustContainerName();
		
			if (currentSubContainerProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				ShowGeneralContainerSettings();
		
				if (currentSubContainer.IsSource) {
					switch (currentSubContainer.type) {
						case PureDataSubContainer.Types.AudioSource:
							ShowAudioSource();
							break;
					}
				}
				else {
					BeginBox();
		
					switch (currentSubContainer.type) {
						case PureDataSubContainer.Types.MixContainer:
							ShowMixContainer();
							break;
						case PureDataSubContainer.Types.RandomContainer:
							ShowRandomContainer();
							break;
						case PureDataSubContainer.Types.SwitchContainer:
							ShowSwitchContainer();
							break;
					}
		
					EndBox();
		
					Separator();
				}
		
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowAudioSource() {
			currentSubContainer.Setup = EditorGUILayout.ObjectField("Source".ToGUIContent(), currentSubContainer.Setup, typeof(PureDataSetup), true) as PureDataSetup;
			ContextMenu(new []{ "Clear".ToGUIContent() }, new GenericMenu.MenuFunction2[]{ OnAudioSourceCleared }, new object[]{ currentSubContainer });
		
			if (currentSubContainer.Setup != null && currentSubContainer.Setup.Info != null) {
				ShowOptions();
			}
		}
		
		void OnAudioSourceCleared(object data) {
			PureDataSubContainer subContainer = data as PureDataSubContainer;
			subContainer.Setup = null;
			subContainer.infoName = "";
			containerManagerSerialized.Update();
		}
		
		void ShowMixContainer() {
			ShowSubSourcesAddFoldout();
		
			if (currentSubContainer.Showing && currentContainer.childrenIds.Count > 0) {
				EditorGUI.indentLevel += 1;
		
				ShowSubContainers(currentSubContainer.childrenIds);
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowRandomContainer() {
			ShowSubSourcesAddFoldout();
		
			if (currentSubContainer.Showing && currentContainer.childrenIds.Count > 0) {
				EditorGUI.indentLevel += 1;
		
				ShowSubContainers(currentSubContainer.childrenIds);
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowSwitchContainer() {
			ShowSwitchContainerEnums(currentSubContainerProperty, currentSubContainer.switchSettings);
		
			ShowSubSourcesAddFoldout();
		
			if (currentSubContainer.Showing && currentContainer.childrenIds.Count > 0) {
				EditorGUI.indentLevel += 1;
		
				ShowSubContainers(currentSubContainer.childrenIds);
		
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowSwitchContainerEnums(SerializedProperty property, PureDataSwitchSettings switchSettings) {
			Rect rect = EditorGUILayout.BeginHorizontal();
			
			// State holder
			EditorGUI.BeginChangeCheck();
			
			switchSettings.StateHolder = EditorGUILayout.ObjectField("State Holder".ToGUIContent(), switchSettings.StateHolderObject, typeof(UnityEngine.Object), true, GUILayout.MaxWidth(switchSettings.StateHolderObject == null ? Screen.width : Screen.width / 1.6F));
			
			// Component list
			if (switchSettings.StateHolderObject != null) {
				List<string> componentNames = new List<string>{ "GameObject" };
				Component[] components = switchSettings.StateHolderObject.GetComponents<Component>();
				
				foreach (Component component in components) {
					componentNames.Add(component.GetType().Name);
				}
		
				float width = Mathf.Min(92 + EditorGUI.indentLevel * 16, EditorGUIUtility.currentViewWidth / 5 + EditorGUI.indentLevel * 16);
				int index = EditorGUI.Popup(new Rect(rect.x + rect.width - width, rect.y, width, rect.height), Array.IndexOf(components, switchSettings.StateHolderComponent) + 1, componentNames.ToArray());
				
				switchSettings.StateHolderComponent = index > 0 ? components[index - 1] : null;
			}
			
			EditorGUILayout.EndHorizontal();
		
			// State field
			if (switchSettings.StateHolder != null) {
				string[] enumNames = switchSettings.StateHolder.GetFieldsPropertiesNames(ObjectExtensions.AllPublicFlags, typeof(Enum));
		
				if (enumNames.Length > 0) {
					int index = Mathf.Max(Array.IndexOf(enumNames, switchSettings.statePath), 0);
					index = EditorGUILayout.Popup("State Field", index, enumNames);
					switchSettings.statePath = enumNames[Mathf.Clamp(index, 0, Mathf.Max(enumNames.Length - 1, 0))];
				}
				else {
					EditorGUILayout.Popup("State Field", 0, new string[0]);
				}
			}
			
			if (EditorGUI.EndChangeCheck()) {
				property.serializedObject.Update();
			}
		}
		
		void OnSubContainerSourceDropped(PureDataSetup droppedObject) {
			currentContainer.subContainers.Add(new PureDataSubContainer(currentContainer, currentSubContainer.id, droppedObject, pureData));
			containerManagerSerialized.Update();
		}
		
		void OnSubContainerSourceAdded(SerializedProperty arrayProperty) {
			if (currentSubContainer.childrenIds.Count > 0) {
				currentContainer.subContainers.Add(new PureDataSubContainer(currentContainer, currentSubContainer.id, currentContainer.GetSubContainerWithID(currentSubContainer.childrenIds.Last()), pureData));
			}
			else {
				currentContainer.subContainers.Add(new PureDataSubContainer(currentContainer, currentSubContainer.id, pureData));
			}
			containerManagerSerialized.Update();
		}
		
		void ShowGeneralContainerSettings() {
			currentSubContainer.type = (PureDataSubContainer.Types)EditorGUILayout.EnumPopup(currentSubContainer.type);
		
			if (GetParentContainerType(currentSubContainer, currentContainer) == PureDataSubContainer.Types.RandomContainer) {
				EditorGUILayout.PropertyField(currentSubContainerProperty.FindPropertyRelative("weight"));
			}
			else if (GetParentContainerType(currentSubContainer, currentContainer) == PureDataSubContainer.Types.SwitchContainer) {
				PureDataSwitchSettings parentSwitchSettings = currentSubContainer.parentId == 0 ? currentContainer.switchSettings : currentContainer.GetSubContainerWithID(currentSubContainer.parentId).switchSettings;
				PureDataSwitchSettings switchSettings = currentSubContainer.switchSettings;
		
				if (parentSwitchSettings.StateHolder != null && !string.IsNullOrEmpty(parentSwitchSettings.statePath)) {
					FieldInfo enumField = parentSwitchSettings.StateHolder.GetType().GetField(parentSwitchSettings.statePath, ObjectExtensions.AllPublicFlags);
					PropertyInfo enumProperty = parentSwitchSettings.StateHolder.GetType().GetProperty(parentSwitchSettings.statePath, ObjectExtensions.AllPublicFlags);
					Type enumType = enumField == null ? enumProperty == null ? null : enumProperty.PropertyType : enumField.FieldType;
		
					if (enumType != null) {
						string[] enumNames = Enum.GetNames(enumType);
						Enum defaultState = (Enum)Enum.Parse(enumType, enumNames[0]);
						Enum selectedState = Enum.GetNames(enumType).Contains(switchSettings.stateName) ? (Enum)Enum.Parse(enumType, switchSettings.stateName) : null;
						Enum selectedEnum = selectedState == null ? EditorGUILayout.EnumPopup("State", defaultState) : EditorGUILayout.EnumPopup("State", selectedState);
						
						switchSettings.stateName = selectedEnum.ToString();
						switchSettings.stateIndex = selectedEnum.GetHashCode();
						
						return;
					}
				}
		
				EditorGUILayout.Popup("State", 0, new string[0]);
			}
		}
		
		void ShowOptions() {
			SerializedProperty optionsProperty = currentSubContainerProperty.FindPropertyRelative("options");
		
			if (AddFoldOut(optionsProperty, "Options".ToGUIContent())) {
				currentSubContainer.options[currentSubContainer.options.Length - 1] = PureDataOption.Volume(0);
				containerManagerSerialized.Update();
			}
		
			if (currentSubContainer.options != null && optionsProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				for (int i = 0; i < currentSubContainer.options.Length; i++) {
					currentOption = currentSubContainer.options[i];
					currentOptionProperty = optionsProperty.GetArrayElementAtIndex(i);
		
					BeginBox();
					
					GUIStyle style = new GUIStyle("foldout");
					style.fontStyle = FontStyle.Bold;
					if (DeleteFoldOut(optionsProperty, i, string.Format("{0} | {1}", currentOption.type, currentOption.GetValueDisplayName()).ToGUIContent(), style)) {
						break;
					}
					
					ShowOption();
					EndBox();
				}
		
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowOption() {
			if (currentOptionProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
		
				
				bool resetValue = ShowOptionTypes();
				
		
				switch (currentOption.type) {
					case PureDataOption.OptionTypes.Volume:
						currentOption.value.type = PureDataOptionValue.ValueTypes.FloatArray;
						float[] volumeData = currentOption.GetValue<float[]>();
						volumeData[0] = Mathf.Max(EditorGUILayout.FloatField("Value".ToGUIContent(), volumeData[0]), 0);
						volumeData[1] = Mathf.Max(EditorGUILayout.FloatField("Time".ToGUIContent(), volumeData[1]), 0);
						currentOption.SetValue(volumeData);
						currentOption.SetDefaultValue(new []{ 1F, 0F });
						break;
					case PureDataOption.OptionTypes.Pitch:
						currentOption.value.type = PureDataOptionValue.ValueTypes.FloatArray;
						float[] pitchData = currentOption.GetValue<float[]>();
						pitchData[0] = EditorGUILayout.FloatField("Value".ToGUIContent(), pitchData[0]);
						pitchData[1] = Mathf.Max(EditorGUILayout.FloatField("Time".ToGUIContent(), pitchData[1]), 0);
						currentOption.SetValue(pitchData);
						currentOption.SetDefaultValue(new []{ 1F, 0F });
						break;
					case PureDataOption.OptionTypes.RandomVolume:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(EditorGUILayout.Slider("Value".ToGUIContent(), currentOption.GetValue<float>(), 0, 1));
						currentOption.SetDefaultValue(0F);
						break;
					case PureDataOption.OptionTypes.RandomPitch:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(EditorGUILayout.Slider("Value".ToGUIContent(), currentOption.GetValue<float>(), 0, 1));
						currentOption.SetDefaultValue(0F);
						break;
					case PureDataOption.OptionTypes.FadeIn:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(Mathf.Max(EditorGUILayout.FloatField("Value".ToGUIContent(), currentOption.GetValue<float>()), 0));
						currentOption.SetDefaultValue(0F);
						break;
					case PureDataOption.OptionTypes.FadeOut:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(Mathf.Max(EditorGUILayout.FloatField("Value".ToGUIContent(), currentOption.GetValue<float>()), 0));
						currentOption.SetDefaultValue(0.1F);
						break;
					case PureDataOption.OptionTypes.Loop:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Bool;
						currentOption.SetValue(EditorGUILayout.Toggle("Value".ToGUIContent(), currentOption.GetValue<bool>()));
						currentOption.SetDefaultValue(false);
						break;
					case PureDataOption.OptionTypes.Clip:
						currentOption.value.type = PureDataOptionValue.ValueTypes.String;
						ShowClipOption();
						currentOption.SetDefaultValue("");
						break;
					case PureDataOption.OptionTypes.Output:
						currentOption.value.type = PureDataOptionValue.ValueTypes.String;
						ShowOutputOption();
						currentOption.SetDefaultValue("Master");
						break;
					case PureDataOption.OptionTypes.PlayRange:
						currentOption.value.type = PureDataOptionValue.ValueTypes.FloatArray;
						float[] playRangeData = currentOption.GetValue<float[]>();
						EditorGUILayout.MinMaxSlider("Value".ToGUIContent(), ref playRangeData[0], ref playRangeData[1], 0, 1);
						playRangeData[0] = float.IsNaN(playRangeData[0]) ? 0 : Mathf.Clamp(playRangeData[0], 0, playRangeData[1]);
						playRangeData[1] = float.IsNaN(playRangeData[1]) ? 1 : Mathf.Clamp(playRangeData[1], playRangeData[0], 1);
						currentOption.SetValue(playRangeData);
						currentOption.SetDefaultValue(new []{ 0F, 1F });
						break;
					case PureDataOption.OptionTypes.Time:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(EditorGUILayout.Slider("Value".ToGUIContent(), currentOption.GetValue<float>(), 0, 1));
						currentOption.SetDefaultValue(0F);
						break;
					case PureDataOption.OptionTypes.DopplerLevel:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(EditorGUILayout.Slider("Value".ToGUIContent(), currentOption.GetValue<float>(), 0, 10));
						currentOption.SetDefaultValue(1F);
						break;
					case PureDataOption.OptionTypes.VolumeRolloffMode:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue((float)(PureDataVolumeRolloffModes)EditorGUILayout.EnumPopup("Value".ToGUIContent(), (PureDataVolumeRolloffModes)currentOption.GetValue<float>()));
						currentOption.SetDefaultValue(0F);
						break;
					case PureDataOption.OptionTypes.MinDistance:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(Mathf.Max(EditorGUILayout.FloatField("Value".ToGUIContent(), currentOption.GetValue<float>()), 0));
						currentOption.SetDefaultValue(5F);
						break;
					case PureDataOption.OptionTypes.MaxDistance:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(Mathf.Max(EditorGUILayout.FloatField("Value".ToGUIContent(), currentOption.GetValue<float>()), 0));
						currentOption.SetDefaultValue(500F);
						break;
					case PureDataOption.OptionTypes.PanLevel:
						currentOption.value.type = PureDataOptionValue.ValueTypes.Float;
						currentOption.SetValue(EditorGUILayout.Slider("Value".ToGUIContent(), currentOption.GetValue<float>(), 0, 1));
						currentOption.SetDefaultValue(0.75F);
						break;
					case PureDataOption.OptionTypes.StepTempo:
						break;
					case PureDataOption.OptionTypes.StepBeats:
						break;
					case PureDataOption.OptionTypes.StepPattern:
						break;
				}
				
				if (resetValue) {
					currentOption.ResetValue();
					currentOptionProperty.serializedObject.Update();
				}
				
				if (currentOption.IsDelayable) {
					currentOption.delay = Mathf.Max(EditorGUILayout.FloatField("Delay".ToGUIContent(), currentOption.delay), 0);
				}
				
				EditorGUI.indentLevel -= 1;
			}
		}

		bool ShowOptionTypes() {
			bool changed = false;
			List<string> types = new List<string>(Enum.GetNames(typeof(PureDataOption.OptionTypes)));
			types.Remove(PureDataOption.OptionTypes.StepBeats.ToString());
			types.Remove(PureDataOption.OptionTypes.StepTempo.ToString());
			types.Remove(PureDataOption.OptionTypes.StepPattern.ToString());
			types.Remove(PureDataOption.OptionTypes.TrackSendType.ToString());
			types.Remove(PureDataOption.OptionTypes.TrackPattern.ToString());
			
			EditorGUI.BeginChangeCheck();
			
			currentOption.type = (PureDataOption.OptionTypes)EditorGUILayout.Popup("Type", (int)currentOption.type, types.ToArray());
			
			if (EditorGUI.EndChangeCheck()) {
				currentOptionProperty.serializedObject.Update();
				changed = true;
			}
			
			return changed;	
		}
		
		void ShowOutputOption() {
			List<string> options = new List<string>();
			options.Add("Master");
			options.AddRange(pureData.busManager.buses.GetNames());
			currentOption.SetValue(Popup(currentOption.GetValue<string>(), options.ToArray(), "Output".ToGUIContent()));
		}

		void ShowClipOption() {
			List<string> options = new List<string>();
			foreach (PureDataClip clip in pureData.clipManager.GetAllClips()) {
				options.Add(clip.Name);
			}
			currentOption.SetValue(Popup(currentOption.GetValue<string>(), options.ToArray(), "Clip".ToGUIContent()));
		}
		
		void ShowSubSourcesAddFoldout() {
			AddFoldOut<PureDataSetup>(subContainersProperty, currentSubContainer, currentSubContainer.childrenIds.Count, "Sources".ToGUIContent(), OnSubContainerSourceDropped, OnSubContainerSourceAdded);
		}
		
		void AdjustContainerName() {
			switch (currentSubContainer.type) {
				case PureDataSubContainer.Types.AudioSource:
					if (currentSubContainer.Setup == null) {
						AdjustName("Audio Source: null", currentSubContainer, currentContainer);
					}
					else {
						AdjustName("Audio Source: " + currentSubContainer.Setup.name, currentSubContainer, currentContainer);
					}
					break;
				case PureDataSubContainer.Types.MixContainer:
					AdjustName("Mix Container", currentSubContainer, currentContainer);
					break;
				case PureDataSubContainer.Types.RandomContainer:
					AdjustName("Random Container", currentSubContainer, currentContainer);
					break;
				case PureDataSubContainer.Types.SwitchContainer:
					AdjustName("Switch Container", currentSubContainer, currentContainer);
					break;
			}
		}
		
		void AdjustName(string prefix, PureDataSubContainer subContainer, PureDataContainer container) {
			subContainer.Name = prefix;
		
			if (subContainer.IsContainer) {
				subContainer.Name += string.Format(" ({0})", subContainer.childrenIds.Count);
			}
		
			if (GetParentContainerType(subContainer, container) == PureDataSubContainer.Types.RandomContainer) {
				subContainer.Name += " | Weight: " + subContainer.weight;
			}
			else if (GetParentContainerType(subContainer, container) == PureDataSubContainer.Types.SwitchContainer) {
				subContainer.Name += " | State: " + subContainer.switchSettings.stateName;
			}
		}
		
		GUIStyle GetContainerStyle(PureDataSubContainer.Types type) {
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			Color textColor = style.normal.textColor;
		
			switch (type) {
				case PureDataSubContainer.Types.AudioSource:
					textColor = new Color(1, 0.5F, 0.3F, 10);
					break;
				case PureDataSubContainer.Types.MixContainer:
					textColor = new Color(0, 1, 1, 10);
					break;
				case PureDataSubContainer.Types.RandomContainer:
					textColor = new Color(1, 1, 0, 10);
					break;
				case PureDataSubContainer.Types.SwitchContainer:
					textColor = new Color(0.5F, 1, 0.3F, 10);
					break;
			}
		
			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;
		
			return style;
		}
		
		PureDataSubContainer.Types GetParentContainerType(PureDataSubContainer subContainer, PureDataContainer container) {
			PureDataSubContainer.Types type = PureDataSubContainer.Types.AudioSource;
		
			if (subContainer.parentId != 0) {
				type = container.GetSubContainerWithID(subContainer.parentId).type;
			}
			else {
				type = container.type.ConvertByName<PureDataSubContainer.Types>();
			}
		
			return type;
		}
		#endregion
	
		#region Sequences
		void ShowSequences() {
			sequencesProperty = sequenceManagerSerialized.FindProperty("sequences");
			sequenceSelections = sequenceSelections ?? new Dictionary<PureDataSequence, SequenceSelectionData>();
			
			if (AddFoldOut(sequencesProperty, "Sequences".ToGUIContent())) {
				sequenceManager.sequences[sequenceManager.sequences.Length - 1] = new PureDataSequence(pureData);
				sequenceManager.sequences[sequenceManager.sequences.Length - 1].SetUniqueName("default", sequenceManager.sequences);
				sequenceManager.UpdateSequenceContainer();
				sequenceManagerSerialized.Update();
			}
			
			if (sequencesProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < sequencesProperty.arraySize; i++) {
					currentSequence = sequenceManager.sequences[i];
					currentSequenceProperty = sequencesProperty.GetArrayElementAtIndex(i);
					stepsProperty = currentSequenceProperty.FindPropertyRelative("steps");
					tracksProperty = currentSequenceProperty.FindPropertyRelative("tracks");
					sequenceSpatializerProperty = currentSequenceProperty.FindPropertyRelative("spatializer");
			
					if (!sequenceSelections.ContainsKey(currentSequence)) {
						sequenceSelections[currentSequence] = new SequenceSelectionData();
					}

					currentSequenceSelection = sequenceSelections[currentSequence];
					currentSequenceSelection.Initialize(currentSequence, currentSequenceProperty);
					
					BeginBox();
		
					if (DeleteFoldOut(sequencesProperty, i, currentSequence.Name.ToGUIContent(), GetSequenceStyle(), OnSequenceReorder)) {
						sequenceManager.UpdateSequenceContainer();
						break;
					}
		
					ShowSequence();
					
					EndBox();
				}
				
				Separator();
				
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void OnSequenceReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			ReorderArray(arrayProperty, sourceIndex, targetIndex);
			sequenceManager.UpdateSequenceContainer();
		}
		
		void ShowSequence() {
			if (currentSequenceProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				EditorGUI.BeginChangeCheck();
				
				// Name
				string sequenceName = EditorGUILayout.TextField(currentSequence.Name);
				if (EditorGUI.EndChangeCheck()) {
					currentSequence.SetUniqueName(sequenceName, currentSequence.Name, "default", sequenceManager.sequences);
					currentSequenceProperty.serializedObject.Update();
					sequenceManager.UpdateSequenceContainer();
				}
				
				EditorGUI.EndDisabledGroup();
				
				ShowSequenceOutput();
				EditorGUILayout.PropertyField(currentSequenceProperty.FindPropertyRelative("loop"));
				EditorGUILayout.PropertyField(currentSequenceProperty.FindPropertyRelative("volume"));
				EditorGUILayout.PropertyField(currentSequenceProperty.FindPropertyRelative("sleepTime"));
				
				ShowSequenceSpatialSettings();
				
				ShowTracks();
				
				Separator();
				
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowSequenceOutput() {
			List<string> options = new List<string>();
			options.Add("Master");
			options.AddRange(pureData.busManager.buses.GetNames());
			
			Popup(currentSequenceProperty.FindPropertyRelative("output"), options.ToArray(), "Output".ToGUIContent());
		}
		
		void ShowSequenceSpatialSettings() {
			GUILayout.Space(8);
			
			BeginBox();
			
			EditorGUILayout.LabelField("3D Sound Settings", new GUIStyle("boldLabel"));
			
			EditorGUI.indentLevel += 1;
			
			EditorGUILayout.PropertyField(sequenceSpatializerProperty.FindPropertyRelative("volumeRolloffMode"));
			EditorGUILayout.PropertyField(sequenceSpatializerProperty.FindPropertyRelative("minDistance"));
			EditorGUILayout.PropertyField(sequenceSpatializerProperty.FindPropertyRelative("maxDistance"));
			EditorGUILayout.PropertyField(sequenceSpatializerProperty.FindPropertyRelative("panLevel"));
			
			Separator();
			EditorGUI.indentLevel -= 1;
			
			EndBox();
		}
		
		void ShowTracks() {
			Separator();
			
			GUILayout.Space(10);
			
			
			Rect rect = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(" ");
			
			// Add Tracks Button
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
			GUIStyle style = new GUIStyle("PreButton");
			style.clipping = TextClipping.Overflow;
			style.contentOffset = new Vector2(-2, -1);
			style.fontSize = 10;
			rect = EditorGUI.IndentedRect(rect);
			rect.width = 86;
			rect.height = 16;
			if (GUI.Button(new Rect(rect.x + 1, rect.y - 1, rect.width - 1, rect.height), "Add Track", style) || tracksProperty.arraySize == 0) {
				AddTrack();
			}
			
			EditorGUI.EndDisabledGroup();
			
			// Step Buttons
			GUI.Box(new Rect(rect.x + 84, rect.y - 1, stepsProperty.arraySize * 18, 18), "", new GUIStyle("ColorPickerBox"));
			rect.x += rect.width;
			rect.width = 16;
			for (int i = 0; i < stepsProperty.arraySize; i++) {
				GUI.Box(new Rect(rect.x - 1, rect.y, 18, tracksProperty.arraySize * 18 + 23), "", new GUIStyle("ColorPickerBox"));
				
				if (GUI.Button(rect, "", GetSequenceStepStyle(i))) {
					currentSequenceSelection.SetStep(i);
				}
				
				// Reorder Handles
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				
				Rect handleRect = new Rect(rect.x + 11, rect.y - 11, -7, 8);
				Reorderable(stepsProperty, i, true, new Rect(handleRect.x - 12, handleRect.y - 4, Mathf.Abs(handleRect.width) + 10, handleRect.height + 6), OnStepReorder);
				GUI.Label(handleRect, "", new GUIStyle("ColorPickerVertThumb"));
				
				EditorGUI.EndDisabledGroup();
				
				rect.x += rect.width + 2;
			}
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.LabelField(GUIContent.none, new GUIStyle("RL DragHandle"), GUILayout.Height(4), GUILayout.Width(86 + 18 * stepsProperty.arraySize + 29));
			
			for (int i = 0; i < tracksProperty.arraySize; i++) {
				currentTrack = currentSequence.tracks[i];
				currentTrackIndex = i;
				currentTrackProperty = tracksProperty.GetArrayElementAtIndex(i);
				
				ShowTrack();
			}
			
			// Add Steps Button
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
			style = new GUIStyle("buttonRight");
			style.fontStyle = FontStyle.Bold;
			style.clipping = TextClipping.Overflow;
			style.contentOffset = new Vector2(-1, 0);
			style.fontSize = 19;
			if (GUI.Button(new Rect(rect.x, rect.y - 1, 16, tracksProperty.arraySize * 18 + 25), "+", style) || stepsProperty.arraySize == 0) {
				AddStep();
			}
			
			EditorGUI.EndDisabledGroup();
			
			ShowSelectedTrackDetails();
			ShowSelectedStepDetails();
		}
		
		void OnStepReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			ReorderArray(arrayProperty, sourceIndex, targetIndex);
			
			for (int i = 0; i < tracksProperty.arraySize; i++) {
				ReorderArray(tracksProperty.GetArrayElementAtIndex(i).FindPropertyRelative("steps"), sourceIndex, targetIndex);
			}
			
			currentSequenceProperty.serializedObject.Update();
			currentSequenceSelection.SetStep(targetIndex);
		}
		
		void AddStep() {
			AddToArray(stepsProperty);
			currentSequence.steps[currentSequence.steps.Length - 1] = new PureDataSequenceStep();
			stepsProperty.serializedObject.Update();
			UpdateTrackStepsArray();
			currentSequenceSelection.SetStep(currentSequence.steps.Length - 1);
		}
		
		void AddTrack() {
			AddToArray(tracksProperty);
			currentSequence.tracks[currentSequence.tracks.Length - 1] = new PureDataSequenceTrack(pureData);
			tracksProperty.serializedObject.Update();
			UpdateTrackStepsArray();
			currentSequenceSelection.SetTrack(currentSequence.tracks.Length - 1);
		}
		
		void DeleteStep() {
			DeleteTrackStepFromTracks(currentSequenceSelection.stepIndex);
					
			if (stepsProperty.arraySize == 0) {
				AddStep();
				currentSequenceSelection.Initialize(currentSequence, currentSequenceProperty);
				
				foreach (PureDataSequenceTrack track in currentSequence.tracks) {
					track.RemovePatternFromSteps(0);
				}
				
				currentSequenceProperty.serializedObject.Update();
			}
		}
		
		void DeleteTrack() {
			if (tracksProperty.arraySize == 0) {
				AddTrack();
				currentSequenceSelection.Initialize(currentSequence, currentSequenceProperty);
			}
		}
		
		void ShowTrack() {
			trackStepsProperty = currentTrackProperty.FindPropertyRelative("steps");
			
			
			// Track
			Rect rect = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(" ");
			EditorGUILayout.EndHorizontal();
			
			EditorGUI.indentLevel += 1;
			rect = EditorGUI.IndentedRect(rect);
			EditorGUI.indentLevel -= 1;
			
			// Reorder Handles
			EditorGUI.BeginDisabledGroup(Application.isPlaying);
			
			Rect handleRect = new Rect(rect.x - 12, rect.y + 10, 12, -3);
			Reorderable(tracksProperty, currentTrackIndex, true, new Rect(handleRect.x - 3, handleRect.y - 11, handleRect.width + 3, Mathf.Abs(handleRect.height) + 15), OnTrackReorder);
			GUI.Label(handleRect, "", new GUIStyle("ColorPickerHorizThumb"));
			
			EditorGUI.EndDisabledGroup();
			
			// Track Name
			rect.width = 72;
			rect.height = 16;
			if (GUI.Button(rect, currentTrack.Name, GetTrackStyle())) {
				currentSequenceSelection.SetTrack(currentTrackIndex);
			}
			
			
			// Grid Lines
			GUI.Box(new Rect(rect.x + 72, rect.y - 1, stepsProperty.arraySize * 18 - 1, 18), "", new GUIStyle("ColorPickerBox"));
			GUI.Box(new Rect(rect.x, rect.y - 1, 72, 18), "", new GUIStyle(currentTrack == currentSequenceSelection.Track ? "TL SelectionButton PreDropGlow" : "ColorPickerBox"));
			
			// Track Steps
			rect.x += rect.width;
			rect.width = 16;
			for (int i = 0; i < trackStepsProperty.arraySize; i++) {
				currentTrackStep = currentTrack.steps[i];
				
				if (GUI.Button(rect, GetSequencePatternName(currentTrackStep.patternIndex), GetTrackStepStyle(i))) {
					currentSequenceSelection.SetStep(i);
					currentSequenceSelection.SetTrack(currentTrackIndex);
				}
				
				rect.x += rect.width + 2;
			}
		}
		
		void OnTrackReorder(SerializedProperty arrayProperty, int sourceIndex, int targetIndex) {
			ReorderArray(arrayProperty, sourceIndex, targetIndex);
			
			currentSequenceProperty.serializedObject.Update();
			currentSequenceSelection.SetTrack(targetIndex);
		}
		
		void ShowSelectedStepDetails() {
			if (!currentSequenceSelection.StepIsNull) {
				Separator();
				
				BeginBox();
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.LabelField("Step Details | " + currentSequenceSelection.stepIndex, new GUIStyle("boldLabel"));
				if (DeleteButton(stepsProperty, currentSequenceSelection.stepIndex)) {
					DeleteStep();
					return;
				}
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUI.indentLevel += 1;
					
				EditorGUILayout.PropertyField(currentSequenceSelection.StepProperty.FindPropertyRelative("tempo"));
				EditorGUILayout.PropertyField(currentSequenceSelection.StepProperty.FindPropertyRelative("beats"));
				
				// Selected Pattern
				if (!currentSequenceSelection.TrackIsNull) {
					string[] options = alphabet.Slice(0, currentSequenceSelection.Track.patterns.Length);
					Array.Resize(ref options, options.Length + 1);
					options.Move(options.Length - 1, 0);
					options[0] = " ";
						
					PureDataSequenceTrackStep step = currentSequenceSelection.Track.steps[currentSequenceSelection.stepIndex];
					step.patternIndex = EditorGUILayout.Popup("Pattern", step.patternIndex + 1, options) - 1;
				}
					
				Separator();
				EditorGUI.indentLevel -= 1;
				EndBox();
			}
			else {
				EditorGUILayout.HelpBox("Select a step.", MessageType.Info);
			}
			
		}

		void ShowSelectedTrackDetails() {
			if (!currentSequenceSelection.TrackIsNull) {
				Separator();
				
				BeginBox();
				EditorGUILayout.BeginHorizontal();
				
				EditorGUILayout.LabelField("Track Details | " + currentSequenceSelection.Track.Name, new GUIStyle("boldLabel"));
				if (DeleteButton(tracksProperty, currentSequenceSelection.trackIndex)) {
					DeleteTrack();
					return;
				}
				
				EditorGUILayout.EndHorizontal();
				
				EditorGUI.indentLevel += 1;
				
				// Instrument Patch
				EditorGUI.BeginDisabledGroup(Application.isPlaying);
				EditorGUI.BeginChangeCheck();
				
				FilePathButton(currentSequenceSelection.TrackProperty.FindPropertyRelative("instrumentPatchPath"), Application.streamingAssetsPath + "/" + generalSettings.patchesPath + "/", "pd", "Instrument".ToGUIContent());
			
				if (EditorGUI.EndChangeCheck()) {
					currentSequenceSelection.TrackProperty.serializedObject.ApplyModifiedProperties();
					sequenceManager.UpdateSequenceContainer();
				}
				EditorGUI.EndDisabledGroup();
				
				if (string.IsNullOrEmpty(currentSequenceSelection.Track.instrumentPatchPath)) {
					EditorGUILayout.HelpBox("Select a valid instrument patch. To be considered valid, an instrument patch must contain at least two [inlet] and two [outlet~].", MessageType.Info);
				}
				else {
					ShowSequencePatterns();
				}
					
				Separator();
				EditorGUI.indentLevel -= 1;
				EndBox();
			}
			else {
				EditorGUILayout.HelpBox("Select a track.", MessageType.Info);
			}
			
		}
		
		void ShowSequencePatterns() {
			sequencePatternsProperty = currentSequenceSelection.TrackProperty.FindPropertyRelative("patterns");
			
			BeginBox();
			
			if (AddFoldOut(sequencePatternsProperty)) {
				currentSequenceSelection.Track.patterns[currentSequenceSelection.Track.patterns.Length - 1] = new PureDataSequencePattern();
				sequencePatternsProperty.serializedObject.Update();
			}
			
			if (sequencePatternsProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < sequencePatternsProperty.arraySize; i++) {
					currentSequencePattern = currentSequenceSelection.Track.patterns[i];
					currentSequencePatternProperty = sequencePatternsProperty.GetArrayElementAtIndex(i);
					
					GUIStyle style = new GUIStyle("foldout");
					style.fontStyle = FontStyle.Bold;
					if (DeleteFoldOut(sequencePatternsProperty, i, string.Format("{0} | {1} | {2} : {3}", GetSequencePatternName(i), currentSequencePattern.sendType, currentSequencePattern.sendSize, currentSequencePattern.subdivision).ToGUIContent(), style)) {
						currentSequenceSelection.Track.RemovePatternFromSteps(i);
						currentSequenceSelection.TrackProperty.serializedObject.Update();
						break;
					}
					
					ShowSequencePattern();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
			
			EndBox();
		}

		void ShowSequencePattern() {
			if (currentSequencePatternProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				EditorGUI.BeginChangeCheck();
				
				EditorGUILayout.PropertyField(currentSequencePatternProperty.FindPropertyRelative("sendType"));
				EditorGUILayout.PropertyField(currentSequencePatternProperty.FindPropertyRelative("sendSize"));
				EditorGUILayout.PropertyField(currentSequencePatternProperty.FindPropertyRelative("subdivision"));
				
				if (EditorGUI.EndChangeCheck()) {
					currentSequencePatternProperty.serializedObject.ApplyModifiedProperties();
					currentSequencePattern.UpdatePatterns();
					currentSequencePatternProperty.serializedObject.Update();
				}
				
				GUILayout.Space(2);
				
				// Timeline
				Rect rect = EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(" ");
				EditorGUILayout.EndHorizontal();
				
				GUIStyle arrowStyle = new GUIStyle("boldLabel");
				arrowStyle.fontSize = 16;
				rect = EditorGUI.IndentedRect(rect);
				rect.height += 4;
				GUI.Box(new Rect(rect.x, rect.y, rect.width, rect.height + currentSequencePattern.sendSize * 16 + 18), "", new GUIStyle("ColorPickerBox"));
				float width = rect.width;
				rect.x -= 7;
				float x = rect.x;
				rect.width = 32;
				
				PureDataSequenceStep step = currentSequenceSelection.Step;
				for (int i = 0; i < step.Beats; i++) {
					rect.x = i * (width / step.Beats) + x;
					GUI.Label(new Rect(rect.x - (i + 1).ToString().GetWidth(EditorStyles.miniFont) / 2 + 8, rect.y - 3, rect.width, rect.height), (i + 1).ToString(), new GUIStyle("miniLabel"));
					GUI.Label(new Rect(rect.x + 4, rect.y + 4, rect.width, rect.height), "◤", arrowStyle);
				}
				
				// Pattern
				rect = EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(" ", GUILayout.Height(currentSequencePattern.sendSize * 16 + 20));
				EditorGUILayout.EndHorizontal();
				
				if (currentSequencePattern.sendType == PureDataPatternSendTypes.Bool) {
					rect.x += EditorGUI.indentLevel * 16 - 5;
					rect.width = ((Screen.width - rect.x - 27) / currentSequencePattern.subdivision);
				}
				else {
					rect.width = ((Screen.width - EditorGUI.indentLevel * 16F - 35) / currentSequencePattern.subdivision) + 75;
				}
				
				x = rect.x;
				float y = rect.y;
				rect.height = 16;
				
				EditorGUI.BeginChangeCheck();
				
				for (int i = 0; i < currentSequencePattern.sendSize; i++) {
					rect.y = y + i * 16;
					
					for (int j = 0; j < currentSequencePattern.subdivision; j++) {
						switch (currentSequencePattern.sendType) {
							case PureDataPatternSendTypes.Bool:
								rect.x = j * rect.width + x;
								currentSequencePattern.pattern[currentSequencePattern.subdivision * i + j] = EditorGUI.Toggle(rect, currentSequencePattern.pattern[currentSequencePattern.subdivision * i + j] != 0, new GUIStyle("toolbarButton")).GetHashCode();
								break;
							case PureDataPatternSendTypes.Float:
								rect.x = j * (rect.width - 75) + x;
								currentSequencePattern.pattern[currentSequencePattern.subdivision * i + j] = EditorGUI.FloatField(rect, currentSequencePattern.pattern[currentSequencePattern.subdivision * i + j]);
								break;
						}
					}
				}
				
				// Reset Button
				GUIStyle resetStyle = new GUIStyle("miniToolbarButton");
				resetStyle.contentOffset = new Vector2(1, 0);
				if (GUI.Button(new Rect(Screen.width - 77, rect.y + 20, 50, 16), "Reset", resetStyle)) {
					currentSequencePattern.ResetPatterns();
				}
				
				if (EditorGUI.EndChangeCheck()) {
					currentSequencePattern.SortPattern();
					currentSequencePatternProperty.serializedObject.Update();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void UpdateTrackStepsArray() {
			for (int i = 0; i < tracksProperty.arraySize; i++) {
				tracksProperty.GetArrayElementAtIndex(i).FindPropertyRelative("steps").arraySize = stepsProperty.arraySize;
			}
				
			tracksProperty.serializedObject.ApplyModifiedProperties();
		}

		void DeleteTrackStepFromTracks(int index) {
			for (int i = 0; i < tracksProperty.arraySize; i++) {
				DeleteFromArray(tracksProperty.GetArrayElementAtIndex(i).FindPropertyRelative("steps"), index);
			}
			
			tracksProperty.serializedObject.ApplyModifiedProperties();
		}
		
		string GetSequencePatternName(int index) {
			if (index < 0) {
				return "";
			}
			
			string patternName = alphabet[index % alphabet.Length];
			int overflowIndex = (int)Mathf.Floor(index / alphabet.Length) - 1;
			
			if (overflowIndex >= 0) {
				patternName += alphabet[overflowIndex];
			}
			
			return patternName;
		}
		
		GUIStyle GetSequenceStyle() {
			GUIStyle style = new GUIStyle("foldout");
			style.fontStyle = FontStyle.Bold;
			Color textColor = style.normal.textColor * 1.5F;
		
			if (Application.isPlaying) {
				switch (currentSequence.State) {
					case PureDataStates.Waiting:
						textColor = new Color(1, 1, 0, 10);
						break;
					case PureDataStates.Playing:
						textColor = new Color(0, 1, 0, 10);
						break;
					case PureDataStates.Stopping:
						textColor = new Color(1, 0.5F, 0, 10);
						break;
					case PureDataStates.Stopped:
						textColor = new Color(1, 0, 0, 10);
						break;
				}
			}

			style.normal.textColor = textColor * 0.7F;
			style.onNormal.textColor = textColor * 0.7F;
			style.focused.textColor = textColor * 0.85F;
			style.onFocused.textColor = textColor * 0.85F;
			style.active.textColor = textColor * 0.85F;
			style.onActive.textColor = textColor * 0.85F;
		
			return style;
		}
		
		GUIStyle GetSequenceStepStyle(int index) {
			GUIStyle style;
			
			if (Application.isPlaying && currentSequence.CurrentStepIndex == index) {
				style = new GUIStyle(index == currentSequenceSelection.stepIndex ? "flow node 3 on" : "flow node 3");
			}
			else if (Application.isPlaying) {
				style = new GUIStyle(index == currentSequenceSelection.stepIndex ? "flow node 6 on" : "flow node 6");
			}
			else {
				style = new GUIStyle(index == currentSequenceSelection.stepIndex ? "flow node 4 on" : "flow node 5");
			}
			
			return style;
		}
		
		GUIStyle GetTrackStyle() {
			GUIStyle style = new GUIStyle("miniLabel");
			style.fontStyle = currentTrack == currentSequenceSelection.Track ? FontStyle.Bold : FontStyle.Normal;
			style.alignment = TextAnchor.MiddleCenter;
			
			return style;
		}
		
		GUIStyle GetTrackStepStyle(int index) {
			string styleName;
			Color styleColor;
			
			if (currentSequenceSelection.Track == currentTrack && currentSequenceSelection.stepIndex == index) {
				styleName = "flow node 2 on";
				styleColor = EditorStyles.boldLabel.normal.textColor * 1.5F;
			}
			else if (currentSequenceSelection.Track == currentTrack || currentSequenceSelection.stepIndex == index) {
				styleName = "flow node 1 on";
				styleColor = EditorStyles.boldLabel.normal.textColor;
			}
			else {
				styleName = "flow node 1";
				styleColor = EditorStyles.boldLabel.normal.textColor;
			}
			
			GUIStyle style = new GUIStyle(styleName);
			style.normal.textColor = styleColor;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.UpperLeft;
			style.contentOffset = new Vector2(3, -28);
			
			return style;
		}
		#endregion
	}
	
	
	public class SequenceSelectionData {
		
		public PureDataSequenceStep Step {
			get {
				stepIndex = Mathf.Clamp(stepIndex, 0, sequence.steps.Length - 1);
				return stepIndex == -1 ? null : sequence.steps[stepIndex];
			}
		}
		
		public SerializedProperty StepProperty {
			get {
				stepIndex = Mathf.Clamp(stepIndex, 0, sequence.steps.Length - 1);
				return sequenceProperty.FindPropertyRelative("steps").GetArrayElementAtIndex(stepIndex);
			}
		}
		
		public PureDataSequenceTrack Track {
			get {
				trackIndex = Mathf.Clamp(trackIndex, 0, sequence.tracks.Length - 1);
				return sequence.tracks[trackIndex];
			}
		}
		
		public SerializedProperty TrackProperty {
			get {
				trackIndex = Mathf.Clamp(trackIndex, 0, sequence.tracks.Length - 1);
				return sequenceProperty.FindPropertyRelative("tracks").GetArrayElementAtIndex(trackIndex);
			}
		}
		
		public bool StepIsNull {
			get {
				return Step == null || StepProperty == null || stepIndex == -1;
			}
		}
		
		public bool TrackIsNull {
			get {
				return Track == null || TrackProperty == null || trackIndex == -1;
			}
		}
		
		public int stepIndex = -1;
		public int trackIndex = -1;
		public PureDataSequence sequence;
		public SerializedProperty sequenceProperty;
		
		public void Initialize(PureDataSequence currentSequence, SerializedProperty currentSequenceProperty) {
			sequence = currentSequence;
			sequenceProperty = currentSequenceProperty;
			
			if (StepIsNull) {
				SetStep(0);
			}
			
			if (TrackIsNull) {
				SetTrack(0);
			}
		}

		public void SetStep(int stepIndex) {
			this.stepIndex = stepIndex;
		}
		
		public void SetTrack(int trackIndex) {
			this.trackIndex = trackIndex;
		}
		
		public void RemoveStep() {
			stepIndex = -1;
		}
		
		public void RemoveTrack() {
			trackIndex = -1;
		}
	}
}