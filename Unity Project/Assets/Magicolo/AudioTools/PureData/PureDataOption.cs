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
		PanLevel
	}
	
	public OptionTypes type;
	public float delay;
	
	public bool IsFloat {
		get {
			return FloatTypes.Contains(type);
		}
	}
	
	public bool IsString {
		get {
			return StringTypes.Contains(type);
		}
	}
		
	public bool IsBool {
		get {
			return BoolTypes.Contains(type);
		}
	}
		
	public bool IsVector2 {
		get {
			return Vector2Types.Contains(type);
		}
	}
		
	public bool IsVolumeRolloffMode {
		get {
			return VolumeRolloffModeTypes.Contains(type);
		}
	}
		
	public bool IsClip {
		get {
			return ClipTypes.Contains(type);
		}
	}
	
	public bool IsDelayable {
		get {
			return DelayableTypes.Contains(type);
		}
	}
	
	[SerializeField]
	float floatValue;
	
	[SerializeField]
	string stringValue;
	
	[SerializeField]
	bool boolValue;
	
	[SerializeField]
	Vector2 vector2Value;
	
	[SerializeField]
	PureDataVolumeRolloffModes volumeRolloffModeValue;
	
	[SerializeField]
	AudioClip clipValue;
	
	static readonly OptionTypes[] FloatTypes = { OptionTypes.FadeIn, OptionTypes.FadeOut, OptionTypes.RandomVolume, OptionTypes.RandomPitch, OptionTypes.DopplerLevel, OptionTypes.MinDistance, OptionTypes.MaxDistance, OptionTypes.PanLevel, OptionTypes.Time };
	static readonly OptionTypes[] StringTypes = { OptionTypes.Output };
	static readonly OptionTypes[] BoolTypes = { OptionTypes.Loop };
	static readonly OptionTypes[] Vector2Types = { OptionTypes.PlayRange, OptionTypes.Volume, OptionTypes.Pitch };
	static readonly OptionTypes[] VolumeRolloffModeTypes = { OptionTypes.VolumeRolloffMode };
	static readonly OptionTypes[] ClipTypes = { OptionTypes.Clip };
	static readonly OptionTypes[] DelayableTypes = { OptionTypes.Volume, OptionTypes.Pitch, OptionTypes.Output, OptionTypes.Time, OptionTypes.PlayRange, OptionTypes.RandomPitch, OptionTypes.RandomVolume };

	PureDataOption(OptionTypes type, object value, float delay = 0) {
		this.type = type;
		SetDefaultValue();
		SetValue(value);
		this.delay = delay;
	}

	public static PureDataOption Clip(AudioClip clip) {
		return new PureDataOption(OptionTypes.Clip, clip);
	}
	
	public static PureDataOption Output(string busName, float delay) {
		return new PureDataOption(OptionTypes.Output, busName, delay);
	}
	
	public static PureDataOption Output(string busName) {
		return Output(busName, 0);
	}
	
	public static PureDataOption FadeIn(float fadeIn) {
		return new PureDataOption(OptionTypes.FadeIn, fadeIn);
	}
	
	public static PureDataOption FadeOut(float fadeOut) {
		return new PureDataOption(OptionTypes.FadeOut, fadeOut);
	}
	
	public static PureDataOption Volume(float volume, float time, float delay) {
		return new PureDataOption(OptionTypes.Volume, new Vector2(volume, time), delay);
	}
	
	public static PureDataOption Volume(float volume, float time) {
		return Volume(volume, time, 0);
	}
	
	public static PureDataOption Volume(float volume) {
		return Volume(volume, 0, 0);
	}
	
	public static PureDataOption Pitch(float pitch, float time, float delay) {
		return new PureDataOption(OptionTypes.Pitch, new Vector2(pitch, time), delay);
	}
	
	public static PureDataOption Pitch(float pitch, float time) {
		return Pitch(pitch, time, 0);
	}
	
	public static PureDataOption Pitch(float pitch) {
		return Pitch(pitch, 0, 0);
	}
	
	public static PureDataOption RandomVolume(float randomVolume, float delay) {
		return new PureDataOption(OptionTypes.RandomVolume, randomVolume, delay);
	}
	
	public static PureDataOption RandomVolume(float randomVolume) {
		return RandomVolume(randomVolume, 0);
	}
	
	public static PureDataOption RandomPitch(float randomPitch, float delay) {
		return new PureDataOption(OptionTypes.RandomPitch, randomPitch, delay);
	}
	
	public static PureDataOption RandomPitch(float randomPitch) {
		return RandomPitch(randomPitch, 0);
	}
	
	public static PureDataOption Loop(bool loop) {
		return new PureDataOption(OptionTypes.Loop, loop);
	}
	
	public static PureDataOption DopplerLevel(float dopplerLevel) {
		return new PureDataOption(OptionTypes.DopplerLevel, dopplerLevel);
	}
	
	public static PureDataOption VolumeRolloffMode(PureDataVolumeRolloffModes volumeRolloffMode) {
		return new PureDataOption(OptionTypes.VolumeRolloffMode, volumeRolloffMode);
	}
	
	public static PureDataOption MinDistance(float minDistance) {
		return new PureDataOption(OptionTypes.MinDistance, minDistance);
	}
	
	public static PureDataOption MaxDistance(float maxDistance) {
		return new PureDataOption(OptionTypes.MaxDistance, maxDistance);
	}
	
	public static PureDataOption PanLevel(float panLevel) {
		return new PureDataOption(OptionTypes.PanLevel, panLevel);
	}
		
	public static PureDataOption PlayRange(float start, float end, float delay) {
		return new PureDataOption(OptionTypes.PlayRange, new Vector2(start, end), delay);
	}

	public static PureDataOption PlayRange(float start, float end) {
		return PlayRange(start, end, 0);
	}

	public static PureDataOption Time(float time, float delay) {
		return new PureDataOption(OptionTypes.Time, Mathf.Clamp01(time), delay);
	}
	
	public static PureDataOption Time(float time) {
		return Time(time, 0);
	}
	
	public string GetValueDisplayName() {
		return Logger.ObjectToString(GetValue()).Replace("Vector2", "");
	}
	
	public T GetValue<T>() {
		return (T)GetValue();
	}
	
	public object GetValue() {
		if (IsFloat) {
			return floatValue;
		}
		if (IsString) {
			return stringValue;
		}
		if (IsBool) {
			return boolValue;
		}
		if (IsVector2) {
			return vector2Value;
		}
		if (IsVolumeRolloffMode) {
			return volumeRolloffModeValue;
		}
		if (IsClip) {
			return clipValue;
		}
		return null;
	}

	public void SetValue(object value) {
		if (value is float) {
			floatValue = (float)value;
		}
		else if (value is string) {
			stringValue = (string)value;
		}
		else if (value is bool) {
			boolValue = (bool)value;
		}
		else if (value is Vector2) {
			vector2Value = (Vector2)value;
		}
		else if (value is PureDataVolumeRolloffModes) {
			volumeRolloffModeValue = (PureDataVolumeRolloffModes)value;
		}
		else if (value is AudioClip) {
			clipValue = (AudioClip)value;
		}
	}
	
	public void SetDefaultValue() {
		floatValue = 0;
		stringValue = "";
		boolValue = false;
		vector2Value = type == OptionTypes.PlayRange ? new Vector2(0, 1) : Vector2.zero;
		volumeRolloffModeValue = PureDataVolumeRolloffModes.Logarithmic;
		clipValue = null;
		delay = 0;
	}
	
	public void Apply(PureDataSource source) {
		switch (type) {
			case PureDataOption.OptionTypes.Volume:
				Vector2 volume = GetValue<Vector2>();
				source.SetVolume(volume.x, volume.y, delay);
				break;
			case PureDataOption.OptionTypes.Pitch:
				Vector2 pitch = GetValue<Vector2>();
				source.SetPitch(pitch.x, pitch.y, delay);
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
				source.SetClip(source.pureData.clipManager.GetClip(GetValue<AudioClip>().name));
				break;
			case PureDataOption.OptionTypes.Output:
				source.SetOutput(GetValue<string>(), delay);
				break;
			case PureDataOption.OptionTypes.PlayRange:
				Vector2 playRange = GetValue<Vector2>();
				source.SetPlayRange(playRange.x, playRange.y, delay, true);
				break;
			case PureDataOption.OptionTypes.Time:
				source.SetPhase(GetValue<float>(), delay);
				break;
			case PureDataOption.OptionTypes.DopplerLevel:
				source.spatializer.DopplerLevel = GetValue<float>();
				break;
			case PureDataOption.OptionTypes.VolumeRolloffMode:
				source.spatializer.VolumeRolloffMode = GetValue<PureDataVolumeRolloffModes>();
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
				Vector2 volume = GetValue<Vector2>();
				sequence.SetVolume(volume.x, volume.y, delay);
				break;
			case PureDataOption.OptionTypes.Loop:
				sequence.SetLoop(GetValue<bool>());
				break;
			case PureDataOption.OptionTypes.Output:
				sequence.SetOutput(GetValue<string>(), delay);
				break;
			case PureDataOption.OptionTypes.VolumeRolloffMode:
				sequence.spatializer.VolumeRolloffMode = GetValue<PureDataVolumeRolloffModes>();
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
			default:
				Logger.LogError(string.Format("{0} can not be applied to {1}.", this, sequence.GetType()));
				break;
		}
	}
	
	public override string ToString() {
		return string.Format("PureDataOption({0}, {1})", type, GetValueDisplayName());
	}
}
