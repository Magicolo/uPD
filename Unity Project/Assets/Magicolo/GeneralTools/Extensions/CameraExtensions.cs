using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class CameraExtensions {

		public static Vector3 GetWorldMousePosition(this Camera camera, float depth = 0) {
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = depth - camera.transform.position.z;
			return camera.ScreenToWorldPoint(mousePosition);
		}
	
		public static bool WorldPointInView(this Camera camera, Vector3 worldPoint) {
			Vector3 viewPoint = camera.WorldToViewportPoint(worldPoint);
			return viewPoint.x >= 0 && viewPoint.x <= 1 && viewPoint.y >= 0 && viewPoint.y <= 1;
		}
	
		public static bool ScreenPointInView(this Camera camera, Vector2 screenPoint) {
			Vector3 viewPoint = camera.ScreenToViewportPoint(screenPoint);
			return viewPoint.x >= 0 && viewPoint.x <= 1 && viewPoint.y >= 0 && viewPoint.y <= 1;
		}
	}
}
