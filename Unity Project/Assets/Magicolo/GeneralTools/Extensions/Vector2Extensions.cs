using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class Vector2Extensions {

		public static Vector2 SetValues(this Vector2 vector, Vector2 values, string axis) {
			return ((Vector4)vector).SetValues((Vector4)values, axis);
		}
		
		public static Vector2 SetValues(this Vector2 vector, Vector2 values) {
			return vector.SetValues(values, "XY");
		}
				
		public static Vector2 Lerp(this Vector2 vector, Vector2 target, float time, string axis) {
			return ((Vector4)vector).Lerp((Vector4)target, time, axis);
		}
			
		public static Vector2 Lerp(this Vector2 vector, Vector2 target, float time) {
			return vector.Lerp(target, time, "XY");
		}
		
		public static Vector2 LerpLinear(this Vector2 vector, Vector2 target, float time, string axis) {
			return ((Vector4)vector).LerpLinear((Vector4)target, time, axis);
		}
		
		public static Vector2 LerpLinear(this Vector2 vector, Vector2 target, float time) {
			return vector.LerpLinear(target, time, "XY");
		}

		public static Vector2 LerpAngles(this Vector2 vector, Vector2 targetAngles, float time, string axis) {
			return ((Vector4)vector).LerpAngles((Vector4)targetAngles, time, axis);
		}

		public static Vector2 LerpAngles(this Vector2 vector, Vector2 targetAngles, float time) {
			return vector.LerpAngles(targetAngles, time, "XY");
		}

		public static Vector2 LerpAnglesLinear(this Vector2 vector, Vector2 targetAngles, float time, string axis) {
			return ((Vector4)vector).LerpAnglesLinear((Vector4)targetAngles, time, axis);
		}
		
		public static Vector2 LerpAnglesLinear(this Vector2 vector, Vector2 targetAngles, float time) {
			return vector.LerpAnglesLinear(targetAngles, time, "XY");
		}
		
		public static Vector2 Oscillate(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center, float offset, string axis) {
			return ((Vector4)vector).Oscillate((Vector4)frequency, (Vector4)amplitude, (Vector4)center, offset, axis);
		}
		
		public static Vector2 Oscillate(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center, float offset) {
			return vector.Oscillate(frequency, amplitude, center, offset, "XY");
		}
		
		public static Vector2 Oscillate(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center, string axis) {
			return vector.Oscillate(frequency, amplitude, center, 0, axis);
		}
		
		public static Vector2 Oscillate(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center) {
			return vector.Oscillate(frequency, amplitude, center, 0, "XY");
		}
		
		public static Vector2 OscillateAngles(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center, float offset, string axis) {
			return ((Vector4)vector).OscillateAngles((Vector4)frequency, (Vector4)amplitude, (Vector4)center, offset, axis);
		}

		public static Vector2 OscillateAngles(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center, float offset) {
			return vector.OscillateAngles(frequency, amplitude, center, offset, "XY");
		}

		public static Vector2 OscillateAngles(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center, string axis) {
			return vector.OscillateAngles(frequency, amplitude, center, 0, axis);
		}

		public static Vector2 OscillateAngles(this Vector2 vector, Vector2 frequency, Vector2 amplitude, Vector2 center) {
			return vector.OscillateAngles(frequency, amplitude, center, 0, "XY");
		}
		
		public static bool Intersects(this Vector2 vector, Rect rect) {
			return ((Vector3)vector).Intersects(rect);
		}
		
		public static Vector2 Rotate(this Vector2 vector, float angle) {
			return ((Vector3)vector).Rotate(angle);
		}
	
		public static Vector2 SquareClamp(this Vector2 vector, float size = 1) {
			return ((Vector3)vector).SquareClamp(size);
		}
	
		public static Vector2 RectClamp(this Vector2 vector, float width = 1, float height = 1) {
			return ((Vector3)vector).RectClamp(width, height);
		}
	
		public static Vector2 Mult(this Vector2 vector, Vector2 otherVector, string axis) {
			return ((Vector4)vector).Mult(otherVector, axis);
		}
	
		public static Vector2 Mult(this Vector2 vector, Vector2 otherVector) {
			return vector.Mult(otherVector, "XY");
		}
	
		public static Vector2 Mult(this Vector2 vector, Vector3 otherVector, string axis) {
			return vector.Mult((Vector2)otherVector, axis);
		}
	
		public static Vector2 Mult(this Vector2 vector, Vector3 otherVector) {
			return vector.Mult((Vector2)otherVector, "XY");
		}
	
		public static Vector2 Mult(this Vector2 vector, Vector4 otherVector, string axis) {
			return vector.Mult((Vector2)otherVector, axis);
		}
	
		public static Vector2 Mult(this Vector2 vector, Vector4 otherVector) {
			return vector.Mult((Vector2)otherVector, "XY");
		}
	
		public static Vector2 Div(this Vector2 vector, Vector2 otherVector, string axis) {
			return ((Vector4)vector).Div(otherVector, axis);
		}
	
		public static Vector2 Div(this Vector2 vector, Vector2 otherVector) {
			return vector.Div(otherVector, "XY");
		}
	
		public static Vector2 Div(this Vector2 vector, Vector3 otherVector, string axis) {
			return vector.Div((Vector2)otherVector, axis);
		}
	
		public static Vector2 Div(this Vector2 vector, Vector3 otherVector) {
			return vector.Div((Vector2)otherVector, "XY");
		}
	
		public static Vector2 Div(this Vector2 vector, Vector4 otherVector, string axis) {
			return vector.Div((Vector2)otherVector, axis);
		}
	
		public static Vector2 Div(this Vector2 vector, Vector4 otherVector) {
			return vector.Div((Vector2)otherVector, "XY");
		}
	
		public static Vector2 Pow(this Vector2 vector, double power, string axis) {
			return ((Vector4)vector).Pow(power, axis);
		}
	
		public static Vector2 Pow(this Vector2 vector, double power) {
			return vector.Pow(power, "XY");
		}
	
		public static Vector2 Round(this Vector2 vector, double step, string axis) {
			return ((Vector4)vector).Round(step, axis);
		}
	
		public static Vector2 Round(this Vector2 vector, double step) {
			return vector.Round(step, "XY");
		}
	
		public static Vector2 Round(this Vector2 vector) {
			return vector.Round(1, "XY");
		}
	
		public static float Average(this Vector2 vector, string axis) {
			return ((Vector4)vector).Average(axis);
		}
	
		public static float Average(this Vector2 vector) {
			return ((Vector4)vector).Average("XY");
		}
	}
}
