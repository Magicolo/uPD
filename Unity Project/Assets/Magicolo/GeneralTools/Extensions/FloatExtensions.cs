using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class FloatExtensions {

		public static float Pow(this float f, double power) {
			return float.IsNaN(f) ? 0 : Mathf.Pow(f, (float)power);
		}
	
		public static float Pow(this float f) {
			return f.Pow(2);
		}
	
		public static float Round(this float f, double step) {
			if (float.IsNaN(f)) {
				return 0;
			}
		
			if (step <= 0) {
				return f;
			}
		
			return (float)(Mathf.Round((float)(f * (1D / step))) / (1D / step));
		}
	
		public static float Round(this float f) {
			return f.Round(1);
		}
	}
}
