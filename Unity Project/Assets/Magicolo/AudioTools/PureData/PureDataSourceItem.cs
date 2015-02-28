using UnityEngine;
using System.Collections;
using Magicolo.AudioTools;

[System.Serializable]
public class PureDataSourceItem : PureDataSourceOrContainerItem {
		
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
		
	public string Path {
		get {
			return audioSource.Info.path;
		}
	}
		
	public int Samples {
		get {
			return audioSource.Info.samples;
		}
	}
		
	public int Frequency {
		get {
			return audioSource.Info.frequency;
		}
	}
		
	public int Channels {
		get {
			return audioSource.Info.channels;
		}
	}
		
	public float Length {
		get {
			return audioSource.Info.length;
		}
	}
		
	public bool LoadOnAwake {
		get {
			return audioSource.Info.loadOnAwake;
		}
	}
		
	public string Output {
		get {
			return audioSource.Info.output;
		}
	}
		
	public bool Fixed {
		get {
			return audioSource.Info.isFixed;
		}
	}
		
	public Vector2 PlayRange {
		get {
			return new Vector2(audioSource.Info.playRangeStart, audioSource.Info.playRangeEnd);
		}
	}
		
	public float Volume {
		get {
			return audioSource.Info.volume;
		}
	}
		
	public float Pitch {
		get {
			return audioSource.Info.pitch;
		}
	}
		
	public float RandomVolume {
		get {
			return audioSource.Info.randomVolume;
		}
	}
		
	public float RandomPitch {
		get {
			return audioSource.Info.randomPitch;
		}
	}
		
	public float FadeIn {
		get {
			return audioSource.Info.fadeIn;
		}
	}
		
	public float FadeOut {
		get {
			return audioSource.Info.fadeOut;
		}
	}
		
	public bool Loop {
		get {
			return audioSource.Info.loop;
		}
	}
		
	public object Source {
		get {
			return audioSource.spatializer.Source;
		}
	}
		
	public float DopplerLevel {
		get {
			return audioSource.Info.dopplerLevel;
		}
	}
		
	public PureDataVolumeRolloffModes VolumeRolloffMode {
		get {
			return audioSource.Info.volumeRolloffMode;
		}
	}
		
	public float MinDistance {
		get {
			return audioSource.Info.minDistance;
		}
	}
		
	public float MaxDistance {
		get {
			return audioSource.Info.maxDistance;
		}
	}
		
	public float PanLevel {
		get {
			return audioSource.Info.panLevel;
		}
	}

	public PureDataSource audioSource;
				
	public PureDataSourceItem(PureDataSource audioSource, PureData pureData)
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
