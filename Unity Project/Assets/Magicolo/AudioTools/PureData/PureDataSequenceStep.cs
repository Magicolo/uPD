using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSequenceStep {

		[Min(1)] public float tempo = 120;
		[Range(1, 16)] public int beats = 4;
	}
}

