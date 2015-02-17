using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo {
	public static class SpriteRendererExtensions {

		public static void SetColor(this SpriteRenderer spriteRenderer, Color color, string channels) {
			Color newColor = spriteRenderer.color;
			if (channels.Contains("R")) newColor.r = color.r;
			if (channels.Contains("G")) newColor.g = color.g;
			if (channels.Contains("B")) newColor.b = color.b;
			if (channels.Contains("A")) newColor.a = color.a;
			spriteRenderer.color = newColor;
		}
		
		public static void SetColor(this SpriteRenderer spriteRenderer, float color, string channels) {
			spriteRenderer.SetColor(new Color(color, color, color, color), channels);
		}
	}
}

