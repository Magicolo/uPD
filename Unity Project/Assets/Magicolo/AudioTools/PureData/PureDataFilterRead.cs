using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;
using LibPDBinding;

namespace Magicolo.AudioTools {
	public class PureDataFilterRead : MonoBehaviour {

		[HideInInspector] public PureData pureData;
	
		bool focused = true;
		bool paused;
		GCHandle dataHandle;
		IntPtr dataPtr;
		
		public static float[] dataSum = new float[0];

		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		void OnDestroy() {
			dataHandle.Free();
			dataPtr = IntPtr.Zero;
		}
		
		void OnApplicationFocus(bool focus) {
			focused = focus || Application.isEditor;
		}
				
		void OnApplicationPause(bool pause) {
			paused = pause;
		}
		
		void OnAudioFilterRead(float[] data, int channels) {
			if (dataPtr == IntPtr.Zero) {
				dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
				dataPtr = dataHandle.AddrOfPinnedObject();
			}
			
			if (pureData.bridge.initialized && focused && !paused && !pureData.editorHelper.editorPaused) {
				LibPD.Process(pureData.bridge.ticks, dataPtr, dataPtr);
			}
		}
	}
}
