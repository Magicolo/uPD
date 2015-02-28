using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSequencePattern : INamable {
		
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

		public PureDataPatternSendTypes sendType;
		[Range(1, 32)] public int sendSize = 1;
		[Range(1, 32)] public int subdivision = 4;
		public float[] pattern = new float[4];
		public float[] sortedPattern;
		public PureData pureData;
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Start() {
			SortPattern();
		}
		
		public float[] GetPattern() {
			if (sortedPattern == null) {
				SortPattern();
			}
			
			return sortedPattern;
		}

		public void SetPattern(int sendSize, int subdivision, float[] pattern) {
			this.sendSize = sendSize;
			this.subdivision = subdivision;
			this.pattern = pattern;
			
			SortPattern();
		}
		
		public void SortPattern() {
			sortedPattern = new float[pattern.Length];
			
			for (int i = 0; i < subdivision; i++) {
				for (int j = 0; j < sendSize; j++) {
					sortedPattern[i * sendSize + j] = pattern[j * subdivision + i];
				}
			}
		}
		
		public void UpdatePatterns() {
			System.Array.Resize(ref pattern, subdivision * sendSize);
			
			SortPattern();
		}
		
		public void ResetPatterns() {
			pattern = new float[0];
			
			UpdatePatterns();
		}
	}
}

