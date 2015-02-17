using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class TransformExtensions {

		#region Position
		public static void SetPosition(this Transform transform, Vector3 position, string axis = "XYZ") {
			transform.position = transform.position.SetValues(position, axis);
		}
		
		public static void SetPosition(this Transform transform, float position, string axis = "XYZ") {
			transform.SetPosition(new Vector3(position, position, position), axis);
		}
		
		public static void Translate(this Transform transform, Vector3 translation, string axis) {
			transform.SetPosition(transform.position + translation, axis);
		}
		
		public static void Translate(this Transform transform, float translation, string axis = "XYZ") {
			transform.SetPosition(transform.position + new Vector3(translation, translation, translation), axis);
		}
		
		public static void TranslateTowards(this Transform transform, Vector3 targetPosition, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					transform.SetPosition(transform.position.Lerp(targetPosition, Time.deltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					transform.SetPosition(transform.position.LerpLinear(targetPosition, Time.deltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void TranslateTowards(this Transform transform, Vector3 targetPosition, float speed, string axis = "XYZ") {
			transform.TranslateTowards(targetPosition, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void TranslateTowards(this Transform transform, float targetPosition, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			transform.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, interpolation, axis);
		}
		
		public static void TranslateTowards(this Transform transform, float targetPosition, float speed, string axis = "XYZ") {
			transform.TranslateTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, InterpolationModes.Lerp, axis);
		}
		
		public static void OscillatePosition(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis = "XYZ") {
			transform.SetPosition(transform.position.Oscillate(frequency, amplitude, center, transform.GetInstanceID() / 1000, axis));
		}
		
		public static void OscillatePosition(this Transform transform, Vector3 frequency, Vector3 amplitude, string axis = "XYZ") {
			transform.OscillatePosition(frequency, amplitude, Vector3.zero, axis);
		}

		public static void OscillatePosition(this Transform transform, float frequency, float amplitude, float center, string axis = "XYZ") {
			transform.OscillatePosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillatePosition(this Transform transform, float frequency, float amplitude, string axis = "XYZ") {
			transform.OscillatePosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.zero, axis);
		}
		
		public static void SetLocalPosition(this Transform transform, Vector3 position, string axis = "XYZ") {
			transform.localPosition = transform.localPosition.SetValues(position, axis);
		}
		
		public static void SetLocalPosition(this Transform transform, float position, string axis = "XYZ") {
			transform.SetLocalPosition(new Vector3(position, position, position), axis);
		}
		
		public static void TranslateLocal(this Transform transform, Vector3 translation, string axis) {
			transform.SetLocalPosition(transform.localPosition + translation, axis);
		}
		
		public static void TranslateLocal(this Transform transform, float translation, string axis = "XYZ") {
			transform.SetLocalPosition(transform.localPosition + new Vector3(translation, translation, translation), axis);
		}
		
		public static void TranslateLocalTowards(this Transform transform, Vector3 targetPosition, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					transform.SetLocalPosition(transform.localPosition.Lerp(targetPosition, Time.deltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					transform.SetLocalPosition(transform.localPosition.LerpLinear(targetPosition, Time.deltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void TranslateLocalTowards(this Transform transform, Vector3 targetPosition, float speed, string axis = "XYZ") {
			transform.TranslateLocalTowards(targetPosition, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void TranslateLocalTowards(this Transform transform, float targetPosition, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			transform.TranslateLocalTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, interpolation, axis);
		}
		
		public static void TranslateLocalTowards(this Transform transform, float targetPosition, float speed, string axis = "XYZ") {
			transform.TranslateLocalTowards(new Vector3(targetPosition, targetPosition, targetPosition), speed, InterpolationModes.Lerp, axis);
		}
		
		public static void OscillateLocalPosition(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis = "XYZ") {
			transform.SetLocalPosition(transform.localPosition.Oscillate(frequency, amplitude, center, transform.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillateLocalPosition(this Transform transform, Vector3 frequency, Vector3 amplitude, string axis = "XYZ") {
			transform.OscillateLocalPosition(frequency, amplitude, Vector3.zero, axis);
		}

		public static void OscillateLocalPosition(this Transform transform, float frequency, float amplitude, float center, string axis = "XYZ") {
			transform.OscillateLocalPosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillateLocalPosition(this Transform transform, float frequency, float amplitude, string axis = "XYZ") {
			transform.OscillateLocalPosition(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.zero, axis);
		}
		#endregion
		
		#region Rotation
		public static void SetEulerAngles(this Transform transform, Vector3 angles, string axis = "XYZ") {
			transform.eulerAngles = transform.eulerAngles.SetValues(angles, axis);
		}
		
		public static void SetEulerAngles(this Transform transform, float angle, string axis = "XYZ") {
			transform.SetEulerAngles(new Vector3(angle, angle, angle), axis);
		}
		
		public static void Rotate(this Transform transform, Vector3 rotation, string axis) {
			transform.SetEulerAngles(transform.eulerAngles + rotation, axis);
		}
		
		public static void Rotate(this Transform transform, float rotation, string axis = "XYZ") {
			transform.SetEulerAngles(transform.eulerAngles + new Vector3(rotation, rotation, rotation), axis);
		}
			
		public static void RotateTowards(this Transform transform, Vector3 targetAngles, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					transform.SetEulerAngles(transform.eulerAngles.LerpAngles(targetAngles, Time.deltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					transform.SetEulerAngles(transform.eulerAngles.LerpAnglesLinear(targetAngles, Time.deltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void RotateTowards(this Transform transform, Vector3 targetAngles, float speed, string axis = "XYZ") {
			transform.RotateTowards(targetAngles, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void RotateTowards(this Transform transform, float targetAngle, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			transform.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, interpolation, axis);
		}
		
		public static void RotateTowards(this Transform transform, float targetAngle, float speed, string axis = "XYZ") {
			transform.RotateTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, InterpolationModes.Lerp, axis);
		}

		public static void OscillateEulerAngles(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis = "XYZ") {
			transform.SetEulerAngles(transform.eulerAngles.OscillateAngles(frequency, amplitude, center, transform.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillateEulerAngles(this Transform transform, Vector3 frequency, Vector3 amplitude, string axis = "XYZ") {
			transform.OscillateEulerAngles(frequency, amplitude, Vector3.zero, axis);
		}

		public static void OscillateEulerAngles(this Transform transform, float frequency, float amplitude, float center, string axis = "XYZ") {
			transform.OscillateEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillateEulerAngles(this Transform transform, float frequency, float amplitude, string axis = "XYZ") {
			transform.OscillateEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.zero, axis);
		}

		public static void SetLocalEulerAngles(this Transform transform, Vector3 angles, string axis = "XYZ") {
			transform.localEulerAngles = transform.localEulerAngles.SetValues(angles, axis);
		}
		
		public static void SetLocalEulerAngles(this Transform transform, float angle, string axis = "XYZ") {
			transform.SetLocalEulerAngles(new Vector3(angle, angle, angle), axis);
		}
		
		public static void RotateLocal(this Transform transform, Vector3 rotation, string axis = "XYZ") {
			transform.SetLocalEulerAngles(transform.localEulerAngles + rotation, axis);
		}
		
		public static void RotateLocal(this Transform transform, float rotation, string axis = "XYZ") {
			transform.SetLocalEulerAngles(transform.localEulerAngles + new Vector3(rotation, rotation, rotation), axis);
		}
			
		public static void RotateLocalTowards(this Transform transform, Vector3 targetAngles, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					transform.SetLocalEulerAngles(transform.localEulerAngles.LerpAngles(targetAngles, Time.deltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					transform.SetLocalEulerAngles(transform.localEulerAngles.LerpAnglesLinear(targetAngles, Time.deltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void RotateLocalTowards(this Transform transform, Vector3 targetAngles, float speed, string axis = "XYZ") {
			transform.RotateLocalTowards(targetAngles, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void RotateLocalTowards(this Transform transform, float targetAngle, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			transform.RotateLocalTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, interpolation, axis);
		}
		
		public static void RotateLocalTowards(this Transform transform, float targetAngle, float speed, string axis = "XYZ") {
			transform.RotateLocalTowards(new Vector3(targetAngle, targetAngle, targetAngle), speed, InterpolationModes.Lerp, axis);
		}

		public static void OscillateLocalEulerAngles(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis = "XYZ") {
			transform.SetLocalEulerAngles(transform.localEulerAngles.OscillateAngles(frequency, amplitude, center, transform.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillateLocalEulerAngles(this Transform transform, Vector3 frequency, Vector3 amplitude, string axis = "XYZ") {
			transform.OscillateLocalEulerAngles(frequency, amplitude, Vector3.one, axis);
		}

		public static void OscillateLocalEulerAngles(this Transform transform, float frequency, float amplitude, float center, string axis = "XYZ") {
			transform.OscillateLocalEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillateLocalEulerAngles(this Transform transform, float frequency, float amplitude, string axis = "XYZ") {
			transform.OscillateLocalEulerAngles(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.one, axis);
		}

		public static Quaternion LookingAt2D(this Transform transform, Vector3 target, float angleOffset, float damping = 100) {
			Vector3 targetDirection = (target - transform.position).normalized;
			float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + angleOffset;
			return Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), damping * Time.deltaTime);
		}
		
		public static Quaternion LookingAt2D(this Transform transform, Transform target, float angleOffset, float damping = 100) {
			return transform.LookingAt2D(target.position, angleOffset, damping);
		}
		
		public static Quaternion LookingAt2D(this Transform transform, Vector3 target) {
			return transform.LookingAt2D(target, 0, 100);
		}
		
		public static Quaternion LookingAt2D(this Transform transform, Transform target) {
			return transform.LookingAt2D(target.position, 0, 100);
		}
		
		public static void LookAt2D(this Transform transform, Vector3 target, float angleOffset, float damping = 100) {
			transform.rotation = transform.LookingAt2D(target, angleOffset, damping);
		}
		
		public static void LookAt2D(this Transform transform, Transform target, float angleOffset, float damping = 100) {
			transform.LookAt2D(target.position, angleOffset, damping);
		}
		
		public static void LookAt2D(this Transform transform, Vector3 target) {
			transform.LookAt2D(target, 0, 100);
		}
		
		public static void LookAt2D(this Transform transform, Transform target) {
			transform.LookAt2D(target.position, 0, 100);
		}
		#endregion
		
		#region Scale
		public static void SetScale(this Transform transform, Vector3 scale, string axis = "XYZ") {
			Vector3 newScale = transform.localScale;
			
			if (axis.Contains("X")) newScale.x = (newScale.x / transform.lossyScale.x) * scale.x;
			if (axis.Contains("Y")) newScale.y = (newScale.y / transform.lossyScale.y) * scale.y;
			if (axis.Contains("Z")) newScale.z = (newScale.z / transform.lossyScale.z) * scale.z;
			
			transform.localScale = newScale;
		}
		
		public static void SetScale(this Transform transform, float scale, string axis = "XYZ") {
			transform.SetScale(new Vector3(scale, scale, scale), axis);
		}
		
		public static void Scale(this Transform transform, Vector3 scale, string axis = "XYZ") {
			transform.SetScale(transform.localScale + scale, axis);
		}
		
		public static void Scale(this Transform transform, float scale, string axis = "XYZ") {
			transform.SetScale(new Vector3(scale, scale, scale), axis);
		}
	
		public static void ScaleTowards(this Transform transform, Vector3 targetScale, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					transform.SetScale(transform.lossyScale.Lerp(targetScale, Time.deltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					transform.SetScale(transform.lossyScale.LerpLinear(targetScale, Time.deltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void ScaleTowards(this Transform transform, Vector3 targetScale, float speed, string axis = "XYZ") {
			transform.ScaleTowards(targetScale, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void ScaleTowards(this Transform transform, float targetScale, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			transform.ScaleTowards(new Vector3(targetScale, targetScale, targetScale), speed, interpolation, axis);
		}
		
		public static void ScaleTowards(this Transform transform, float targetScale, float speed, string axis = "XYZ") {
			transform.ScaleTowards(new Vector3(targetScale, targetScale, targetScale), speed, InterpolationModes.Lerp, axis);
		}

		public static void OscillateScale(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis = "XYZ") {
			transform.SetScale(transform.lossyScale.Oscillate(frequency, amplitude, center, transform.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillateScale(this Transform transform, Vector3 frequency, Vector3 amplitude, string axis = "XYZ") {
			transform.OscillateScale(frequency, amplitude, Vector3.one, axis);
		}

		public static void OscillateScale(this Transform transform, float frequency, float amplitude, float center, string axis = "XYZ") {
			transform.OscillateScale(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillateScale(this Transform transform, float frequency, float amplitude, string axis = "XYZ") {
			transform.OscillateScale(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.one, axis);
		}
		
		public static void FlipScale(this Transform transform, string axis = "XYZ") {
			transform.SetScale(transform.lossyScale.SetValues(-transform.lossyScale, axis), axis);
		}
		
		public static void SetLocalScale(this Transform transform, Vector3 scale, string axis = "XYZ") {
			transform.localScale = transform.localScale.SetValues(scale, axis);
		}
		
		public static void SetLocalScale(this Transform transform, float scale, string axis = "XYZ") {
			transform.SetLocalScale(new Vector3(scale, scale, scale), axis);
		}
		
		public static void ScaleLocal(this Transform transform, Vector3 scale, string axis = "XYZ") {
			transform.SetLocalScale(transform.localScale + scale, axis);
		}
		
		public static void ScaleLocal(this Transform transform, float scale, string axis = "XYZ") {
			transform.ScaleLocal(new Vector3(scale, scale, scale), axis);
		}
			
		public static void ScaleLocalTowards(this Transform transform, Vector3 targetScale, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			switch (interpolation) {
				case InterpolationModes.Lerp:
					transform.SetLocalScale(transform.localScale.Lerp(targetScale, Time.deltaTime * speed, axis), axis);
					break;
				case InterpolationModes.Linear:
					transform.SetLocalScale(transform.localScale.LerpLinear(targetScale, Time.deltaTime * speed, axis), axis);
					break;
			}
		}
		
		public static void ScaleLocalTowards(this Transform transform, Vector3 targetScale, float speed, string axis = "XYZ") {
			transform.ScaleLocalTowards(targetScale, speed, InterpolationModes.Lerp, axis);
		}
		
		public static void ScaleLocalTowards(this Transform transform, float targetScale, float speed, InterpolationModes interpolation, string axis = "XYZ") {
			transform.ScaleLocalTowards(new Vector3(targetScale, targetScale, targetScale), speed, interpolation, axis);
		}
		
		public static void ScaleLocalTowards(this Transform transform, float targetScale, float speed, string axis = "XYZ") {
			transform.ScaleLocalTowards(new Vector3(targetScale, targetScale, targetScale), speed, InterpolationModes.Lerp, axis);
		}

		public static void OscillateLocalScale(this Transform transform, Vector3 frequency, Vector3 amplitude, Vector3 center, string axis = "XYZ") {
			transform.SetLocalScale(transform.localScale.Oscillate(frequency, amplitude, center, transform.GetInstanceID() / 1000, axis), axis);
		}
		
		public static void OscillateLocalScale(this Transform transform, Vector3 frequency, Vector3 amplitude, string axis = "XYZ") {
			transform.OscillateLocalScale(frequency, amplitude, Vector3.one, axis);
		}

		public static void OscillateLocalScale(this Transform transform, float frequency, float amplitude, float center, string axis = "XYZ") {
			transform.OscillateLocalScale(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), new Vector3(center, center, center), axis);
		}
		
		public static void OscillateLocalScale(this Transform transform, float frequency, float amplitude, string axis = "XYZ") {
			transform.OscillateLocalScale(new Vector3(frequency, frequency, frequency), new Vector3(amplitude, amplitude, amplitude), Vector3.one, axis);
		}

		public static void FlipLocalScale(this Transform transform, string axis = "XYZ") {
			transform.SetLocalScale(transform.localScale.SetValues(-transform.localScale, axis), axis);
		}
		#endregion
		
		public static Transform[] GetChildren(this Transform parent) {
			List<Transform> children = new List<Transform>();
			if (parent != null) {
				if (parent.childCount > 0) {
					for (int i = 0; i < parent.childCount; i++) {
						Transform child = parent.GetChild(i);
						children.Add(child);
					}
				}
			}
			return children.ToArray();
		}
		
		public static Transform[] GetChildrenRecursive(this Transform parent) {
			List<Transform> children = new List<Transform>();
			if (parent != null) {
				foreach (Transform child in parent.GetChildren()) {
					children.Add(child);
					if (child.childCount > 0) {
						children.AddRange(child.GetChildrenRecursive());
					}
				}
			}
			return children.ToArray();
		}
		
		public static Transform FindChildRecursive(this Transform parent, string childName) {
			foreach (Transform child in parent.GetChildrenRecursive()) {
				if (child.name == childName) {
					return child;
				}
			}
			return null;
		}
		
		public static Transform AddChild(this Transform parent, string childName, PrimitiveType primitiveType) {
			GameObject child = GameObject.CreatePrimitive(primitiveType);
			
			if (!string.IsNullOrEmpty(childName)) {
				child.name = childName;
			}
			
			child.transform.parent = parent;
			child.transform.Reset();
			return child.transform;
		}
		
		public static Transform AddChild(this Transform parent, string childName) {
			GameObject child = new GameObject();
			
			if (!string.IsNullOrEmpty(childName)) {
				child.name = childName;
			}
			
			child.transform.parent = parent;
			child.transform.Reset();
			return child.transform;
		}
		
		public static Transform FindOrAddChild(this Transform parent, string childName, PrimitiveType primitiveType) {
			Transform child = parent.FindChild(childName) ?? parent.AddChild(childName, primitiveType);
			return child;
		}
		
		public static Transform FindOrAddChild(this Transform parent, string childName) {
			Transform child = parent.FindChild(childName) ?? parent.AddChild(childName);
			return child;
		}
		
		public static void SortChildren(this Transform parent) {
			Transform[] children = parent.GetChildren();
			List<string> childrendNames = new List<string>();
			
			foreach (Transform child in children) {
				childrendNames.Add(child.name);
			}
			
			Array.Sort(childrendNames.ToArray(), children);
			
			foreach (Transform child in children) {
				child.parent = null;
				child.parent = parent;
			}
		}
		
		public static void SortChildrenRecursive(this Transform parent) {
			parent.SortChildren();
			foreach (Transform child in parent.GetChildren()) {
				if (child.childCount > 0) child.SortChildrenRecursive();
			}
		}
		
		public static void SetChildrenActive(this Transform parent, bool value) {
			foreach (Transform child in parent.GetChildren()) {
				child.gameObject.SetActive(value);
			}
		}
		
		public static void DestroyChildren(this Transform parent) {
			foreach (Transform child in parent.GetChildren()) {
				if (Application.isPlaying) {
					UnityEngine.Object.Destroy(child.gameObject);
				}
				else {
					UnityEngine.Object.DestroyImmediate(child.gameObject);
				}
			}
		}
		
		public static void DestroyChildrenImmediate(this Transform parent) {
			foreach (Transform child in parent.GetChildren()) {
				UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
		
		public static void Reset(this Transform transform) {
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
	}
}
