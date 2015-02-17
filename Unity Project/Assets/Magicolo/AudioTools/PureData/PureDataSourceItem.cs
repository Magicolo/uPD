using UnityEngine;
using System.Collections;
using Magicolo.AudioTools;

[System.Serializable]
public abstract class PureDataSourceItem : PureDataSourceOrContainerItem {
		
	public abstract string Path {
		get;
	}
	
	public abstract int Samples {
		get;
	}
	
	public abstract int Frequency {
		get;
	}
	
	public abstract int Channels {
		get;
	}
	
	public abstract float Length {
		get;
	}
	
	public abstract bool LoadOnAwake {
		get;
	}
	
	public abstract string Output {
		get;
	}
	
	public abstract bool Fixed {
		get;
	}
	
	public abstract Vector2 PlayRange {
		get;
	}
	
	public abstract float Volume {
		get;
	}
	
	public abstract float Pitch {
		get;
	}
	
	public abstract float RandomVolume {
		get;
	}
	
	public abstract float RandomPitch {
		get;
	}
	
	public abstract float FadeIn {
		get;
	}
	
	public abstract float FadeOut {
		get;
	}
	
	public abstract bool Loop {
		get;
	}
			
	public abstract object Source {
		get;
	}
		
	public abstract float DopplerLevel {
		get;
	}
	
	public abstract PureDataVolumeRolloffModes VolumeRolloffMode {
		get;
	}
	
	public abstract float MinDistance {
		get;
	}
	
	public abstract float MaxDistance {
		get;
	}
	
	public abstract float PanLevel {
		get;
	}
	
	protected PureDataSourceItem(PureData pureData)
		: base(pureData) {
	}

}
