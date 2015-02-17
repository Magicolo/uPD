using UnityEngine;

namespace Magicolo {
	public static class AnimationCurveExtensions {

		public static AnimationCurve Clamp(this AnimationCurve curve, float minTime, float maxTime, float minValue, float maxValue) {
			for (int i = 0; i < curve.keys.Length; i++) {
				Keyframe key = curve.keys[i];
				if (key.time < minTime || key.time > maxTime || key.value < minValue || key.value > maxValue) {
					var newKey = new Keyframe(Mathf.Clamp(key.time, minTime, maxTime), Mathf.Clamp(key.value, minValue, maxValue));
					newKey.inTangent = key.inTangent;
					newKey.outTangent = key.outTangent;
					curve.MoveKey(i, newKey);
				}
			}
			return curve;
		}
	}
}
