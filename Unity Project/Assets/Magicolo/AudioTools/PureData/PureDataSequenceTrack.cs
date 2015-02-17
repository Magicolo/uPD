using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSequenceTrack : INamable, IIdentifiable {

		public string Name {
			get {
				return string.IsNullOrEmpty(instrumentPatchPath) ? "default" : Path.GetFileNameWithoutExtension(instrumentPatchPath);
			}
			set {
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
		
		public bool IsValid {
			get {
				string assetInstrumentPatchPath = "Assets/StreamingAssets/" + pureData.generalSettings.patchesPath + Path.AltDirectorySeparatorChar + instrumentPatchPath;
				return !string.IsNullOrEmpty(instrumentPatchPath) && File.Exists(assetInstrumentPatchPath);
			}
		}
		
		public string instrumentPatchPath;
		public PureDataSequenceTrackStep[] steps = new PureDataSequenceTrackStep[4];
		public PureDataSequencePattern[] patterns = new PureDataSequencePattern[0];
		public PureData pureData;

		public PureDataSequenceTrack(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			foreach (PureDataSequencePattern pattern in patterns) {
				pattern.Initialize(pureData);
			}
		}

		public void Start() {
			foreach (PureDataSequencePattern pattern in patterns) {
				pattern.Start();
			}
		}
		
		public void Step(float tickSpeed, int stepIndex, PureDataSequence sequence) {
			PureDataSequenceTrackStep trackStep = steps[stepIndex];
			
			if (trackStep.patternIndex != -1) {
				PureDataSequencePattern pattern = patterns[trackStep.patternIndex];
			
				pureData.communicator.Send(string.Format("utrack_size{0}_{1}", sequence.Id, Id), pattern.sendSize);
				pureData.communicator.Send(string.Format("utrack_delay{0}_{1}", sequence.Id, Id), tickSpeed * 1000 / pattern.subdivision);
				pureData.communicator.Send(string.Format("utrack_pattern{0}_{1}", sequence.Id, Id), pattern.GetPattern());
			}
			else {
				pureData.communicator.SendBang(string.Format("utrack_pattern{0}_{1}", sequence.Id, Id));
			}
		}
		
		public void RemovePatternFromSteps(int index) {
			foreach (PureDataSequenceTrackStep step in steps) {
				if (step.patternIndex == index) {
					step.patternIndex = -1;
				}
			}
		}
	}
}

