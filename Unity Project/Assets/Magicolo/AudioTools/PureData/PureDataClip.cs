using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataClip  : INamable, IIdentifiable {
		
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
			}
		}
		
		[SerializeField]
		string path;
		public string Path {
			get {
				return path;
			}
		}
		
		[SerializeField]
		int samples;
		public int Samples {
			get {
				return IsFixed ? (int)(samples * (PlayRangeEnd - PlayRangeStart)) : samples;
			}
		}
		
		[SerializeField]
		int frequency;
		public int Frequency {
			get {
				return frequency;
			}
		}
		
		[SerializeField]
		int channels;
		public int Channels {
			get {
				return channels;
			}
		}
		
		[SerializeField]
		float length;
		public float Length {
			get {
				return IsFixed ? length * (PlayRangeEnd - PlayRangeStart) : length;
			}
		}

		bool isLoaded;
		public bool IsLoaded {
			get {
				return isLoaded;
			}
		}

		PureDataInfo info;
		public PureDataInfo Info {
			get {
				if (info == null || pureData.generalSettings.ApplicationIsEditor) {
					info = pureData.infoManager.GetInfo(Name);
				}
				
				return info;
			}
		}
			
		bool isFixed;
		public bool IsFixed {
			get {
				return Info.isFixed;
			}
		}
		
		float playRangeStart;
		public float PlayRangeStart {
			get {
				return Info.playRangeStart;
			}
		}
		
		float playRangeEnd;
		public float PlayRangeEnd {
			get {
				return Info.playRangeEnd;
			}
		}
		
		public PureData pureData;

		public PureDataClip(PureDataInfo info, PureData pureData) {
			this.name = info.Name;
			this.path = info.path;
			this.samples = info.samples;
			this.frequency = info.frequency;
			this.channels = info.channels;
			this.length = info.length;
			this.pureData = pureData;
		}

		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Load() {
			isLoaded = IsFixed == isFixed && PlayRangeStart == playRangeStart && PlayRangeEnd == playRangeEnd;
			
			if (!isLoaded) {
				AudioClip clip = Resources.Load<AudioClip>(Path);
				
				if (clip == null) {
					return;
				}
				
				float[] dataLeft;
				float[] dataRight;
				
				if (IsFixed) {
					clip.GetUntangledData(out dataLeft, out dataRight, (int)(PlayRangeStart * clip.samples), (int)((PlayRangeEnd - PlayRangeStart) * clip.samples * clip.channels));
				}
				else {
					clip.GetUntangledData(out dataLeft, out dataRight);
				}
				
				pureData.communicator.WriteArray("uaudioclip_left" + Name, dataLeft);
				pureData.communicator.WriteArray("uaudioclip_right" + Name, dataRight);
				
				Resources.UnloadAsset(clip);
				isLoaded = true;
				isFixed = IsFixed;
				playRangeStart = PlayRangeStart;
				playRangeEnd = PlayRangeEnd;
			}
		}
		
		public void Unload() {
			if (isLoaded) {
				pureData.communicator.ResizeArray("uaudioclip_left" + Name, 0);
				pureData.communicator.ResizeArray("uaudioclip_right" + Name, 0);
				isLoaded = false;
			}
		}
	}
}
