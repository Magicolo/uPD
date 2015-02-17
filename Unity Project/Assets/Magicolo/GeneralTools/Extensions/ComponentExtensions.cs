using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class ComponentExtensions {

		public static T AddComponent<T>(this Component component) where T : Component {
			return component.gameObject.AddComponent<T>();
		}
	
		public static T GetOrAddComponent<T>(this Component component) where T : Component {
			return component.gameObject.GetOrAddComponent<T>();
		}
	
		public static int GetHierarchyDepth(this Component component) {
			int depth = 0;
			Transform currentTransform = component.transform;
		
			while (currentTransform.parent != null) {
				currentTransform = currentTransform.parent;
				depth += 1;
			}
		
			return depth;
		}
	
		public static GameObject[] GetChildren(this Component parent) {
			var children = new List<GameObject>();
			foreach (var child in parent.transform.GetChildren()) {
				children.Add(child.gameObject);
			}
			return children.ToArray();
		}
	
		public static GameObject[] GetChildrenRecursive(this Component parent) {
			var children = new List<GameObject>();
			foreach (var child in parent.transform.GetChildrenRecursive()) {
				children.Add(child.gameObject);
			}
			return children.ToArray();
		}
	
		public static int GetChildCount(this Component parent) {
			return parent.transform.childCount;
		}
	
		public static void SortChildren(this Component parent) {
			parent.transform.SortChildren();
		}
	
		public static void SortChildrenRecursive(this Component parent) {
			parent.transform.SortChildrenRecursive();
		}

		public static GameObject GetChild(this Component parent, int index) {
			return parent.transform.GetChild(index).gameObject;
		}
	
		public static GameObject FindChild(this Component parent, string childName) {
			foreach (var child in parent.transform.GetChildren()) {
				if (child.name == childName) return child.gameObject;
			}
			return null;
		}

		public static GameObject FindChildRecursive(this Component parent, string childName) {
			foreach (var child in parent.transform.GetChildrenRecursive()) {
				if (child.name == childName) return child.gameObject;
			}
			return null;
		}
	
		public static GameObject AddChild(this Component parent, string childName, PrimitiveType primitiveType) {
			return parent.transform.AddChild(childName, primitiveType).gameObject;
		}
			
		public static GameObject AddChild(this Component parent, string childName) {
			return parent.transform.AddChild(childName).gameObject;
		}
		
		public static GameObject FindOrAddChild(this Component parent, string childName, PrimitiveType primitiveType) {
			return parent.transform.FindOrAddChild(childName, primitiveType).gameObject;
		}
	
		public static GameObject FindOrAddChild(this Component parent, string childName) {
			return parent.transform.FindOrAddChild(childName).gameObject;
		}
	
		public static void SetChildrenActive(this Component parent, bool value) {
			parent.transform.SetChildrenActive(value);
		}
	
		public static void DestroyChildren(this Component parent) {
			parent.transform.DestroyChildren();
		}

		public static void RemoveComponent<T>(this Component component) where T : Component {
			T toRemove = component.GetComponent<T>();
			if (toRemove != null) {
				toRemove.Remove();
			}
		}

		public static T GetClosest<T>(this Component source, IList<T> targets) where T : Component {
			float closestDistance = 1000000;
			T closestTarget = default(T);

			foreach (T target in targets) {
				float distance = Vector3.Distance(source.transform.position, target.transform.position);
				if (distance < closestDistance) {
					closestTarget = target;
					closestDistance = distance;
				}
			}
			return closestTarget;
		}

		public static T[] GetComponents<T>(this IList<Component> components) where T : Component {
			T[] componentArray = new T[components.Count];
		
			for (int i = 0; i < components.Count; i++) {
				componentArray[i] = components[i].GetComponent<T>();
			}
			return componentArray;
		}
	}
}
