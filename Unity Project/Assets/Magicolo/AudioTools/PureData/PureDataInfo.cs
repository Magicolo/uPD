using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataInfo : INamable {

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
		
		public string path;
		public int samples;
		public int frequency;
		public int channels;
		public float length;
		public float adjustedLength {
			get {
				return (length - length * playRangeStart - length * (1 - playRangeEnd)) / Mathf.Abs(pitch);
			}
		}
		
		public bool loadOnAwake;
		public string output = "Master";
		public bool isFixed = true;
		public float playRangeStart;
		public float adjustedPlayRangeStart {
			get {
				return pitch >= 0 ? playRangeStart / Mathf.Abs(pitch) : playRangeEnd / Mathf.Abs(pitch);
			}
		}
		public float playRangeEnd = 1;
		public float adjustedPlayRangeEnd {
			get {
				return pitch >= 0 ? playRangeEnd / Mathf.Abs(pitch) : playRangeStart / Mathf.Abs(pitch);
			}
		}
		[Min] public float volume = 1;
		public float pitch = 1;
		[Range(0, 1)] public float randomVolume;
		[Range(0, 1)] public float randomPitch;
		public float fadeIn;
		public float fadeOut = 0.01F;
		public bool loop;
		[Range(0, 10)] public float dopplerLevel = 1;
		public PureDataVolumeRolloffModes volumeRolloffMode;
		[Min] public float minDistance = 1;
		[Min] public float maxDistance = 500;
		[Range(0, 1)] public float panLevel = 0.75F;
		
		public PureData pureData;

		public PureDataInfo(AudioClip clip, PureData pureData) {
			this.Name = clip.name;
			this.path = HelperFunctions.GetResourcesPath(clip);
			this.samples = clip.samples;
			this.frequency = clip.frequency;
			this.channels = clip.channels;
			this.length = clip.length;
			this.pureData = pureData;
		}

		public PureDataInfo(PureDataInfo info, PureData pureData) {
			this.name = info.name;
			this.path = info.path;
			this.samples = info.samples;
			this.frequency = info.frequency;
			this.channels = info.channels;
			this.length = info.length;
			this.loadOnAwake = info.loadOnAwake;
			this.output = info.output;
			this.isFixed = info.isFixed;
			this.playRangeStart = info.playRangeStart;
			this.playRangeEnd = info.playRangeEnd;
			this.volume = info.volume;
			this.pitch = info.pitch;
			this.randomVolume = info.randomVolume;
			this.randomPitch = info.randomPitch;
			this.fadeIn = info.fadeIn;
			this.fadeOut = info.fadeOut;
			this.loop = info.loop;
			this.dopplerLevel = info.dopplerLevel;
			this.volumeRolloffMode = info.volumeRolloffMode;
			this.minDistance = info.minDistance;
			this.maxDistance = info.maxDistance;
			this.panLevel = info.panLevel;
			this.pureData = pureData;
		}

		public void Update(AudioClip clip) {
			Name = clip.name;
			path = HelperFunctions.GetResourcesPath(clip);
			samples = clip.samples;
			frequency = clip.frequency;
			channels = clip.channels;
			length = clip.length;
		}
	}
}
