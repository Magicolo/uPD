using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.AudioTools;

[System.Serializable]
public class PureDataSequenceItem : PureDataItem {

	public override string Name {
		get {
			return sequence.Name;
		}
	}

	public override PureDataStates State {
		get {
			return sequence.State;
		}
	}

	public string Output {
		get {
			return sequence.output;
		}
	}
		
	public bool Loop {
		get {
			return sequence.loop;
		}
	}
		
	public float Volume {
		get {
			return sequence.volume;
		}
	}
		
	public object Source {
		get {
			return sequence.spatializer.Source;
		}
	}
		
	public PureDataVolumeRolloffModes VolumeRolloffMode {
		get {
			return sequence.spatializer.VolumeRolloffMode;
		}
	}
		
	public float MinDistance {
		get {
			return sequence.spatializer.MinDistance;
		}
	}
		
	public float MaxDistance {
		get {
			return sequence.spatializer.MaxDistance;
		}
	}
		
	public float PanLevel {
		get {
			return sequence.spatializer.PanLevel;
		}
	}
		
	public int CurrentStepIndex {
		get {
			return sequence.CurrentStepIndex;
		}
	}

	public int NextStepIndex {
		get {
			int index = sequence.CurrentStepIndex + 1;
				
			if (index >= GetStepCount()) {
				index = sequence.loop ? index % GetStepCount() : -1;
			}
				
			return index;
		}
	}

	protected PureDataSequence sequence;
		
	public PureDataSequenceItem(PureDataSequence sequence, PureData pureData)
		: base(pureData) {
			
		this.sequence = sequence;
	}
		
	public override void Play(float delay) {
		sequence.Play(delay);
	}

	public override void Stop(float delay) {
		sequence.Stop(delay);
	}

	public override void StopImmediate() {
		sequence.StopImmediate();
	}

	public float GetStepTempo(int stepIndex) {
		return sequence.GetStepTempo(stepIndex);
	}
		
	public float GetCurrentStepTempo() {
		return sequence.GetStepTempo(CurrentStepIndex);
	}
		
	public int GetStepBeats(int stepIndex) {
		return sequence.GetStepBeats(stepIndex);
	}

	public float GetCurrentStepBeats() {
		return sequence.GetStepBeats(CurrentStepIndex);
	}
		
	public int GetStepPattern(int trackIndex, int stepIndex) {
		return sequence.GetStepPattern(trackIndex, stepIndex);
	}
		
	public int GetCurrentStepPattern(int trackIndex) {
		return sequence.GetStepPattern(trackIndex, CurrentStepIndex);
	}

	public int GetStepCount() {
		return sequence.steps.Length;
	}
		
	public int GetTrackCount() {
		return sequence.tracks.Length;
	}
		
	public int GetPatternCount(int trackIndex) {
		return sequence.tracks[trackIndex].patterns.Length;
	}
		
	public override void ApplyOptions(params PureDataOption[] options) {
		sequence.ApplyOptions(options);
	}

	public override string ToString() {
		return string.Format("{0}({1}, {2})", typeof(PureDataSequenceItem).Name, Name, State);
	}
}