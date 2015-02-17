using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	public static class Rigidbody2DExtension {

		#region Position
		public static void SetPosition(this Rigidbody2D rigidbody, Vector2 position, string axis = "XY") {
			rigidbody.MovePosition(rigidbody.transform.position.SetValues(position, axis));
		}
		
		public static void SetPosition(this Rigidbody2D rigidbody, float position, string axis = "XY") {
			rigidbody.SetPosition(new Vector2(position, position), axis);
		}
		
		public static void Translate(this Rigidbody2D rigidbody, Vector2 translation, string axis = "XY") {
			rigidbody.SetPosition(rigidbody.transform.position + (Vector3)translation, axis);
		}
		
		public static void Translate(this Rigidbody2D rigidbody, float translation, string axis = "XY") {
			rigidbody.Translate(new Vector2(translation, translation), axis);
		}
		
		public static void TranslateTowards(this Rigidbody2D rigidbody, Vector2 targetPosition, float speed, InterpolationModes interpolation, string axis = "XY") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					rigidbody.SetPosition(rigidbody.transform.position.Lerp(targetPosition, Time.fixedDeltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetPosition(rigidbody.transform.position.LerpLinear(targetPosition, Time.fixedDeltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void TranslateTowards(this Rigidbody2D rigidbody, Vector2 targetPosition, float speed, string axis = "XY") {
			rigidbody.TranslateTowards(targetPosition, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void TranslateTowards(this Rigidbody2D rigidbody, float targetPosition, float speed, InterpolationModes interpolation, string axis = "XY") {
			rigidbody.TranslateTowards(new Vector2(targetPosition, targetPosition), speed, interpolation, axis);
		}
		
		public static void TranslateTowards(this Rigidbody2D rigidbody, float targetPosition, float speed, string axis = "XY") {
			rigidbody.TranslateTowards(new Vector2(targetPosition, targetPosition), speed, InterpolationModes.Lerp, axis);
		}
		
		public static void OscillatePosition(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, Vector2 center, string axis = "XY") {
			rigidbody.SetPosition(rigidbody.transform.position.Oscillate(frequency, amplitude, center, rigidbody.transform.GetInstanceID() / 1000, axis));
		}
		
		public static void OscillatePosition(this Rigidbody2D rigidbody, Vector2 frequency, Vector2 amplitude, string axis = "XY") {
			rigidbody.OscillatePosition(frequency, amplitude, Vector2.zero, axis);
		}

		public static void OscillatePosition(this Rigidbody2D rigidbody, float frequency, float amplitude, float center, string axis = "XY") {
			rigidbody.OscillatePosition(new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), new Vector2(center, center), axis);
		}
		
		public static void OscillatePosition(this Rigidbody2D rigidbody, float frequency, float amplitude, string axis = "XY") {
			rigidbody.OscillatePosition(new Vector2(frequency, frequency), new Vector2(amplitude, amplitude), Vector2.zero, axis);
		}
		#endregion
		
		#region Rotation
		public static void SetEulerAngles(this Rigidbody2D rigidbody, float angle) {
			rigidbody.MoveRotation(angle);
		}
		
		public static void Rotate(this Rigidbody2D rigidbody, float rotation) {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.z + rotation);
		}
			
		public static void RotateTowards(this Rigidbody2D rigidbody, float targetAngle, float speed, InterpolationModes interpolation) {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAngles(new Vector3(targetAngle, targetAngle, targetAngle), Time.fixedDeltaTime * speed, "Z").z);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAnglesLinear(new Vector3(targetAngle, targetAngle, targetAngle), Time.fixedDeltaTime * speed, "Z").z);
					break;
			}
		}
		
		public static void RotateTowards(this Rigidbody2D rigidbody, float targetAngle, float speed) {
			rigidbody.RotateTowards(targetAngle, speed, InterpolationModes.Lerp);
		}

		public static void OscillateEulerAngles(this Rigidbody2D rigidbody, float frequency, float amplitude, float center) {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.OscillateAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), rigidbody.GetInstanceID() / 1000, "Z").z);
		}
		
		public static void OscillateEulerAngles(this Rigidbody2D rigidbody, float frequency, float amplitude) {
			rigidbody.OscillateEulerAngles(frequency, amplitude, 0);
		}
		#endregion
	}
}