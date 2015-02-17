using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class ListExtensions {

		public static T Pop<T>(this List<T> list, int index = 0) {
			if (list == null || list.Count == 0) {
				return default(T);
			}
			
			T item = list[index];
			list.RemoveAt(index);
			return item;
		}
	
		public static T Pop<T>(this List<T> list, T element) {
			return list.Pop(list.IndexOf(element));
		}
	
		public static T PopLast<T>(this List<T> list) {
			return list.Pop(list.Count - 1);
		}
	
		public static T PopRandom<T>(this List<T> list) {
			return list.Pop(Random.Range(0, list.Count));
		}
	
		public static List<T> PopRange<T>(this List<T> list, int startIndex, int count) {
			List<T> popped = new List<T>(count);
		
			for (int i = 0; i < count; i++) {
				popped[i] = list.Pop(i + startIndex);
			}
			return popped;
		}
	
		public static List<T> PopRange<T>(this List<T> list, int count) {
			return list.PopRange(0, count);
		}
		
		public static List<T> Slice<T>(this List<T> list, int startIndex) {
			return list.Slice(startIndex, list.Count - 1);
		}
	
		public static List<T> Slice<T>(this List<T> list, int startIndex, int endIndex) {
			List<T> slicedArray = new List<T>(endIndex - startIndex);
			for (int i = 0; i < endIndex - startIndex; i++) {
				slicedArray[i] = list[i + startIndex];
			}
			return slicedArray;
		}
	}
}
