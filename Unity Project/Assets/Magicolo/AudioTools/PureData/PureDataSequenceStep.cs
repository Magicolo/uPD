using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSequenceStep {

		[SerializeField, PropertyField(typeof(MinAttribute), 1)]
		float tempo;
		public float Tempo {
			get {
				return tempo;
			}
			set {
				tempo = Mathf.Max(value, 1);
			}
		}
		
		[SerializeField, PropertyField(typeof(RangeAttribute), 1, 16)]
		int beats;
		public int Beats {
			get {
				return beats;
			}
			set {
				beats = Mathf.Clamp(value, 1, 16);
			}
		}
	}
}

