using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[AddComponentMenu("Magicolo/Pure Data/Play")]
	public class PureDataPlay : MonoBehaviour {
								
		public void Play(string soundName) {
			PureData.Play(soundName);
		}
									
		public void PlayContainer(string containerName) {
			PureData.PlayContainer(containerName);
		}
	}
}