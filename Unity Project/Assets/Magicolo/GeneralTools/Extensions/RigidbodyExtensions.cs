using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class RigidbodyExtensions {

		#region Position
		public static void SetPosition(this Rigidbody rigidbody, Vector3 position, string axis = "XYZ") {
			rigidbody.MovePosition(rigidbody.transform.position.SetValues(position, axis));
		}
		
		public static void SetPosition(this Rigidbody rigidbody, float position, string axis = "XYZ") {
			rigidbody.SetPosition(new Vector3(position, position, position), axis);
		}
		
		public static void Translate(this Rigidbody rigidbody, Vector3 translation, string axis = "XYZ") {
			rigidbody.SetPosition(rigidbody.transform.position + translation, axis);
		}
		
		public static void Translate(this Rigidbody rigidbody, float translation, string axis = "XYZ") {
			rigidbody.SetPosition(rigidbody.transform.position + new Vector3(translation, translation, translation), axis);
		}
		
		public static void TranslateTowards(this Rigidbody rigidbody, Vector3 targetPosition, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					rigidbody.SetPosition(rigidbody.transform.position.Lerp(targetPosition, Time.fixedDeltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetPosition(rigidbody.transform.position.LerpLinear(targetPosition, Time.fixedDeltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void TranslateTowards(this Rigidbody rigidbody, Vector3 targetPosition, float speed, string axis = "XYZ") {
			rigidbody.TranslateTowards(targetPosition, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void TranslateTowards(this Rigidbody rigidbody, float targetPosition, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			rigidbody.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, interpolation, axis);
		}
		
		public static void TranslateTowards(this Rigidbody rigidbody, float targetPosition, float speed, string axis = "XYZ") {
			rigidbody.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, InterpolationModes.Lerp, axis);
		}
		
		public static void OscillatePosition(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis = "XYZ") {
			rigidbody.SetPosition(rigidbody.transform.position.Oscillate(frequency, amplitude, center, rigidbody.transform.GetInstanceID() / 1000, axis));
		}
		
		public static void OscillatePosition(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, string axis = "XYZ") {
			rigidbody.OscillatePosition(frequency, amplitude, Vector3.zero, axis);
		}

		public static void OscillatePosition(this Rigidbody rigidbody, float frequency, float amplitude, float center, string axis = "XYZ") {
			rigidbody.OscillatePosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillatePosition(this Rigidbody rigidbody, float frequency, float amplitude, string axis = "XYZ") {
			rigidbody.OscillatePosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.zero, axis);
		}
		#endregion
		
		#region Rotation
		public static void SetEulerAngles(this Rigidbody rigidbody, Vector3 angles, string axis = "XYZ") {
			rigidbody.MoveRotation(Quaternion.Euler(rigidbody.transform.eulerAngles.SetValues(angles, axis)));
		}
		
		public static void SetEulerAngles(this Rigidbody rigidbody, float angle, string axis = "XYZ") {
			rigidbody.SetEulerAngles(new Vector3(angle, angle, angle), axis);
		}
		
		public static void Rotate(this Rigidbody rigidbody, Vector3 rotation, string axis = "XYZ") {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles + rotation, axis);
		}
		
		public static void Rotate(this Rigidbody rigidbody, float rotation, string axis = "XYZ") {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles + new Vector3(rotation, rotation, rotation), axis);
		}
			
		public static void RotateTowards(this Rigidbody rigidbody, Vector3 targetAngles, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAngles(targetAngles, Time.fixedDeltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.LerpAnglesLinear(targetAngles, Time.fixedDeltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void RotateTowards(this Rigidbody rigidbody, Vector3 targetAngles, float speed, string axis = "XYZ") {
			rigidbody.RotateTowards(targetAngles, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void RotateTowards(this Rigidbody rigidbody, float targetAngle, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			rigidbody.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, interpolation, axis);
		}
		
		public static void RotateTowards(this Rigidbody rigidbody, float targetAngle, float speed, string axis = "XYZ") {
			rigidbody.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, InterpolationModes.Lerp, axis);
		}

		public static void OscillateEulerAngles(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis = "XYZ") {
			rigidbody.SetEulerAngles(rigidbody.transform.eulerAngles.OscillateAngles(frequency, amplitude, center, rigidbody.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillateEulerAngles(this Rigidbody rigidbody, Vector3 frequency, Vector3 amplitude, string axis = "XYZ") {
			rigidbody.OscillateEulerAngles(frequency, amplitude, Vector3.zero, axis);
		}

		public static void OscillateEulerAngles(this Rigidbody rigidbody, float frequency, float amplitude, float center, string axis = "XYZ") {
			rigidbody.OscillateEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillateEulerAngles(this Rigidbody rigidbody, float frequency, float amplitude, string axis = "XYZ") {
			rigidbody.OscillateEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.zero, axis);
		}
		#endregion
	}
}

