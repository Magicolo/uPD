using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataListener {
	
		public Transform transform;
		public Vector3 position;
		public Vector3 right;
		public PureData pureData;
		
		AudioListener listener;
		PureDataFilterRead filterRead;
		
		public PureDataListener(PureData pureData) {
			this.pureData = pureData;
			
			Initialize(pureData);
		}
		
		public void Initialize(PureData pureData) {
			listener = Object.FindObjectOfType<AudioListener>();
		
			if (listener == null) {
				GameObject newListener = new GameObject("Listener");
				listener = newListener.AddComponent<AudioListener>();
				listener.transform.Reset();
				Logger.LogWarning("No listener was found in the scene. One was automatically created.");
			}
		
			// HACK Trick to activate OnAudioFilterRead
			listener.enabled = false;
			filterRead = listener.GetOrAddComponent<PureDataFilterRead>();
			filterRead.Initialize(pureData);
			listener.enabled = true;
			transform = listener.transform;
		}
		
		public void Update() {
			if (transform != null) {
				position = transform.position;
				right = transform.right;
			}
		}
	}
}