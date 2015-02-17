using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSpatializer : PureDataSpatializerBase {
		
		[SerializeField]
		PureDataVolumeRolloffModes volumeRolloffMode;
		public override PureDataVolumeRolloffModes VolumeRolloffMode {
			get {
				return volumeRolloffMode;
			}
			set {
				volumeRolloffMode = value;
				spatialize = true;
			}
		}

		[SerializeField]
		[Min] float minDistance = 1;
		public override float MinDistance {
			get {
				return minDistance;
			}
			set {
				minDistance = value;
				spatialize = true;
			}
		}

		[SerializeField]
		[Min] float maxDistance = 500;
		public override float MaxDistance {
			get {
				return maxDistance;
			}
			set {
				maxDistance = value;
				spatialize = true;
			}
		}

		[SerializeField]
		[Range(0, 1)] float panLevel = 0.75F;
		public override float PanLevel {
			get {
				return panLevel;
			}
			set {
				panLevel = value;
				spatialize = true;
			}
		}

		Transform source;
		public override object Source {
			get {
				if (source == null && sourceId != 0 || !pureData.generalSettings.ApplicationPlaying) {
					source = pureData.references.GetObjectWithId<Transform>(sourceId);
					sourceId = source == null ? 0 : sourceId;
				}
				return source;
			}
			set {
				source = value as Transform;
			}
		}

		string panLeftSendName;
		public override string PanLeftSendName {
			get {
				return panLeftSendName;
			}
		}

		string panRightSendName;
		public override string PanRightSendName {
			get {
				return panRightSendName;
			}
		}

		[SerializeField]
		int sourceId;
		public int SourceId {
			get {
				return sourceId;
			}
			set {
				sourceId = value;
			}
		}
		
		public PureDataSpatializer(PureData pureData)
			: base(pureData) {
		}

		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Start() {
			panLeftSendName = "uspatializer_pan_left" + Name;
			panRightSendName = "uspatializer_pan_right" + Name;
		}
		
		public void OnDrawGizmos() {
			#if UNITY_EDITOR
			if (Source == null){
				return;
			}
			
			Gizmos.DrawIcon((Source as Transform).position, "pd.png", true);
			
			if ((UnityEditor.Selection.gameObjects.Contains(pureData.gameObject) || UnityEditor.Selection.gameObjects.Contains(Source)) && Showing) {
				Gizmos.color = new Color(0.25F, 0.5F, 0.75F, 1);
				Gizmos.DrawWireSphere((Source as Transform).position, MinDistance);
				Gizmos.color = new Color(0.25F, 0.75F, 0.5F, 0.35F);
				Gizmos.DrawWireSphere((Source as Transform).position, MaxDistance);
			}
			#endif
		}
	}
}