using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class Vector4Extensions {
	
		public static Vector4 SetValues(this Vector4 vector, Vector4 values, string axis) {
			return new Vector4(axis.Contains("X") ? values.x : vector.x, axis.Contains("Y") ? values.y : vector.y, axis.Contains("Z") ? values.z : vector.z, axis.Contains("W") ? values.w : vector.w);
		}
		
		public static Vector4 SetValues(this Vector4 vector, Vector4 values) {
			return vector.SetValues(values, "XYZW");
		}
				
		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float time, string axis) {
			return vector.SetValues(new Vector4(Mathf.Lerp(vector.x, target.x, time), Mathf.Lerp(vector.y, target.y, time), Mathf.Lerp(vector.z, target.z, time), Mathf.Lerp(vector.w, target.w, time)), axis);
		}
			
		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float time) {
			return vector.Lerp(target, time, "XYZW");
		}
		
		public static Vector4 LerpLinear(this Vector4 vector, Vector4 target, float time, string axis) {
			Vector4 difference = target - vector;
			Vector4 direction = Vector4.zero.SetValues(difference, axis);
			float distance = direction.magnitude;
					
			Vector4 adjustedDirection = direction.normalized * time;
					
			if (adjustedDirection.magnitude < distance) {
				vector += Vector4.zero.SetValues(adjustedDirection, axis);
			}
			else {
				vector = vector.SetValues(target, axis);
			}
			
			return vector;
		}
		
		public static Vector4 LerpLinear(this Vector4 vector, Vector4 target, float time) {
			return vector.LerpLinear(target, time, "XYZW");
		}

		public static Vector4 LerpAngles(this Vector4 vector, Vector4 targetAngles, float time, string axis) {
			return vector.SetValues(new Vector4(Mathf.LerpAngle(vector.x, targetAngles.x, time), Mathf.LerpAngle(vector.y, targetAngles.y, time), Mathf.LerpAngle(vector.z, targetAngles.z, time), Mathf.LerpAngle(vector.w, targetAngles.w, time)), axis);
		}

		public static Vector4 LerpAngles(this Vector4 vector, Vector4 targetAngles, float time) {
			return vector.LerpAngles(targetAngles, time, "XYZW");
		}

		public static Vector4 LerpAnglesLinear(this Vector4 vector, Vector4 targetAngles, float time, string axis) {
			Vector4 difference = new Vector4(Mathf.DeltaAngle(vector.x, targetAngles.x), Mathf.DeltaAngle(vector.y, targetAngles.y), Mathf.DeltaAngle(vector.z, targetAngles.z), Mathf.DeltaAngle(vector.w, targetAngles.w));
			Vector4 direction = Vector4.zero.SetValues(difference, axis);
			float distance = direction.magnitude * Mathf.Rad2Deg;
					
			Vector4 adjustedDirection = direction.normalized * time;
					
			if (adjustedDirection.magnitude < distance) {
				vector += Vector4.zero.SetValues(adjustedDirection, axis);
			}
			else {
				vector = vector.SetValues(targetAngles, axis);
			}
			
			return vector;
		}
		
		public static Vector4 LerpAnglesLinear(this Vector4 vector, Vector4 targetAngles, float time) {
			return vector.LerpAnglesLinear(targetAngles, time, "XYZW");
		}
		
		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset, string axis) {
			return vector.SetValues(new Vector4(center.x + amplitude.x * Mathf.Sin(frequency.x * Time.time + offset), center.y + amplitude.y * Mathf.Sin(frequency.y * Time.time + offset), center.z + amplitude.z * Mathf.Sin(frequency.z * Time.time + offset), center.w + amplitude.w * Mathf.Sin(frequency.w * Time.time + offset)), axis);
		}
		
		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset) {
			return vector.Oscillate(frequency, amplitude, center, offset, "XYZW");
		}
		
		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, string axis) {
			return vector.Oscillate(frequency, amplitude, center, 0, axis);
		}
		
		public static Vector4 Oscillate(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center) {
			return vector.Oscillate(frequency, amplitude, center, 0, "XYZW");
		}
		
		public static Vector4 OscillateAngles(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset, string axis) {
			return vector.SetValues(new Vector4(center.x + amplitude.x * Mathf.Sin(frequency.x * Time.time + offset) * Mathf.Rad2Deg, center.y + amplitude.y * Mathf.Sin(frequency.y * Time.time + offset) * Mathf.Rad2Deg, center.z + amplitude.z * Mathf.Sin(frequency.z * Time.time + offset) * Mathf.Rad2Deg, center.w + amplitude.w * Mathf.Sin(frequency.w * Time.time + offset) * Mathf.Rad2Deg), axis);
		}

		public static Vector4 OscillateAngles(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, float offset) {
			return vector.OscillateAngles(frequency, amplitude, center, offset, "XYZW");
		}

		public static Vector4 OscillateAngles(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center, string axis) {
			return vector.OscillateAngles(frequency, amplitude, center, 0, axis);
		}

		public static Vector4 OscillateAngles(this Vector4 vector, Vector4 frequency, Vector4 amplitude, Vector4 center) {
			return vector.OscillateAngles(frequency, amplitude, center, 0, "XYZW");
		}
		
		public static Vector4 Mult(this Vector4 vector, Vector4 otherVector, string axis) {
			if (axis.Contains("X")) {
				vector.x *= otherVector.x;
			}
		
			if (axis.Contains("Y")) {
				vector.y *= otherVector.y;
			}
		
			if (axis.Contains("Z")) {
				vector.z *= otherVector.z;
			}
		
			if (axis.Contains("W")) {
				vector.w *= otherVector.w;
			}
		
			return vector;
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector4 otherVector) {
			return vector.Mult(otherVector, "XYZW");
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector2 otherVector, string axis) {
			return vector.Mult((Vector4)otherVector, axis);
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector2 otherVector) {
			return vector.Mult((Vector4)otherVector, "XY");
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector3 otherVector, string axis) {
			return vector.Mult((Vector4)otherVector, axis);
		}
	
		public static Vector4 Mult(this Vector4 vector, Vector3 otherVector) {
			return vector.Mult((Vector4)otherVector, "XYZ");
		}
	
		public static Vector4 Div(this Vector4 vector, Vector4 otherVector, string axis) {
			if (axis.Contains("X")) {
				vector.x /= otherVector.x;
			}
		
			if (axis.Contains("Y")) {
				vector.y /= otherVector.y;
			}
		
			if (axis.Contains("Z")) {
				vector.z /= otherVector.z;
			}
		
			if (axis.Contains("W")) {
				vector.w /= otherVector.w;
			}
		
			return vector;
		}
	
		public static Vector4 Div(this Vector4 vector, Vector4 otherVector) {
			return vector.Div(otherVector, "XYZW");
		}
	
		public static Vector4 Div(this Vector4 vector, Vector2 otherVector, string axis) {
			return vector.Div((Vector4)otherVector, axis);
		}
	
		public static Vector4 Div(this Vector4 vector, Vector2 otherVector) {
			return vector.Div((Vector4)otherVector, "XY");
		}
	
		public static Vector4 Div(this Vector4 vector, Vector3 otherVector, string axis) {
			return vector.Div((Vector4)otherVector, axis);
		}
	
		public static Vector4 Div(this Vector4 vector, Vector3 otherVector) {
			return vector.Div((Vector4)otherVector, "XYZ");
		}
	
		public static Vector4 Pow(this Vector4 vector, double power, string axis) {
			if (axis.Contains("X")) {
				vector.x = Mathf.Pow(vector.x, (float)power);
			}
			if (axis.Contains("Y")) {
				vector.y = Mathf.Pow(vector.y, (float)power);
			}
			if (axis.Contains("Z")) {
				vector.z = Mathf.Pow(vector.z, (float)power);
			}
			if (axis.Contains("W")) {
				vector.w = Mathf.Pow(vector.w, (float)power);
			}
			return vector;
		}
	
		public static Vector4 Pow(this Vector4 vector, double power) {
			return vector.Pow(power, "XYZW");
		}
	
		public static Vector4 Round(this Vector4 vector, double step, string axis) {
			if (axis.Contains("X")) {
				vector.x = vector.x.Round(step);
			}
			if (axis.Contains("Y")) {
				vector.y = vector.y.Round(step);
			}
			if (axis.Contains("Z")) {
				vector.z = vector.z.Round(step);
			}
			if (axis.Contains("W")) {
				vector.w = vector.w.Round(step);
			}
			return vector;
		}
	
		public static Vector4 Round(this Vector4 vector, double step) {
			return vector.Round(step, "XYZW");
		}
	
		public static Vector4 Round(this Vector4 vector) {
			return vector.Round(1, "XYZW");
		}
	
		public static float Average(this Vector4 vector, string axis) {
			float average = 0;
			int axisCount = 0;
		
			if (axis.Contains("X")) {
				average += vector.x;
				axisCount += 1;
			}
		
			if (axis.Contains("Y")) {
				average += vector.y;
				axisCount += 1;
			}
		
			if (axis.Contains("Z")) {
				average += vector.z;
				axisCount += 1;
			}
		
			if (axis.Contains("W")) {
				average += vector.w;
				axisCount += 1;
			}
		
			return average / axisCount;
		}
	
		public static float Average(this Vector4 vector) {
			return ((Vector4)vector).Average("XYZW");
		}
	}
}
