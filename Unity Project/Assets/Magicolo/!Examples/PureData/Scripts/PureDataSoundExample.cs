using UnityEngine;
using System.Collections;

namespace Magicolo {
	public class PureDataSoundExample : MonoBehaviour {

		PureDataSourceItem sourceItem;
		Vector2 scroll;
		
		void Update() {
			transform.OscillatePosition(1, 25, 0, "X");
		}
	
		void OnGUI() {
			GUILayout.Label("Current Item: " + (sourceItem == null ? "None" : sourceItem.ToString()));
		
			GUILayout.Space(16);
		
			scroll = GUILayout.BeginScrollView(scroll, GUILayout.Width(Screen.width - 50));
		
			GUILayout.Label("Plays the looping sound named 'Synth_Up' spatialized around the listener.");
			if (GUILayout.Button("Play")) {
				sourceItem = PureData.Play("Synth_Up");
			}
		
			GUILayout.Space(8);
		
			GUILayout.Label("Plays the sound named 'Synth_Chaotic' spatialized around the example transform and changes it's pitch to 0.25.");
			if (GUILayout.Button("Play Long")) {
				sourceItem = PureData.Play("Synth_Chaotic", transform, PureDataOption.Pitch(0.25F));
			}
		
			if (sourceItem != null) {
				GUILayout.Space(8);
		
				GUILayout.Label("Ramps the volume of the last played sound to 0.01 in 2 seconds.");
				if (GUILayout.Button("Fade Down")) {
					sourceItem.ApplyOptions(PureDataOption.Volume(0.01F, 2));
				}
				
				GUILayout.Space(8);
		
				GUILayout.Label("Sets the volume of the last played sound to target.");
				GUILayout.Label("Volume: " + sourceItem.Volume);
				float volume = GUILayout.HorizontalSlider(sourceItem.Volume, 0, 0.5F);
				if (volume != sourceItem.Volume) {
					sourceItem.ApplyOptions(PureDataOption.Volume(volume, 0.01F));
				}
			
				GUILayout.Space(8);
		
				GUILayout.Label("Ramps the pitch of the last played sound to target after a 1 second delay.");
				GUILayout.Label("Pitch: " + sourceItem.Pitch);
				float pitch = GUILayout.HorizontalSlider(sourceItem.Pitch, 0, 5);
				if (pitch != sourceItem.Pitch) {
					sourceItem.ApplyOptions(PureDataOption.Pitch(pitch, 0.5F, 1));
				}
		
				GUILayout.Space(8);
		
				GUILayout.Label("Stops the last played sound if it is still playing with it's fade out.");
				if (GUILayout.Button("Stop")) {
					sourceItem.Stop();
					sourceItem = null;
				}
		
				GUILayout.Space(8);
		
				GUILayout.Label("Stops the last played sound if it is still playing without fade out.");
				if (GUILayout.Button("Stop Immediatly")) {
					sourceItem.StopImmediate();
					sourceItem = null;
				}
			
				GUILayout.Space(8);
		
				GUILayout.Label("Stops all sounds with fade out.");
				if (GUILayout.Button("Stop All")) {
					PureData.StopAll();
					sourceItem = null;
				}
			}
		
			GUILayout.EndScrollView();
		}
	}
}