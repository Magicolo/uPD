using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSourceItemInternal : PureDataSourceItem {

		public override string Name {
			get {
				return audioSource.Name;
			}
		}
		
		public override PureDataStates State {
			get {
				return audioSource.State;
			}
		}
		
		public override string Path {
			get {
				return audioSource.Info.path;
			}
		}
		
		public override int Samples {
			get {
				return audioSource.Info.samples;
			}
		}
		
		public override int Frequency {
			get {
				return audioSource.Info.frequency;
			}
		}
		
		public override int Channels {
			get {
				return audioSource.Info.channels;
			}
		}
		
		public override float Length {
			get {
				return audioSource.Info.length;
			}
		}
		
		public override bool LoadOnAwake {
			get {
				return audioSource.Info.loadOnAwake;
			}
		}
		
		public override string Output {
			get {
				return audioSource.Info.output;
			}
		}
		
		public override bool Fixed {
			get {
				return audioSource.Info.isFixed;
			}
		}
		
		public override Vector2 PlayRange {
			get {
				return new Vector2(audioSource.Info.playRangeStart, audioSource.Info.playRangeEnd);
			}
		}
		
		public override float Volume {
			get {
				return audioSource.Info.volume;
			}
		}
		
		public override float Pitch {
			get {
				return audioSource.Info.pitch;
			}
		}
		
		public override float RandomVolume {
			get {
				return audioSource.Info.randomVolume;
			}
		}
		
		public override float RandomPitch {
			get {
				return audioSource.Info.randomPitch;
			}
		}
		
		public override float FadeIn {
			get {
				return audioSource.Info.fadeIn;
			}
		}
		
		public override float FadeOut {
			get {
				return audioSource.Info.fadeOut;
			}
		}
		
		public override bool Loop {
			get {
				return audioSource.Info.loop;
			}
		}
		
		public override object Source {
			get {
				return audioSource.spatializer.Source;
			}
		}
		
		public override float DopplerLevel {
			get {
				return audioSource.Info.dopplerLevel;
			}
		}
		
		public override PureDataVolumeRolloffModes VolumeRolloffMode {
			get {
				return audioSource.Info.volumeRolloffMode;
			}
		}
		
		public override float MinDistance {
			get {
				return audioSource.Info.minDistance;
			}
		}
		
		public override float MaxDistance {
			get {
				return audioSource.Info.maxDistance;
			}
		}
		
		public override float PanLevel {
			get {
				return audioSource.Info.panLevel;
			}
		}

		public PureDataSource audioSource;
				
		public PureDataSourceItemInternal(PureDataSource audioSource, PureData pureData)
			: base(pureData) {
			
			this.audioSource = audioSource;
		}
	
		public override void Play(float delay) {
			audioSource.Play(delay);
		}

		public override void Pause(float delay) {
			audioSource.Pause(delay);
		}
		
		public override void Stop(float delay) {
			audioSource.Stop(delay);
		}

		public override void StopImmediate() {
			audioSource.StopImmediate();
		}

		public override void Load() {
			audioSource.Load();
		}
		
		public override void Unload() {
			audioSource.Unload();
		}

		public override void ApplyOptions(params PureDataOption[] options) {
			audioSource.ApplyOptions(options);
		}

		public override string ToString() {
			return string.Format("{0}({1}, {2})", typeof(PureDataSourceItem).Name, Name, State);
		}
	}
}