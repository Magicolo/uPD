using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.AudioTools;

[System.Serializable]
public abstract class PureDataSequenceItem : PureDataItem {

	public abstract string Output {
		get;
	}
	
	public abstract bool Loop {
		get;
	}
	
	public abstract float Volume {
		get;
	}
	
	public abstract object Source {
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
	
	public abstract int CurrentStepIndex {
		get;
	}
	
	protected PureDataSequenceItem(PureData pureData)
		: base(pureData) {
	}
	
	public abstract float GetStepTempo(int stepIndex);
	
	public abstract int GetStepBeats(int stepIndex);
}