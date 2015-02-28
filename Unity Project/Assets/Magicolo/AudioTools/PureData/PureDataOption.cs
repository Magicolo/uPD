using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo;
using Magicolo.AudioTools;

/// <summary>
/// Lets you override the settings of a sound set in the inspector.
/// </summary>
[System.Serializable]
public class PureDataOption {

	public enum OptionTypes {
		Volume,
		Pitch,
		RandomVolume,
		RandomPitch,
		FadeIn,
		FadeOut,
		Loop,
		Clip,
		Output,
		PlayRange,
		Time,
		DopplerLevel,
		VolumeRolloffMode,
		MinDistance,
		MaxDistance,
		PanLevel,
		StepTempo,
		StepBeats,
		StepPattern,
		TrackSendType,
		TrackPattern
	}
	
	public OptionTypes type;
	public PureDataOptionValue value;
	public float delay;
	
	public bool IsDelayable {
		get {
			return DelayableTypes.Contains(type);
		}
	}
	
	static readonly OptionTypes[] DelayableTypes = { OptionTypes.Volume, OptionTypes.Pitch, OptionTypes.Output, OptionTypes.Time, OptionTypes.PlayRange, OptionTypes.RandomPitch, OptionTypes.RandomVolume };

	PureDataOption(OptionTypes type, object value, object defaultValue, float delay = 0) {
		this.type = type;
		this.value = new PureDataOptionValue(value, defaultValue);
		this.delay = delay;
	}

	public static PureDataOption Clip(string clipName) {
		return new PureDataOption(OptionTypes.Clip, clipName, null);
	}
	
	public static PureDataOption Output(string busName, float delay) {
		return new PureDataOption(OptionTypes.Output, busName, "Master", delay);
	}
	
	public static PureDataOption Output(string busName) {
		return Output(busName, 0);
	}
	
	public static PureDataOption FadeIn(float fadeIn) {
		return new PureDataOption(OptionTypes.FadeIn, fadeIn, 0);
	}
	
	public static PureDataOption FadeOut(float fadeOut) {
		return new PureDataOption(OptionTypes.FadeOut, fadeOut, 0);
	}
	
	public static PureDataOption Volume(float volume, float time, float delay) {
		return new PureDataOption(OptionTypes.Volume, new []{ volume, time }, new []{ 1F, 0F }, delay);
	}
	
	public static PureDataOption Volume(float volume, float time) {
		return Volume(volume, time, 0);
	}
	
	public static PureDataOption Volume(float volume) {
		return Volume(volume, 0, 0);
	}
	
	public static PureDataOption Pitch(float pitch, float time, float delay) {
		return new PureDataOption(OptionTypes.Pitch, new []{ pitch, time }, new []{ 1F, 0F }, delay);
	}
	
	public static PureDataOption Pitch(float pitch, float time) {
		return Pitch(pitch, time, 0);
	}
	
	public static PureDataOption Pitch(float pitch) {
		return Pitch(pitch, 0, 0);
	}
	
	public static PureDataOption RandomVolume(float randomVolume, float delay) {
		return new PureDataOption(OptionTypes.RandomVolume, randomVolume, 0, delay);
	}
	
	public static PureDataOption RandomVolume(float randomVolume) {
		return RandomVolume(randomVolume, 0);
	}
	
	public static PureDataOption RandomPitch(float randomPitch, float delay) {
		return new PureDataOption(OptionTypes.RandomPitch, randomPitch, 0, delay);
	}
	
	public static PureDataOption RandomPitch(float randomPitch) {
		return RandomPitch(randomPitch, 0);
	}
	
	public static PureDataOption Loop(bool loop) {
		return new PureDataOption(OptionTypes.Loop, loop, false);
	}
	
	public static PureDataOption DopplerLevel(float dopplerLevel) {
		return new PureDataOption(OptionTypes.DopplerLevel, dopplerLevel, 1);
	}
	
	public static PureDataOption VolumeRolloffMode(PureDataVolumeRolloffModes volumeRolloffMode) {
		return new PureDataOption(OptionTypes.VolumeRolloffMode, volumeRolloffMode, (float)PureDataVolumeRolloffModes.Logarithmic);
	}
	
	public static PureDataOption MinDistance(float minDistance) {
		return new PureDataOption(OptionTypes.MinDistance, minDistance, 5);
	}
	
	public static PureDataOption MaxDistance(float maxDistance) {
		return new PureDataOption(OptionTypes.MaxDistance, maxDistance, 500);
	}
	
	public static PureDataOption PanLevel(float panLevel) {
		return new PureDataOption(OptionTypes.PanLevel, panLevel, 0.75F);
	}
		
	public static PureDataOption PlayRange(float start, float end, float delay) {
		return new PureDataOption(OptionTypes.PlayRange, new []{ start, end }, new []{ 0F, 1F }, delay);
	}

	public static PureDataOption PlayRange(float start, float end) {
		return PlayRange(start, end, 0);
	}

	public static PureDataOption Time(float time, float delay) {
		return new PureDataOption(OptionTypes.Time, time, 0, delay);
	}
	
	public static PureDataOption Time(float time) {
		return Time(time, 0);
	}
	
	public static PureDataOption StepTempo(int stepIndex, float tempo) {
		return new PureDataOption(OptionTypes.StepTempo, new []{ (float)stepIndex, (float)tempo }, new []{ 0F, 120F });
	}
	
	public static PureDataOption StepBeats(int stepIndex, int beats) {
		return new PureDataOption(OptionTypes.StepBeats, new []{ (float)stepIndex, (float)beats }, new []{ 0F, 4F });
	}
	
	public static PureDataOption StepPattern(int trackIndex, int stepIndex, int patternIndex) {
		return new PureDataOption(OptionTypes.StepPattern, new []{ (float)trackIndex, (float)stepIndex, (float)patternIndex }, new []{ 0F, 0F, 0F });
	}

	public static PureDataOption TrackSendType(int trackIndex, int patternIndex, PureDataPatternSendTypes sendType) {
		return new PureDataOption(OptionTypes.TrackSendType, new []{ (float)trackIndex, (float)patternIndex, (float)sendType }, new []{ 0F, 0F, 0F });
	}

	public static PureDataOption TrackPattern(int trackIndex, int patternIndex, float[] pattern) {
		float[] patternData = new float[pattern.Length + 4];
		patternData[0] = trackIndex;
		patternData[1] = patternIndex;
		patternData[2] = 1;
		patternData[3] = pattern.Length;
		pattern.CopyTo(patternData, 4);
		
		return new PureDataOption(OptionTypes.TrackPattern, patternData, new float[patternData.Length]);
	}

	public static PureDataOption TrackPattern(int trackIndex, int patternIndex, float[,] pattern) {
		int sendSize = pattern.GetLength(0);
		int subdivision = pattern.GetLength(1);
		float[] patternData = new float[pattern.Length + 4];
		patternData[0] = trackIndex;
		patternData[1] = patternIndex;
		patternData[2] = sendSize;
		patternData[3] = subdivision;
		
		for (int row = 0; row < sendSize; row++) {
			for (int column = 0; column < subdivision; column++) {
				patternData[row * subdivision + column + 4] = pattern[row, column];
			}
		}
		
		return new PureDataOption(OptionTypes.TrackPattern, patternData, new float[patternData.Length]);
	}

	public string GetValueDisplayName() {
		return value.GetValueDisplayName();
	}
	
	public T GetValue<T>() {
		return value.GetValue<T>();
	}
	
	public object GetValue() {
		return value.GetValue();
	}

	public void SetValue(object value) {
		this.value.SetValue(value);
	}
	
	public void SetDefaultValue(object value) {
		this.value.SetDefaultValue(value);
	}
	
	public void ResetValue() {
		value.ResetValue();
	}
	
	public void Apply(PureDataSource source) {
		switch (type) {
			case PureDataOption.OptionTypes.Volume:
				float[] volumeData = GetValue<float[]>();
				source.SetVolume(volumeData[0], volumeData[1], delay);
				break;
			case PureDataOption.OptionTypes.Pitch:
				float[] pitchData = GetValue<float[]>();
				source.SetPitch(pitchData[0], pitchData[1], delay);
				break;
			case PureDataOption.OptionTypes.RandomVolume:
				float randomVolume = GetValue<float>();
				source.SetVolume(source.Info.volume + source.Info.volume * HelperFunctions.RandomRange(-randomVolume, randomVolume), 0, delay);
				break;
			case PureDataOption.OptionTypes.RandomPitch:
				float randomPitch = GetValue<float>();
				source.SetPitch(source.Info.pitch + source.Info.pitch * HelperFunctions.RandomRange(-randomPitch, randomPitch), 0, delay);
				break;
			case PureDataOption.OptionTypes.FadeIn:
				source.SetFadeIn(GetValue<float>());
				break;
			case PureDataOption.OptionTypes.FadeOut:
				source.SetFadeOut(GetValue<float>());
				break;
			case PureDataOption.OptionTypes.Loop:
				source.SetLoop(GetValue<bool>());
				break;
			case PureDataOption.OptionTypes.Clip:
				source.SetClip(source.pureData.clipManager.GetClip(GetValue<string>()));
				break;
			case PureDataOption.OptionTypes.Output:
				source.SetOutput(GetValue<string>(), delay);
				break;
			case PureDataOption.OptionTypes.PlayRange:
				float[] playRangeData = GetValue<float[]>();
				source.SetPlayRange(playRangeData[0], playRangeData[1], delay, true);
				break;
			case PureDataOption.OptionTypes.Time:
				source.SetPhase(GetValue<float>(), delay);
				break;
			case PureDataOption.OptionTypes.DopplerLevel:
				source.spatializer.DopplerLevel = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.VolumeRolloffMode:
				source.spatializer.VolumeRolloffMode = (PureDataVolumeRolloffModes)GetValue<float>();
				break;
			case PureDataOption.OptionTypes.MinDistance:
				source.spatializer.MinDistance = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.MaxDistance:
				source.spatializer.MaxDistance = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.PanLevel:
				source.spatializer.PanLevel = GetValue<float>();
				break;
			default:
				Logger.LogError(string.Format("{0} can not be applied to {1}.", this, source.GetType()));
				break;
		}
	}
	
	public void Apply(PureDataSequence sequence) {
		switch (type) {
			case PureDataOption.OptionTypes.Volume:
				float[] volumeData = GetValue<float[]>();
				sequence.SetVolume(volumeData[0], volumeData[1], delay);
				break;
			case PureDataOption.OptionTypes.Loop:
				sequence.SetLoop(GetValue<bool>());
				break;
			case PureDataOption.OptionTypes.Output:
				sequence.SetOutput(GetValue<string>(), delay);
				break;
			case PureDataOption.OptionTypes.VolumeRolloffMode:
				sequence.spatializer.VolumeRolloffMode = (PureDataVolumeRolloffModes)GetValue<float>();
				break;
			case PureDataOption.OptionTypes.MinDistance:
				sequence.spatializer.MinDistance = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.MaxDistance:
				sequence.spatializer.MaxDistance = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.PanLevel:
				sequence.spatializer.PanLevel = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.StepTempo:
				float[] stepTempoData = GetValue<float[]>();
				sequence.SetStepTempo((int)stepTempoData[0], stepTempoData[1]);
				break;
			case PureDataOption.OptionTypes.StepBeats:
				float[] stepBeatsData = GetValue<float[]>();
				sequence.SetStepBeats((int)stepBeatsData[0], (int)stepBeatsData[1]);
				break;
			case PureDataOption.OptionTypes.StepPattern:
				float[] stepPatternData = GetValue<float[]>();
				sequence.SetStepPattern((int)stepPatternData[0], (int)stepPatternData[1], (int)stepPatternData[2]);
				break;
			case PureDataOption.OptionTypes.TrackSendType:
				float[] trackSendTypeData = GetValue<float[]>();
				sequence.SetTrackSendType((int)trackSendTypeData[0], (int)trackSendTypeData[1], (PureDataPatternSendTypes)trackSendTypeData[2]);
				break;
			case PureDataOption.OptionTypes.TrackPattern:
				float[] trackPatternData = GetValue<float[]>();
				sequence.SetTrackPattern((int)trackPatternData[0], (int)trackPatternData[1], (int)trackPatternData[2], (int)trackPatternData[3], trackPatternData.Slice(4));
				break;
			default:
				Logger.LogError(string.Format("{0} can not be applied to {1}.", this, sequence.GetType()));
				break;
		}
	}
	
	public override string ToString() {
		return string.Format("PureDataOption({0}, {1})", type, GetValueDisplayName());
	}
}
