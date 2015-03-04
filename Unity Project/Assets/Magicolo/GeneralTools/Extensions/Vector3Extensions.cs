using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class Vector3Extensions {
		
		public static Vector3 SetValues(this Vector3 vector, Vector3 values, string axis) {
			return ((Vector4)vector).SetValues((Vector4)values, axis);
		}
		
		public static Vector3 SetValues(this Vector3 vector, Vector3 values) {
			return vector.SetValues(values, "XYZ");
		}
				
		public static Vector3 Lerp(this Vector3 vector, Vector3 target, float time, string axis) {
			return ((Vector4)vector).Lerp((Vector4)target, time, axis);
		}
			
		public static Vector3 Lerp(this Vector3 vector, Vector3 target, float time) {
			return vector.Lerp(target, time, "XYZ");
		}
		
		public static Vector3 LerpLinear(this Vector3 vector, Vector3 target, float time, string axis) {
			return ((Vector4)vector).LerpLinear((Vector4)target, time, axis);
		}
		
		public static Vector3 LerpLinear(this Vector3 vector, Vector3 target, float time) {
			return vector.LerpLinear(target, time, "XYZ");
		}

		public static Vector3 LerpAngles(this Vector3 vector, Vector3 targetAngles, float time, string axis) {
			return ((Vector4)vector).LerpAngles((Vector4)targetAngles, time, axis);
		}

		public static Vector3 LerpAngles(this Vector3 vector, Vector3 targetAngles, float time) {
			return vector.LerpAngles(targetAngles, time, "XYZ");
		}

		public static Vector3 LerpAnglesLinear(this Vector3 vector, Vector3 targetAngles, float time, string axis) {
			return ((Vector4)vector).LerpAnglesLinear((Vector4)targetAngles, time, axis);
		}
		
		public static Vector3 LerpAnglesLinear(this Vector3 vector, Vector3 targetAngles, float time) {
			return vector.LerpAnglesLinear(targetAngles, time, "XYZ");
		}
		
		public static Vector3 Oscillate(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center, float offset, string axis) {
			return ((Vector4)vector).Oscillate((Vector4)frequency, (Vector4)amplitude, (Vector4)center, offset, axis);
		}
		
		public static Vector3 Oscillate(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center, float offset) {
			return vector.Oscillate(frequency, amplitude, center, offset, "XYZ");
		}
		
		public static Vector3 Oscillate(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis) {
			return vector.Oscillate(frequency, amplitude, center, 0, axis);
		}
		
		public static Vector3 Oscillate(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center) {
			return vector.Oscillate(frequency, amplitude, center, 0, "XYZ");
		}
		
		public static Vector3 OscillateAngles(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center, float offset, string axis) {
			return ((Vector4)vector).OscillateAngles((Vector4)frequency, (Vector4)amplitude, (Vector4)center, offset, axis);
		}

		public static Vector3 OscillateAngles(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center, float offset) {
			return vector.OscillateAngles(frequency, amplitude, center, offset, "XYZ");
		}

		public static Vector3 OscillateAngles(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis) {
			return vector.OscillateAngles(frequency, amplitude, center, 0, axis);
		}

		public static Vector3 OscillateAngles(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center) {
			return vector.OscillateAngles(frequency, amplitude, center, 0, "XYZ");
		}
		
		public static bool Intersects(this Vector3 vector, Rect rect) {
			return vector.x >= rect.xMin && vector.x <= rect.xMax && vector.y >= rect.yMin && vector.y <= rect.yMax;
		}
		
		public static Vector3 Rotate(this Vector3 vector, float angle) {
			return vector.Rotate(angle, Vector3.forward);
		}
	
		public static Vector3 Rotate(this Vector3 vector, float angle, Vector3 axis) {
			angle %= 360;
			return Quaternion.AngleAxis(-angle, axis) * vector;
		}
		
		public static Vector3 SquareClamp(this Vector3 vector, float size = 1) {
			return vector.RectClamp(size, size);
		}
	
		public static Vector3 RectClamp(this Vector3 vector, float width = 1, float height = 1) {
			float clamped;
		
			if (vector.x < -width || vector.x > width) {
				clamped = Mathf.Clamp(vector.x, -width, width);
				vector.y *= clamped / vector.x;
				vector.x = clamped;
			}
		
			if (vector.y < -height || vector.y > height) {
				clamped = Mathf.Clamp(vector.y, -height, height);
				vector.x *= clamped / vector.y;
				vector.y = clamped;
			}
		
			return vector;
		}
	
		public static Vector3 Mult(this Vector3 vector, Vector3 otherVector, string axis) {
			return ((Vector4)vector).Mult(otherVector, axis);
		}
	
		public static Vector3 Mult(this Vector3 vector, Vector3 otherVector) {
			return vector.Mult(otherVector, "XYZ");
		}
	
		public static Vector3 Mult(this Vector3 vector, Vector2 otherVector, string axis) {
			return vector.Mult((Vector3)otherVector, axis);
		}
	
		public static Vector3 Mult(this Vector3 vector, Vector2 otherVector) {
			return vector.Mult((Vector3)otherVector, "XY");
		}
	
		public static Vector3 Mult(this Vector3 vector, Vector4 otherVector, string axis) {
			return vector.Mult((Vector3)otherVector, axis);
		}
	
		public static Vector3 Mult(this Vector3 vector, Vector4 otherVector) {
			return vector.Mult((Vector3)otherVector, "XYZ");
		}
	
		public static Vector3 Div(this Vector3 vector, Vector3 otherVector, string axis) {
			return ((Vector4)vector).Div(otherVector, axis);
		}
	
		public static Vector3 Div(this Vector3 vector, Vector3 otherVector) {
			return vector.Div(otherVector, "XYZ");
		}
	
		public static Vector3 Div(this Vector3 vector, Vector2 otherVector, string axis) {
			return vector.Div((Vector3)otherVector, axis);
		}
	
		public static Vector3 Div(this Vector3 vector, Vector2 otherVector) {
			return vector.Div((Vector3)otherVector, "XY");
		}
	
		public static Vector3 Div(this Vector3 vector, Vector4 otherVector, string axis) {
			return vector.Div((Vector3)otherVector, axis);
		}
	
		public static Vector3 Div(this Vector3 vector, Vector4 otherVector) {
			return vector.Div((Vector3)otherVector, "XYZ");
		}
	
		public static Vector3 Pow(this Vector3 vector, double power, string axis) {
			return ((Vector4)vector).Pow(power, axis);
		}
	
		public static Vector3 Pow(this Vector3 vector, double power) {
			return vector.Pow(power, "XYZ");
		}
	
		public static Vector3 Round(this Vector3 vector, double step, string axis) {
			return ((Vector4)vector).Round(step, axis);
		}
	
		public static Vector3 Round(this Vector3 vector, double step) {
			return vector.Round(step, "XYZ");
		}
	
		public static Vector3 Round(this Vector3 vector) {
			return vector.Round(1, "XYZ");
		}
	
		public static float Average(this Vector3 vector, string axis) {
			return ((Vector4)vector).Average(axis);
		}
	
		public static float Average(this Vector3 vector) {
			return ((Vector4)vector).Average("XYZ");
		}
	}
}
