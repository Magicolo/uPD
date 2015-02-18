using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSource : INamable, IIdentifiable {

		public string Name {
			get {
				return Info.Name;
			}
			set {
				Info.Name = value;
			}
		}
		
		int id;
		public int Id {
			get {
				return id;
			}
			set {
				id = value;
				spatializer.UpdateSendNames();
			}
		}
		
		PureDataStates state;
		public PureDataStates State {
			get {
				return state;
			}
			set {
				state = value;
			}
		}
		
		PureDataClip clip;
		public PureDataClip Clip {
			get {
				return clip;
			}
			set {
				clip = value;
				pureData.communicator.Send("uaudiosource_clipsamples" + Id, clip.Samples);
				pureData.communicator.Send("uaudiosource_clipname" + Id, clip.Name);
			}
		}

		[System.NonSerialized] PureDataInfo info;
		public PureDataInfo Info {
			get {
				return info;
			}
			set {
				info = value;
			}
		}

		#region Internal settings
		public float adjustedLength {
			get {
				return Info.length / Mathf.Abs(Info.pitch);
			}
		}
		public float adjustedClippedLength {
			get {
				return (Info.playRangeEnd - Info.playRangeStart) * adjustedLength;
			}
		}
		public float adjustedThreshold {
			get {
				if (Info.pitch >= 0) {
					if (Info.isFixed) {
						return (adjustedClippedLength - adjustedFadeOut) / adjustedClippedLength;
					}
				
					return (adjustedClippedLength - adjustedFadeOut) / adjustedLength + Info.playRangeStart;
				}
				
				if (Info.isFixed) {
					return (adjustedClippedLength - adjustedFadeOut) / adjustedClippedLength - 1;
				}
				
				return (adjustedClippedLength - adjustedFadeOut) / adjustedLength - Info.playRangeEnd;

			}
		}
		public float adjustedFadeOut {
			get {
				if (Info.isFixed) {
					return Mathf.Clamp(Info.fadeOut, thresholdEpsilon * adjustedClippedLength, adjustedClippedLength - (thresholdEpsilon * adjustedClippedLength));
				}
				
				return Mathf.Clamp(Info.fadeOut, thresholdEpsilon * adjustedLength, adjustedClippedLength - (thresholdEpsilon * adjustedLength));
			}
		}
		public float adjustedPhaseStart {
			get {
				if (Info.pitch >= 0) {
					return Info.isFixed ? 0 : Info.playRangeStart;
				}
				
				return Info.isFixed ? 1 - thresholdEpsilon : Info.playRangeEnd;
			}
		}
		public float readSpeed;
		public float threshold;
		public const float thresholdEpsilon = 0.001F;
		public float phase;
		public bool sourceSwitch;
		public bool memoryReaderSwitch;
		public bool stopperSwitch;
		public bool monoSwitch;
		public bool stereoSwitch;
		#endregion
		
		public PureDataSourceSpatializer spatializer;
		public PureData pureData;
		
		public PureDataSource(PureData pureData) {
			this.pureData = pureData;
			
			spatializer = new PureDataSourceSpatializer(this, pureData);
		}

		public void Update() {
			spatializer.Update();
		}
		
		public void Play(float delay = 0) {
			delay = Mathf.Max(delay, 0);
			
			if (State == PureDataStates.Waiting) {
				State = PureDataStates.Playing;
				
				pureData.sourceManager.Activate(this);
				
				SwitchOn(delay);
				FadeIn(delay);
			}
			else if (State == PureDataStates.Paused) {
				State = PureDataStates.Playing;
				
				SetPaused(false, 0.01F, delay);
			}
		}

		public void Pause(float delay = 0) {
			delay = Mathf.Max(delay, 0);
			
			if (State == PureDataStates.Playing) {
				State = PureDataStates.Paused;
				
				SetPaused(true, 0.01F, delay);
			}
		}
		
		public void Stop(float delay = 0) {
			delay = Mathf.Max(delay, 0);
			
			if ((State == PureDataStates.Playing || State == PureDataStates.Paused) && State != PureDataStates.Stopping && State != PureDataStates.Stopped) {
				State = PureDataStates.Stopping;
				
				FadeOut(delay);
				SetStopperSwitch(false, delay);
			}
		}
		
		public void StopImmediate() {
			if (State != PureDataStates.Stopped) {
				State = PureDataStates.Stopped;
				
				SwitchOff();
				SetSourceSwitch(false);
				pureData.sourceManager.Deactivate(this);
			}
		}

		public void FadeIn(float delay = 0) {
			pureData.communicator.SendDelayedMessage("uaudiosource_fade" + Id, delay, 0, 0);
			pureData.communicator.SendDelayedMessage("uaudiosource_fade" + Id, delay, 1, Mathf.Max(Info.fadeIn * 1000, 10));
		}
		
		public void FadeOut(float delay = 0) {
			pureData.communicator.SendDelayedMessage("uaudiosource_fade" + Id, delay, 0, Mathf.Max(adjustedFadeOut * 1000, 10));
			pureData.communicator.SendDelayedMessage("uaudiosource_stopdelay" + Id, delay, Mathf.Max(adjustedFadeOut * 1000, 10));
		}
		
		public void SwitchOn(float delay = 0) {
			SetMemoryReaderSwitch(true, delay);
			
			if (Clip.Channels == 1) {
				SetMonoSwitch(true, delay);
				SetStereoSwitch(false, delay);
			}
			else {
				SetMonoSwitch(false, delay);
				SetStereoSwitch(true, delay);
			}
		}
		
		public void SwitchOff(float delay = 0) {
			SetMemoryReaderSwitch(false, delay);
			SetStopperSwitch(false, delay);
			SetMonoSwitch(false, delay);
			SetStereoSwitch(false, delay);
		}

		public void SetClip(PureDataClip clip) {
			Clip = clip;
			Load();
			SwitchOff();
			SetSourceSwitch(true);
			
			Info = new PureDataInfo(pureData.infoManager.GetInfo(clip.Name), pureData);
			State = PureDataStates.Waiting;
			SetPaused(false);
			SetOutput(Info.output);
			SetVolume(Info.volume + Info.volume * HelperFunctions.RandomRange(-Info.randomVolume, Info.randomVolume));
			SetPitch(Info.pitch + Info.pitch * HelperFunctions.RandomRange(-Info.randomPitch, Info.randomPitch));
			SetPlayRange(Info.playRangeStart, Info.playRangeEnd);
			SetFadeIn(Info.fadeIn);
			SetFadeOut(Info.fadeOut);
			SetLoop(Info.loop);
		}
		
		public void SetSource(object targetSource) {
			spatializer.Source = targetSource;
		}
		
		public void SetPaused(bool paused, float time = 0, float delay = 0) {
			int pauseState = (!paused).GetHashCode();
			
			if (paused) {
				pureData.communicator.SendDelayedMessage("uaudiosource_pause" + Id, delay + time, pauseState);
			}
			else {
				pureData.communicator.SendDelayedMessage("uaudiosource_pause" + Id, 0, pauseState);
			}
			
			pureData.communicator.SendDelayedMessage("uaudiosource_pausevolume" + Id, delay, pauseState, time * 1000);
			
		}
		
		public void SetOutput(string targetOutput, float delay = 0) {
			Info.output = targetOutput;
			
			pureData.communicator.SendDelayedMessage("uaudiosource_output" + Id, delay, Info.output);
		}
		
		public void SetPlayRange(float start, float end, float delay = 0, bool checkFixed = false) {
			if (checkFixed && Info.isFixed) {
				Logger.LogError("Can not set the play range of a fixed clip.");
				return;
			}
			
			start = Mathf.Clamp(start, 0, Mathf.Min(end, 1));
			end = Mathf.Clamp(end, start, 1);
			Info.playRangeStart = start;
			Info.playRangeEnd = end;
			
			SetPhase(adjustedPhaseStart, delay);
			SetThreshold(adjustedThreshold, 0, delay);
		}
		
		public void SetVolume(float targetVolume, float time = 0, float delay = 0) {
			Info.volume = Mathf.Max(targetVolume, 0);
			time = Mathf.Max(time, 0);
			
			pureData.communicator.SendDelayedMessage("uaudiosource_volume" + Id, delay, Info.volume, time * 1000);
		}
		
		public void SetPitch(float targetPitch, float time = 0, float delay = 0) {
			Info.pitch = targetPitch;
			time = Mathf.Max(time, 0);
			
			SetReadSpeed(((float)Clip.Frequency / (float)Clip.Samples) * Info.pitch, time, delay);
			SetThreshold(adjustedThreshold, time, delay);
		}
		
		public void SetReadSpeed(float targetReadSpeed, float time = 0, float delay = 0) {
			readSpeed = targetReadSpeed;
			
			pureData.communicator.SendDelayedMessage("uaudiosource_readspeed" + Id, delay, readSpeed, time * 1000);
		}
		
		public void SetThreshold(float targetThreshold, float time = 0, float delay = 0) {
			threshold = targetThreshold;

			pureData.communicator.SendDelayedMessage("uaudiosource_threshold" + Id, delay, threshold, time * 1000);
		}
		
		public void SetPhase(float targetPhase, float delay = 0) {
			phase = Mathf.Clamp01(targetPhase);
			
			pureData.communicator.SendDelayedMessage("uaudiosource_phase" + Id, delay, phase);
		}
		
		public void SetFadeIn(float targetFadeIn) {
			Info.fadeIn = Mathf.Clamp(targetFadeIn, 0, adjustedLength);
		}
		
		public void SetFadeOut(float targetFadeOut) {
			Info.fadeOut = targetFadeOut;
			
			SetThreshold(adjustedThreshold);
		}
		
		public void SetLoop(bool targetLoop, float delay = 0) {
			Info.loop = targetLoop;
			
			SetStopperSwitch(!Info.loop, delay);
		}
		
		public void SetSourceSwitch(bool targetSwitch, float delay = 0) {
			sourceSwitch = targetSwitch;
			
			pureData.communicator.SendDelayedMessage("uaudiosource_switch" + Id, delay, sourceSwitch);
		}
		
		public void SetMemoryReaderSwitch(bool targetSwitch, float delay = 0) {
			memoryReaderSwitch = targetSwitch;
			
			pureData.communicator.SendDelayedMessage("uaudiosource_memoryreaderswitch" + Id, delay, memoryReaderSwitch);
		}
		
		public void SetStopperSwitch(bool targetSwitch, float delay = 0) {
			stopperSwitch = targetSwitch;
			
			pureData.communicator.SendDelayedMessage("uaudiosource_stopperswitch" + Id, delay, stopperSwitch);
		}
		
		public void SetMonoSwitch(bool targetSwitch, float delay = 0) {
			monoSwitch = targetSwitch;
			
			pureData.communicator.SendDelayedMessage("uaudiosource_monoswitch" + Id, delay, monoSwitch);
		}
		
		public void SetStereoSwitch(bool targetSwitch, float delay = 0) {
			stereoSwitch = targetSwitch;
			
			pureData.communicator.SendDelayedMessage("uaudiosource_stereoswitch" + Id, delay, stereoSwitch);
		}
		
		public void Load() {
			Clip.Load();
		}
		
		public void Unload() {
			Clip.Unload();
		}
		
		public void ApplyOptions(params PureDataOption[] options) {
			foreach (PureDataOption option in options) {
				option.Apply(this);
			}
		}
	}
}
