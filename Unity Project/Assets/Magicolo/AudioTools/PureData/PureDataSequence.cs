using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSequence : PureDataIdManager<PureDataSequenceTrack>, INamable, IIdentifiable {

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
		int id;
		public int Id {
			get {
				return id;
			}
			set {
				id = value;
				spatializer.UpdateSendNames(this);
			}
		}
		
		PureDataStates state;
		public PureDataStates State {
			get {
				return state;
			}
			set {
				state = value;
				pureData.editorHelper.RepaintInspector();
			}
		}
		
		public bool HasValidTracks {
			get {
				foreach (PureDataSequenceTrack track in tracks) {
					if (track.IsValid) {
						return true;
					}
				}
				
				return false;
			}
		}
		
		int currentStepIndex;
		public int CurrentStepIndex {
			get {
				return pureData.generalSettings.ApplicationPlaying ? currentStepIndex : -1;
			}
			set {
				currentStepIndex = value;
				pureData.editorHelper.RepaintInspector();
			}
		}
		
		int nextStepIndex;
		public int NextStepIndex {
			get {
				return pureData.generalSettings.ApplicationPlaying ? nextStepIndex : -1;
			}
			set {
				nextStepIndex = value;
				pureData.editorHelper.RepaintInspector();
			}
		}
		
		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 5)]
		public float volume = 1;
		public float Volume {
			get {
				return volume;
			}
			set {
				volume = value;
				if (pureData.generalSettings.ApplicationPlaying) {
					SetVolume(volume, 0.01F);
				}
			}
		}
		
		public string output = "Master";
		public bool loop;
		[Min] public float sleepTime = 1;
		
		bool sequenceSwitch;
		bool stepperSwitch;
		bool stopperSwitch;
		float tickSpeed;
		Queue nextSources;
		
		public PureDataSequenceStep[] steps = new PureDataSequenceStep[4];
		public PureDataSequenceTrack[] tracks;
		public PureDataSequenceSpatializer spatializer;
		public PureData pureData;
		
		public PureDataSequence(PureData pureData) {
			this.pureData = pureData;
			
			tracks = new []{ new PureDataSequenceTrack(pureData) };
			spatializer = new PureDataSequenceSpatializer(pureData);
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			foreach (PureDataSequenceTrack track in tracks) {
				track.Initialize(pureData);
			}
			
			spatializer.Initialize(pureData);
		}

		public void Start() {
			nextSources = new Queue();
			CurrentStepIndex = -1;
			NextStepIndex = -1;
			state = PureDataStates.Stopped;
			
			foreach (PureDataSequenceTrack track in tracks) {
				track.Start();
			}
		}
		
		public void Update() {
			spatializer.Update();
		}
		
		public void Play(float delay = 0) {
			if (state == PureDataStates.Waiting) {
				SetSleepTime(sleepTime);
				
				if (delay > 0) {
					pureData.communicator.SendDelayedMessage("usequence_play", delay, Id);
				}
				else {
					state = PureDataStates.Playing;
					NextStepIndex = -1;
			
					SwitchOff();
					SetSource(nextSources.Dequeue());
					SetOutput(output);
					SetVolume(volume, 0.01F);
					SetTickSpeed(60F * steps[0].Beats / steps[0].Tempo);
					SwitchOn();
				
					pureData.sequenceManager.Activate(this);
					pureData.communicator.SendMessage<bool>("usequence_messages" + Id, "Play");
				}
			}
		}
		
		public void Stop(float delay = 0) {
			if (state != PureDataStates.Stopped && state != PureDataStates.Stopping) {
				if (delay > 0) {
					pureData.communicator.SendDelayedMessage("usequence_stop", delay, Id);
				}
				else {
					state = PureDataStates.Stopping;
					
					SetStepperSwitch(false);
					SetStopperSwitch(true);
				
					foreach (PureDataSequenceTrack track in tracks) {
						pureData.communicator.SendBang(string.Format("utrack_pattern{0}_{1}", Id, track.Id));
					}
				}
			}
		}
		
		public void StopImmediate() {
			if (state != PureDataStates.Stopped) {
				state = PureDataStates.Stopped;
				NextStepIndex = -1;
				CurrentStepIndex = -1;
				nextSources.Clear();
			
				SwitchOff();
			
				pureData.sequenceManager.Deactivate(this);
				pureData.communicator.SendMessage<bool>("usequence_messages" + Id, "Sleep");
			}
		}
			
		public void Step() {
			NextStepIndex += 1;
			CurrentStepIndex = NextStepIndex;
			
			if (CurrentStepIndex >= steps.Length && loop) {
				NextStepIndex = 0;
				CurrentStepIndex = 0;
			}
			
			if (CurrentStepIndex < steps.Length) {
				PureDataSequenceStep step = steps[CurrentStepIndex];
				
				SetTickSpeed(60F * step.Beats / step.Tempo);
				
				foreach (PureDataSequenceTrack track in tracks) {
					track.Step(tickSpeed, CurrentStepIndex, this);
				}
				
				pureData.communicator.SendMessage("usequence_messages" + Id, "Step", CurrentStepIndex);
			}
			else {
				Stop();
				pureData.communicator.SendMessage<bool>("usequence_messages" + Id, "Stop");
			}
		}
		
		public void SwitchOn(float delay = 0) {
			SetSequenceSwitch(true, delay);
			SetStepperSwitch(true, delay);
		}
		
		public void SwitchOff(float delay = 0) {
			SetSequenceSwitch(false, delay);
			SetStepperSwitch(false, delay);
			SetStopperSwitch(false, delay);
		}
		
		public void SetNextSource(object source) {
			nextSources.Enqueue(source);
		}
		
		public void SetSource(object source) {
			spatializer.Source = source;
		}
		
		public void SetVolume(float targetVolume, float time = 0, float delay = 0) {
			volume = Mathf.Max(targetVolume, 0);
			time = Mathf.Max(time, 0);
			
			pureData.communicator.SendDelayedMessage("usequence_volume" + Id, delay, volume, time * 1000);
		}

		public void SetLoop(bool targetLoop) {
			loop = targetLoop;
		}
		
		public void SetSequenceSwitch(bool targetSwitch, float delay = 0) {
			sequenceSwitch = targetSwitch;
			
			pureData.communicator.SendDelayedMessage("usequence_switch" + Id, delay, sequenceSwitch);
		}
		
		public void SetStepperSwitch(bool targetSwitch, float delay = 0) {
			stepperSwitch = targetSwitch;
			
			pureData.communicator.SendDelayedMessage("usequence_stepper" + Id, delay, stepperSwitch);
		}
		
		public void SetStopperSwitch(bool targetSwitch, float delay = 0) {
			stopperSwitch = targetSwitch;
			
			pureData.communicator.SendDelayedMessage("usequence_stopper" + Id, delay, stopperSwitch);
		}
				
		public void SetSleepTime(float targetSleepTime, float delay = 0) {
			sleepTime = targetSleepTime;
			
			pureData.communicator.SendDelayedMessage("usequence_sleep" + Id, delay, sleepTime * 1000);
		}
		
		public void SetTickSpeed(float targetTickSpeed, float delay = 0) {
			tickSpeed = targetTickSpeed;
			
			pureData.communicator.SendDelayedMessage("usequence_metro" + Id, delay, tickSpeed * 1000);
		}
		
		public void SetOutput(string targetOutput, float delay = 0) {
			output = targetOutput;
			
			pureData.communicator.SendDelayedMessage("usequence_output" + Id, delay, output);
		}

		public void SetStepTempo(int stepIndex, float tempo) {
			steps[stepIndex].Tempo = tempo;
			pureData.editorHelper.RepaintInspector();
		}

		public void SetStepBeats(int stepIndex, int beats) {
			steps[stepIndex].Beats = beats;
			pureData.editorHelper.RepaintInspector();
		}

		public void SetStepPattern(int trackIndex, int stepIndex, int patternIndex) {
			tracks[trackIndex].SetStepPattern(stepIndex, patternIndex);
			pureData.editorHelper.RepaintInspector();
		}

		public void SetTrackSendType(int trackIndex, int patternIndex, PureDataPatternSendTypes sendType) {
			tracks[trackIndex].SetSendType(patternIndex, sendType);
			pureData.editorHelper.RepaintInspector();
		}

		public void SetTrackPattern(int trackIndex, int patternIndex, int sendSize, int subdivision, float[] pattern) {
			tracks[trackIndex].SetPattern(patternIndex, sendSize, subdivision, pattern);
			pureData.editorHelper.RepaintInspector();
		}
		
		public float GetStepTempo(int stepIndex) {
			return steps[stepIndex].Tempo;
		}

		public int GetStepBeats(int stepIndex) {
			return steps[stepIndex].Beats;
		}
		
		public int GetStepPattern(int trackIndex, int stepIndex) {
			return tracks[trackIndex].steps[stepIndex].patternIndex;
		}
		
		public void ApplyOptions(params PureDataOption[] options) {
			foreach (PureDataOption option in options) {
				option.Apply(this);
			}
		}
	}
}
