using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class ColorExtensions {

		public static Color SetValues(this Color color, Color values, string channels) {
			return ((Vector4)color).SetValues((Vector4)values, HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
		
		public static Color SetValues(this Color color, Color values) {
			return ((Vector4)color).SetValues((Vector4)values);
		}
				
		public static Color Lerp(this Color color, Color target, float time, string channels) {
			return ((Vector4)color).Lerp((Vector4)target, time, HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
			
		public static Color Lerp(this Color color, Color target, float time) {
			return ((Vector4)color).Lerp((Vector4)target, time);
		}
		
		public static Color LerpLinear(this Color color, Color target, float time, string channels) {
			return ((Vector4)color).LerpLinear((Vector4)target, time, HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
		
		public static Color LerpLinear(this Color color, Color target, float time) {
			return ((Vector4)color).LerpLinear((Vector4)target, time);
		}

		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center, float offset, string channels) {
			return ((Vector4)color).Oscillate((Vector4)frequency, (Vector4)amplitude, (Vector4)center, offset, HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
		
		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center, float offset) {
			return ((Vector4)color).Oscillate((Vector4)frequency, (Vector4)amplitude, (Vector4)center, offset);
		}
		
		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center, string channels) {
			return color.Oscillate(frequency, amplitude, center, 0, channels);
		}
		
		public static Color Oscillate(this Color color, Color frequency, Color amplitude, Color center) {
			return color.Oscillate(frequency, amplitude, center, 0);
		}
		
		public static Color Mult(this Color color, Color otherColor, string channels) {
			return ((Vector4)color).Mult((Vector4)otherColor, HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
	
		public static Color Mult(this Color color, Color otherColor) {
			return ((Vector4)color).Mult((Vector4)otherColor);
		}
	
		public static Color Div(this Color color, Color otherColor, string channels) {
			return ((Vector4)color).Div((Vector4)otherColor, HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
	
		public static Color Div(this Color color, Color otherColor) {
			return ((Vector4)color).Div((Vector4)otherColor);
		}
	
		public static Color Pow(this Color color, double power, string channels) {
			return ((Vector4)color).Pow(power, HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
	
		public static Color Pow(this Color color, double power) {
			return ((Vector4)color).Pow(power);
		}
	
		public static Color Round(this Color color, double step, string channels) {
			return ((Vector4)color).Round(step, HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
	
		public static Color Round(this Color color, double step) {
			return ((Vector4)color).Round(step);
		}
	
		public static Color Round(this Color color) {
			return color.Round(1);
		}
	
		public static float Average(this Color color, string channels) {
			return ((Vector4)color).Average(HelperFunctions.ColorChannelsToVectorAxis(channels));
		}
	
		public static float Average(this Color color) {
			return ((Vector4)color).Average();
		}
	
		public static Color ToHSV(this Color RGBColor) {
			float R = RGBColor.r;
			float G = RGBColor.g;
			float B = RGBColor.b;
			float H = 0;
			float S = 0;
			float V = 0;
			float d = 0;
			float h = 0;
		
			float minRGB = Mathf.Min(R, Mathf.Min(G, B));
			float maxRGB = Mathf.Max(R, Mathf.Max(G, B));
	
			if (minRGB == maxRGB) {
				return new Color(0, 0, minRGB, RGBColor.a);
			}

			if (R == minRGB) {
				d = G - B;
			}
			else if (B == minRGB) {
				d = R - G;
			}
			else {
				d = B - R;
			}
			
			if (R == minRGB) {
				h = 3;
			}
			else if (B == minRGB) {
				h = 1;
			}
			else {
				h = 5;
			}
			
			H = (60 * (h - d / (maxRGB - minRGB))) / 360;
			S = (maxRGB - minRGB) / maxRGB;
			V = maxRGB;
		
			return new Color(H, S, V, RGBColor.a);
		}
	
		public static Color ToRGB(this Color HSVColor) {
			float H = HSVColor.r;
			float S = HSVColor.g;
			float V = HSVColor.b;
			float R = 0;
			float G = 0;
			float B = 0;
			float maxHSV = 255 * V;
			float minHSV = maxHSV * (1 - S);
			float h = H * 360;
			float z = (maxHSV - minHSV) * (1 - Mathf.Abs((h / 60) % 2 - 1));
		
			if (0 <= h && h < 60) {
				R = maxHSV;
				G = z + minHSV;
				B = minHSV;
			}
			else if (60 <= h && h < 120) {
				R = z + minHSV;
				G = maxHSV;
				B = minHSV;
			}
			else if (120 <= h && h < 180) {
				R = minHSV;
				G = maxHSV;
				B = z + minHSV;	
			}
			else if (180 <= h && h < 240) {
				R = minHSV;
				G = z + minHSV;
				;
				B = maxHSV;
			}
			else if (240 <= h && h < 300) {
				R = z + minHSV;
				G = minHSV;
				B = maxHSV;
			}
			else if (300 <= h && h < 360) {
				R = maxHSV;
				G = minHSV;
				B = z + minHSV;
			}
			return new Color(R / 255, G / 255, B / 255, HSVColor.a);
		}
	}
}
