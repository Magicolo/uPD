using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class Vector2Extensions {

		public static bool Intersects(this Vector2 vector, Rect rect) {
			return vector.x >= rect.xMin && vector.x <= rect.xMax && vector.y >= rect.yMin && vector.y <= rect.yMax;
		}
		
		public static Vector2 Rotate(this Vector2 vector, float angle) {
			return ((Vector3)vector).Rotate(angle);
		}
	
		public static Vector2 Lerp(this Vector2 vector, Vector2 target, float time) {
			return Vector2.Lerp(vector, target, time);
		}
		
		public static Vector2 SquareClamp(this Vector2 vector, float size = 1) {
			Vector3 v = new Vector3(vector.x, vector.y, 0).SquareClamp(size);
			return new Vector2(v.x, v.y);
		}
	
		public static Vector2 RectClamp(this Vector2 vector, float width = 1, float height = 1) {
			Vector3 v = new Vector3(vector.x, vector.y, 0).RectClamp(width, height);
			return new Vector2(v.x, v.y);
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
