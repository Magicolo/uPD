using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class QuaternionExtensions {

		public static Quaternion Round(this Quaternion quaternion, double step, string axis) {
			if (step <= 0) return quaternion;
			if (axis.Contains("X")) {
				quaternion.x = (float)(Mathf.Round((float)(quaternion.x * (1D / step))) / (1D / step));
			}
			if (axis.Contains("Y")) {
				quaternion.y = (float)(Mathf.Round((float)(quaternion.y * (1D / step))) / (1D / step));
			}
			if (axis.Contains("Z")) {
				quaternion.z = (float)(Mathf.Round((float)(quaternion.z * (1D / step))) / (1D / step));
			}
			if (axis.Contains("W")) {
				quaternion.w = (float)(Mathf.Round((float)(quaternion.w * (1D / step))) / (1D / step));
			}
			return quaternion;
		}
	
		public static Quaternion Round(this Quaternion quaternion, double step) {
			return quaternion.Round(step, "XYZW");
		}
	
		public static Quaternion Round(this Quaternion quaternion) {
			return quaternion.Round(1, "XYZW");
		}
	
		public static Quaternion Pow(this Quaternion quaternion, float power) {
			float inputMagnitude = quaternion.Magnitude();
			Vector3 nHat = new Vector3(quaternion.x, quaternion.y, quaternion.z).normalized;
			Quaternion vectorBit = new Quaternion(nHat.x, nHat.y, nHat.z, 0)
			.ScalarMultiply(power * Mathf.Acos(quaternion.w / inputMagnitude))
				.Exp();
			return vectorBit.ScalarMultiply(Mathf.Pow(inputMagnitude, power));
		}
 
		public static Quaternion Exp(this Quaternion quaternion) {
			float inputA = quaternion.w;
			var inputV = new Vector3(quaternion.x, quaternion.y, quaternion.z);
			float outputA = Mathf.Exp(inputA) * Mathf.Cos(inputV.magnitude);
			Vector3 outputV = Mathf.Exp(inputA) * (inputV.normalized * Mathf.Sin(inputV.magnitude));
			return new Quaternion(outputV.x, outputV.y, outputV.z, outputA);
		}
 
		public static float Magnitude(this Quaternion quaternion) {
			return Mathf.Sqrt(quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z + quaternion.w * quaternion.w);
		}
 
		public static Quaternion ScalarMultiply(this Quaternion quaternion, float scalar) {
			return new Quaternion(quaternion.x * scalar, quaternion.y * scalar, quaternion.z * scalar, quaternion.w * scalar);
		}
	}
}
