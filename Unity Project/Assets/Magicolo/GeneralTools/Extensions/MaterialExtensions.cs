using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public static class MaterialExtensions {

		public static void SetColor(this Material material, Color color, string channels) {
			Color newColor = material.color;
			if (channels.Contains("R")) newColor.r = color.r;
			if (channels.Contains("G")) newColor.g = color.g;
			if (channels.Contains("B")) newColor.b = color.b;
			if (channels.Contains("A")) newColor.a = color.a;
			material.color = newColor;
		}
		
		public static void SetColor(this Material material, float color, string channels) {
			material.SetColor(new Color(color, color, color, color), channels);
		}
	}
}

