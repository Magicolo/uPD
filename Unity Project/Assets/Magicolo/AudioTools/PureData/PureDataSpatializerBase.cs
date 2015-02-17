using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public abstract class PureDataSpatializerBase : INamable, IShowable {

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

		[SerializeField]
		bool showing;
		public bool Showing {
			get {
				return showing;
			}
			set {
				showing = value;
			}
		}
		
		public abstract object Source {
			get;
			set;
		}
		
		public abstract PureDataVolumeRolloffModes VolumeRolloffMode {
			get;
			set;
		}

		public abstract float MinDistance {
			get;
			set;
		}

		public abstract float MaxDistance {
			get;
			set;
		}

		public abstract float PanLevel {
			get;
			set;
		}
		
		public abstract string PanLeftSendName {
			get;
		}
		
		public abstract string PanRightSendName {
			get;
		}
		
		public PureData pureData;
		
		protected bool spatialize;
		protected Vector3 sourcePosition;
		protected float distance;
		protected float attenuation;
		protected bool panInitialized;
		protected bool skipped;
		
		protected PureDataSpatializerBase(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void Update() {
			if (SourceHasChanged()) {
				skipped = false;
				Spatialize();
			}
		}
		
		public virtual void Spatialize(bool initialize = false) {
			if (Source == null) {
				SendDefaultPan();
				return;
			}
			
			panInitialized = !initialize && panInitialized;
			sourcePosition = GetSourcePosition(initialize);
			distance = Vector3.Distance(sourcePosition, pureData.listener.position);
			
			SetAttenuation();
			Pan();
		}

		public virtual void Pan() {
			if (skipped) {
				SendSkippedPan();
				spatialize = true;
				return;
			}
			
			Vector3 listenerToSource = sourcePosition - pureData.listener.position;
			
			float angle = Vector3.Angle(pureData.listener.right, listenerToSource);
			float panLeft = ((1 - PanLevel) + PanLevel * Mathf.Sin(Mathf.Max(180 - angle, 90) * Mathf.Deg2Rad)) * attenuation;
			float panRight = ((1 - PanLevel) + PanLevel * Mathf.Sin(Mathf.Max(angle, 90) * Mathf.Deg2Rad)) * attenuation;
			
			SendPan(panLeft, panRight);
			
			panInitialized = true;
		}

		public virtual void SetAttenuation() {
			const float curveDepth = 3.5F;
			
			float normalizedDistance = Mathf.Clamp01(Mathf.Max(distance - MinDistance, 0) / Mathf.Max(MaxDistance - MinDistance, 0.001F));
			
			if (VolumeRolloffMode == PureDataVolumeRolloffModes.Logarithmic) {
				attenuation = Mathf.Pow((1F - Mathf.Pow(normalizedDistance, 1F / curveDepth)), curveDepth);
			}
			else {
				attenuation = 1F - normalizedDistance;
			}
		}
		
		public virtual void SendPan(float panLeft, float panRight) {
			pureData.communicator.Send(PanLeftSendName, panLeft, panInitialized ? 10 : 0);
			pureData.communicator.Send(PanRightSendName, panRight, panInitialized ? 10 : 0);
		}
		
		public virtual void SendDefaultPan() {
			pureData.communicator.Send(PanLeftSendName, 1, 0);
			pureData.communicator.Send(PanRightSendName, 1, 0);
		}
		
		public virtual void SendSkippedPan() {
			pureData.communicator.Send(PanLeftSendName, 0, 0);
			pureData.communicator.Send(PanRightSendName, 0, 0);
		}
		
		public virtual Vector3 GetSourcePosition(bool initialize = false) {
			sourcePosition = pureData.listener.position;
			
			if (Source as PureDataListener != null) {
				sourcePosition = ((PureDataListener)Source).position;
			}
			else if (pureData.generalSettings.IsMainThread() && Source as Transform != null) {
				sourcePosition = ((Transform)Source).position;
			}
			else if (Source is Vector3) {
				sourcePosition = ((Vector3)Source);
			}
			else if (initialize) {
				skipped = true;
			}
			
			return sourcePosition;
		}

		public virtual bool SourceHasChanged() {
			bool hasChanged = false;
			
			if (spatialize) {
				hasChanged = true;
				spatialize = false;
			}
			
			if (Source != null && Source != pureData.listener && pureData.listener.transform.hasChanged) {
				pureData.SetTransformHasChanged(pureData.listener.transform, false);
				spatialize = true;
				hasChanged = true;
			}
			
			if (Source as Transform != null && ((Transform)Source).hasChanged) {
				pureData.SetTransformHasChanged(((Transform)Source), false);
				spatialize = true;
				hasChanged = true;
			}
			
			return hasChanged;
		}
	}
}

