using UnityEngine;

namespace Magicolo {
	public static class IntExtensions {

		public static float Pow(this int i, double power) {
			return Mathf.Pow(i, (float)power);
		}
	
		public static float Pow(this int i) {
			return i.Pow(2);
		}
	
		public static int Round(this int i, double step) {
			if (step <= 0) {
				return i;
			}
		
			return (int)(Mathf.Round((float)(i * (1D / step))) / (1D / step));
		}
	
		public static int Round(this int i) {
			return i.Round(1);
		}
	}
}
