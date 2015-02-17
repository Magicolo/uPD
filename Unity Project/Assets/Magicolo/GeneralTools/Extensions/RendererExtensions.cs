using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class RendererExtensions {

		public static Color GetColor(this Renderer renderer) {
			if (renderer is SpriteRenderer) {
				return ((SpriteRenderer)renderer).color;
			}
			return renderer.sharedMaterial.color;
		}
		
		public static void SetColor(this Renderer renderer, Color color, string channels) {
			if (renderer is SpriteRenderer) {
				((SpriteRenderer)renderer).SetColor(color, channels);
			}
			else {
				renderer.sharedMaterial.SetColor(color, channels);
			}
		}
		
		public static void SetColor(this Renderer renderer, float color, string channels) {
			renderer.SetColor(new Color(color, color, color, color), channels);
		}
		
		public static bool IsVisibleFrom(this Renderer renderer, Camera camera) {
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
			return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
		}
	}
}
