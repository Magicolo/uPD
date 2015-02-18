using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSequenceItemInternal : PureDataSequenceItem {

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

		public override string Output {
			get {
				return sequence.output;
			}
		}
		
		public override bool Loop {
			get {
				return sequence.loop;
			}
		}
		
		public override float Volume {
			get {
				return sequence.volume;
			}
		}
		
		public override object Source {
			get {
				return sequence.spatializer.Source;
			}
		}
		
		public override PureDataVolumeRolloffModes VolumeRolloffMode {
			get {
				return sequence.spatializer.VolumeRolloffMode;
			}
		}
		
		public override float MinDistance {
			get {
				return sequence.spatializer.MinDistance;
			}
		}
		
		public override float MaxDistance {
			get {
				return sequence.spatializer.MaxDistance;
			}
		}
		
		public override float PanLevel {
			get {
				return sequence.spatializer.PanLevel;
			}
		}
		
		public override int CurrentStepIndex {
			get {
				return sequence.CurrentStepIndex;
			}
		}

		public PureDataSequence sequence;
		
		public PureDataSequenceItemInternal(PureDataSequence sequence, PureData pureData)
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

		public override float GetStepTempo(int stepIndex) {
			return sequence.GetStepTempo(stepIndex);
		}
		
		public override int GetStepBeats(int stepIndex) {
			return sequence.GetStepBeats(stepIndex);
		}

		public override void ApplyOptions(params PureDataOption[] options) {
			sequence.ApplyOptions(options);
		}

		public override string ToString() {
			return string.Format("{0}({1}, {2})", typeof(PureDataSequenceItem).Name, Name, State);
		}
	}
}