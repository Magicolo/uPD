using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class AudioClipExtensions {

		public static AudioSource PlayOnListener(this AudioClip audioClip) {
			AudioListener listener = Object.FindObjectOfType<AudioListener>();
			if (listener == null) {
				Logger.LogError("No listener was found in the scene.");
				return null;
			}
		
			GameObject gameObject = new GameObject(audioClip.name);
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.clip = audioClip;
			gameObject.transform.parent = listener.transform;
			gameObject.transform.Reset();
			audioSource.Play();
		
			return audioSource;
		}
	
		public static AudioClip Add(this AudioClip audioClip, AudioClip otherAudioClip) {
			int length = audioClip.samples >= otherAudioClip.samples ? audioClip.samples : otherAudioClip.samples;
			AudioClip clipSum = AudioClip.Create(audioClip.name + " + " + otherAudioClip.name, length, audioClip.channels, audioClip.frequency, true, false);
		
			float[] dataSum;
			float[] otherData;
		
			if (audioClip.samples >= otherAudioClip.samples) {
				dataSum = new float[audioClip.samples];
				audioClip.GetData(dataSum, 0);
				otherData = new float[otherAudioClip.samples];
				otherAudioClip.GetData(otherData, 0);
			}
			else {
				dataSum = new float[otherAudioClip.samples];
				otherAudioClip.GetData(dataSum, 0);
				otherData = new float[audioClip.samples];
				audioClip.GetData(otherData, 0);
			}
		
			for (int i = 0; i < otherData.Length; i++) {
				dataSum[i] += otherData[i];
			}
		
			clipSum.SetData(dataSum, 0);
		
			return clipSum;
		}
	
		public static void GetUntangledData(this AudioClip audioClip, out float[] dataLeft, out float[] dataRight, int offsetSamples, int amountOfValues) {
			float[] data = new float[amountOfValues];
			audioClip.GetData(data, offsetSamples);

			if (audioClip.channels > 1) {
				dataLeft = new float[amountOfValues / 2];
				dataRight = new float[amountOfValues / 2];
				
				for (int i = 0, j = 0; i < amountOfValues - 1; i += 2, j += 1) {
					dataLeft[j] = data[i];
					dataRight[j] = data[i + 1];
				}
			}
			else {
				dataLeft = data;
				dataRight = new float[0];
			}
			
			data = null;
			System.GC.Collect();
		}
		
		public static void GetUntangledData(this AudioClip audioClip, out float[] dataLeft, out float[] dataRight) {
			audioClip.GetUntangledData(out dataLeft, out dataRight, 0, audioClip.samples * audioClip.channels);
		}
	}
}
