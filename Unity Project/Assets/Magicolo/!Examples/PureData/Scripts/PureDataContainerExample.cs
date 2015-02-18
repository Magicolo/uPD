using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public class PureDataContainerExample : MonoBehaviour {

		public enum ContainerStates {
			One,
			Two,
			Three,
			Four
		}
	
		ContainerStates state;
		public ContainerStates State {
			get {
				return state;
			}
		}
	
		PureDataContainerItem containerItem;
		Vector2 scroll;
		
		void OnGUI() {
			GUILayout.Label("Current Item: " + (containerItem == null ? "None" : containerItem.ToString()));
		
			GUILayout.Space(16);
		
			scroll = GUILayout.BeginScrollView(scroll, GUILayout.Width(Screen.width - 50));
		
			GUILayout.Label("Plays a container that plays multiple sounds simultaneously.");
			if (GUILayout.Button("Play Mix")) {
				containerItem = PureData.PlayContainer("Mix");
			}
		
			GUILayout.Space(8);
			
			GUILayout.Label("Plays a container that plays a random sound from a collection.");
			if (GUILayout.Button("Play Random")) {
				containerItem = PureData.PlayContainer("Random");
			}
		
			GUILayout.Space(8);
					
			GUILayout.Label("Plays a container that plays a sound from a collection based on the state of an enum field.");
			GUILayout.Label("State: " + state);
			state = (ContainerStates)(int)GUILayout.HorizontalSlider((int)state, 0, 3).Round();
			if (GUILayout.Button("Play Switch")) {
				containerItem = PureData.PlayContainer("Switch");
			}
		
			GUILayout.Space(8);
		
			if (containerItem != null) {
				GUILayout.Label("Stops the last played sound if it is still playing with it's fade out.");
				if (GUILayout.Button("Stop")) {
					containerItem.Stop();
					containerItem = null;
				}
		
				GUILayout.Space(8);
		
				GUILayout.Label("Stops the last played sound if it is still playing without fade out.");
				if (GUILayout.Button("Stop Immediatly")) {
					containerItem.StopImmediate();
					containerItem = null;
				}
			
				GUILayout.Space(8);
		
				GUILayout.Label("Stops all sounds with fade out.");
				if (GUILayout.Button("Stop All")) {
					PureData.StopAll();
					containerItem = null;
				}
			}
		
			GUILayout.EndScrollView();
		}
	}
}