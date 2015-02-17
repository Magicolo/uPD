using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class Vector4Extensions {
	
		public static Vector4 Lerp(this Vector4 vector, Vector4 target, float time) {
			return Vector4.Lerp(vector, target, time);
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
