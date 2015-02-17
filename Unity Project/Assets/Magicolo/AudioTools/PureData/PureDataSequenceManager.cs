using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSequenceManager : PureDataScriptableIdManager<PureDataSequence> {

		public string containerPath;
		public PureDataSequence[] sequences = new PureDataSequence[0];
		public PureData pureData;
		
		Dictionary<string, PureDataSequence> sequenceDict;
		List<PureDataSequence> activeSequences;
		List<PureDataSequence> inactiveSequences;
		List<PureDataSequence> sequencesToDeactivate;
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			foreach (PureDataSequence sequence in sequences) {
				sequence.Initialize(pureData);
			}
		}

		public void Start() {
			activeSequences = new List<PureDataSequence>();
			inactiveSequences = new List<PureDataSequence>();
			sequencesToDeactivate = new List<PureDataSequence>();
			
			ResetIds();
			BuildSequenceDict();
			
			pureData.communicator.Receive("usequence_play", PlaySequenceReceiver, true);
			pureData.communicator.Receive("usequence_stop", StopSequenceReceiver, true);
			pureData.communicator.Receive("usequence_step", StepSequenceReceiver, true);
			
			foreach (PureDataSequence sequence in sequences) {
				sequence.Start();
			}
		}
		
		public void Update() {
			for (int i = sequencesToDeactivate.Count - 1; i >= 0; i--) {
				PureDataSequence sequence = sequencesToDeactivate.PopLast();
				activeSequences.Remove(sequence);
				inactiveSequences.Add(sequence);
			}
			
			for (int i = activeSequences.Count - 1; i >= 0; i--) {
				activeSequences[i].Update();
			}
		}
		
		public void UpdateSequenceContainer() {
			#if !UNITY_WEBPLAYER
			if (!SetContainerPath()) {
				return;
			}
			
			ResetIds();
			
			ThreadPool.QueueUserWorkItem(new WaitCallback(WriteToSequenceContainer));
			#endif
		}
		
		public void WriteToSequenceContainer(object state) {
			#if !UNITY_WEBPLAYER
			List<string> text = new List<string>();
			text.Add("#N canvas 560 298 464 178 10;");
			
			foreach (PureDataSequence sequence in sequences) {
				if (sequence.HasValidTracks) {
					text.AddRange(CreateSequenceText(sequence));
				}
			}
			
			File.WriteAllLines(containerPath, text.ToArray());
			#endif
		}
		
		public List<string> CreateSequenceText(PureDataSequence sequence) {
			List<string> sequenceText = new List<string>();
			
			sequenceText.Add("#N canvas 740 649 450 203 usequence 0;");
			sequenceText.Add("#X obj 21 41 switch~;");
			sequenceText.Add(string.Format("#X obj 21 20 r usequence_switch{0};", sequence.Id));
			sequenceText.Add(string.Format("#X obj 21 140 usequencespatializer {0};", sequence.Id));
			sequenceText.Add(string.Format("#X obj 21 120 usequencestopper {0};", sequence.Id));
			sequenceText.Add(string.Format("#X obj 21 79 usequencestepper {0};", sequence.Id));
			sequenceText.Add(string.Format("#X obj 21 160 usequenceoutput {0};", sequence.Id));
			
			for (int i = 0; i < sequence.tracks.Length; i++) {
				if (sequence.tracks[i].IsValid) {
					sequenceText.AddRange(CreateTrackText(sequence, sequence.tracks[i]));
					sequenceText.Add(string.Format("#X connect {0} 0 2 0;", 6 + i));
					sequenceText.Add(string.Format("#X connect {0} 1 2 1;", 6 + i));
					sequenceText.Add(string.Format("#X connect {0} 0 3 0;", 6 + i));
					sequenceText.Add(string.Format("#X connect {0} 1 3 0;", 6 + i));
				}
			}
			
			sequenceText.Add("#X connect 1 0 0 0;");
			sequenceText.Add("#X connect 2 0 5 0;");
			sequenceText.Add("#X connect 2 1 5 1;");
			sequenceText.Add("#X connect 6 0 3 0;");
			sequenceText.Add("#X connect 6 0 2 0;");
			sequenceText.Add("#X connect 6 1 3 0;");
			sequenceText.Add("#X connect 6 1 2 1;");
			sequenceText.Add(string.Format("#X restore 22 19 pd usequence {0};", sequence.Name));
			
			return sequenceText;
		}
		
		public List<string> CreateTrackText(PureDataSequence sequence, PureDataSequenceTrack track) {
			List<string> trackText = new List<string>();
			
			trackText.Add("#N canvas 43 653 311 113 utrack 0;");
			trackText.Add("#X obj 23 65 outlet~ left;");
			trackText.Add("#X obj 103 65 outlet~ right;");
			trackText.Add(string.Format("#X obj 23 44 ../../{0};", HelperFunctions.GetPathWithoutExtension(track.instrumentPatchPath)));
			trackText.Add(string.Format("#X obj 23 24 usequencemessager {0} {1};", sequence.Id, track.Id));
			trackText.Add("#X connect 2 0 0 0;");
			trackText.Add("#X connect 2 1 1 0;");
			trackText.Add("#X connect 3 0 2 0;");
			trackText.Add("#X connect 3 1 2 1;");
			trackText.Add(string.Format("#X restore 21 99 pd utrack {0};", track.Name));
			
			return trackText;
		}
			
		public void Activate(PureDataSequence sequence) {
			if (!activeSequences.Contains(sequence)) {
				inactiveSequences.Remove(sequence);
				activeSequences.Add(sequence);
			}
		}
					
		public void Deactivate(PureDataSequence sequence) {
			sequencesToDeactivate.Add(sequence);
		}

		public void StopAll(float delay) {
			activeSequences.ForEach(sequence => sequence.Stop(delay));
		}
		
		public void StopAllImmediate() {
			activeSequences.ForEach(sequence => sequence.StopImmediate());
		}
		
		public bool SetContainerPath() {
			#if !UNITY_WEBPLAYER
			if (string.IsNullOrEmpty(containerPath) || !File.Exists(containerPath) || !HelperFunctions.PathIsRelativeTo(containerPath, Application.streamingAssetsPath)) {
				containerPath = Path.GetFullPath(HelperFunctions.GetAssetPath("usequencecontainer.pd"));
			}
			
			if (!File.Exists(containerPath)) {
				Logger.LogError("Can not find usequencecontainer.pd patch.");
				return false;
			}
			#endif
			
			return true;
		}
		
		public void BuildSequenceDict() {
			sequenceDict = new Dictionary<string, PureDataSequence>();
			
			foreach (PureDataSequence sequence in sequences) {
				sequenceDict[sequence.Name] = sequence;
			}
		}
		
		public PureDataSequence GetSequence(string sequenceName) {
			PureDataSequence sequence = null;
			
			try {
				sequence = sequenceDict[sequenceName];
			}
			catch {
				Logger.LogError(string.Format("Sequence named {0} was not found.", sequenceName));
			}
			
			return sequence;
		}
		
		public PureDataSequence GetSequence(string sequenceName, object source) {
			PureDataSequence sequence = GetSequence(sequenceName);
			sequence.State = PureDataStates.Waiting;
			sequence.SetNextSource(source);
			return sequence;
		}

		public void ResetIds() {
			ResetUniqueIds(sequences);
			
			foreach (PureDataSequence sequence in sequences) {
				sequence.ResetUniqueIds(sequence.tracks);
			}
		}
		
		public void PlaySequenceReceiver(float sequenceId) {
			GetIdentifiableWithId((int)sequenceId).Play();
		}
		
		public void StopSequenceReceiver(float sequenceId) {
			GetIdentifiableWithId((int)sequenceId).StopImmediate();
		}
		
		public void StepSequenceReceiver(float sequenceId) {
			GetIdentifiableWithId((int)sequenceId).Step();
		}
		
		public static void Switch(PureDataSequenceManager source, PureDataSequenceManager target) {
			source.sequences = target.sequences;
			
			source.Initialize(source.pureData);
		}
		
		public static PureDataSequenceManager Create(string path) {
			return HelperFunctions.GetOrAddAssetOfType<PureDataSequenceManager>("Sequences", path);
		}
	}
}