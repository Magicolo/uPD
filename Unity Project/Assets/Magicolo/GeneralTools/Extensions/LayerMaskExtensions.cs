using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class LayerMaskExtensions {

		public static LayerMask Inverse(this LayerMask layerMask) {
			return ~layerMask;
		}
 
		public static LayerMask AddToMask(this LayerMask layerMask, params int[] layerNumbers) {
			foreach (int layer in layerNumbers) {
				layerMask |= (1 << layer);
			}
			return layerMask;
		}
	
		public static LayerMask AddToMask(this LayerMask layerMask, params string[] layerNames) {
			foreach (string layer in layerNames) {
				layerMask |= (1 << LayerMask.NameToLayer(layer));
			}
			return layerMask;
		}
 
		public static LayerMask RemoveFromMask(this LayerMask layerMask, params string[] layerNames) {
			layerMask = layerMask.Inverse();
			layerMask = layerMask.AddToMask(layerNames);
			return layerMask.Inverse();
		}
	
		public static LayerMask RemoveFromMask(this LayerMask layerMask, params int[] layerNumbers) {
			layerMask = layerMask.Inverse();
			layerMask = layerMask.AddToMask(layerNumbers);
			return layerMask.Inverse();
		}
	
		public static string[] LayerNames(this LayerMask layerMask) {
			var names = new List<string>();
 
			for (int i = 0; i < 32; ++i) {
				int shifted = 1 << i;
				if ((layerMask & shifted) == shifted) {
					string layerName = LayerMask.LayerToName(i);
					if (!string.IsNullOrEmpty(layerName)) {
						names.Add(layerName);
					}
				}
			}
			return names.ToArray();
		}
	}
}
