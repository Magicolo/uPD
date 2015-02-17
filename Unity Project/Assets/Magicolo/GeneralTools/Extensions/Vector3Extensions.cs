using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class Vector3Extensions {

		public static Vector3 SetValues(this Vector3 vector, Vector3 values, string axis = "XYZ") {
			Vector3 newVector = vector;
			
			if (axis.Contains("X")) newVector.x = values.x;
			if (axis.Contains("Y")) newVector.y = values.y;
			if (axis.Contains("Z")) newVector.z = values.z;
			
			return newVector;
		}
			
		public static Vector3 Lerp(this Vector3 vector, Vector3 target, float time, string axis = "XYZ") {
			return vector.SetValues(new Vector3(Mathf.Lerp(vector.x, target.x, time), Mathf.Lerp(vector.y, target.y, time), Mathf.Lerp(vector.z, target.z, time)), axis);
		}
		
		public static Vector3 LerpLinear(this Vector3 vector, Vector3 target, float time, string axis = "XYZ") {
			Vector3 difference = target - vector;
			Vector3 direction = Vector3.zero.SetValues(difference, axis);
			float distance = direction.magnitude;
					
			direction.Normalize();
					
			float adjustedSpeed = Time.deltaTime * time;
			Vector3 adjustedDirection = direction * adjustedSpeed;
					
			if (adjustedDirection.magnitude < distance) {
				vector += Vector3.zero.SetValues(adjustedDirection, axis);
			}
			else {
				vector = vector.SetValues(target, axis);
			}
			
			return vector;
		}
		
		public static Vector3 LerpAngles(this Vector3 vector, Vector3 targetAngles, float time, string axis = "XYZ") {
			return vector.SetValues(new Vector3(Mathf.LerpAngle(vector.x, targetAngles.x, time), Mathf.LerpAngle(vector.y, targetAngles.y, time), Mathf.LerpAngle(vector.z, targetAngles.z, time)), axis);
		}

		public static Vector3 LerpAnglesLinear(this Vector3 vector, Vector3 target, float time, string axis = "XYZ") {
			Vector3 difference = new Vector3(Mathf.DeltaAngle(vector.x, target.x), Mathf.DeltaAngle(vector.y, target.y), Mathf.DeltaAngle(vector.z, target.z));
			Vector3 direction = Vector3.zero.SetValues(difference, axis);
			float distance = direction.magnitude * Mathf.Rad2Deg;
					
			direction.Normalize();
					
			float adjustedSpeed = Time.deltaTime * time * Mathf.Rad2Deg;
			Vector3 adjustedDirection = direction * adjustedSpeed;
					
			if (adjustedDirection.magnitude < distance) {
				vector += Vector3.zero.SetValues(adjustedDirection, axis);
			}
			else {
				vector = vector.SetValues(target, axis);
			}
			
			return vector;
		}
		
		public static Vector3 Oscillate(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center, float offset = 0, string axis = "XYZ") {
			return vector.SetValues(new Vector3(center.x + amplitude.x * Mathf.Sin(frequency.x * Time.time + offset), center.y + amplitude.y * Mathf.Sin(frequency.y * Time.time + offset), center.z + amplitude.z * Mathf.Sin(frequency.z * Time.time + offset)), axis);
		}
		
		public static Vector3 OscillateAngles(this Vector3 vector, Vector3 frequency, Vector3 amplitude, Vector3 center, float offset = 0, string axis = "XYZ") {
			return vector.SetValues(new Vector3(center.x + amplitude.x * Mathf.Sin(frequency.x * Time.time + offset) * Mathf.Rad2Deg, center.y + amplitude.y * Mathf.Sin(frequency.y * Time.time + offset) * Mathf.Rad2Deg, center.z + amplitude.z * Mathf.Sin(frequency.z * Time.time + offset) * Mathf.Rad2Deg), axis);
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
