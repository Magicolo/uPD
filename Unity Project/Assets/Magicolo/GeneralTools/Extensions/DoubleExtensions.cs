using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class DoubleExtensions {

		public static float Pow(this double d, double power) {
			return double.IsNaN(d) ? 0 : Mathf.Pow((float)d, (float)power);
		}
	
		public static float Pow(this double d) {
			return d.Pow(2);
		}
	
		public static double Round(this double d, double step) {
			if (double.IsNaN(d)) {
				return 0;
			}
		
			if (step <= 0) {
				return d;
			}
		
			return (double)(Mathf.Round((float)(d * (1D / step))) / (1D / step));
		}
	
		public static double Round(this double d) {
			return d.Round(1);
		}
	}
}
