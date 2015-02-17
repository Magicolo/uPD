using UnityEngine;
using System.Collections;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	public class PureDataSetup : MonoBehaviour {

		[SerializeField]
		PureDataInfo info;
		public PureDataInfo Info {
			get {
				if (info == null) {
					info = pureData.infoManager.GetInfo(name);
				}
				return info;
			}
			set {
				info = value;
			}
		}

		public AudioClip Clip {
			get {
				return Info == null ? null : Resources.Load<AudioClip>(Info.path);
			}
		}
		
		public PureData pureData;
		
		public void UpdateSetup() {
			if (Clip == null) {
				gameObject.Remove();
				return;
			}
		}

		public void UpdateInfo() {
			pureData.infoManager.SetInfo(Info.Name, info);
		}
		
		public void FreezeTransform() {
			transform.hideFlags = HideFlags.HideInInspector;
			transform.Reset();
		}
	}
}
