using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class DelegateExtensions {

		public static bool Contains(this System.Delegate del, System.Type type, string methodName) {
			bool contains = false;
			if (del != null && del.GetInvocationList() != null) {
				contains = !System.Array.TrueForAll(del.GetInvocationList(), invoker => invoker.Method.DeclaringType != type && invoker.Method.Name != methodName);
			}
			return contains;
		}
	
		public static bool Contains(this System.Delegate del, object obj, string methodName) {
			return del.Contains(obj.GetType(), methodName);
		}
	
		public static bool Contains(this System.Delegate del, string methodName) {
			return !System.Array.TrueForAll(del.GetInvocationList(), invoker => invoker.Method.Name != methodName);
		}
	}
}
