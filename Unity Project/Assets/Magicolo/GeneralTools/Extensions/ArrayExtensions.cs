using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class ArrayExtensions {

		public static bool Contains<T>(this T[] array, T value) {
			return array.Any(t => object.Equals(t, value));
		}
	
		public static bool Contains<T>(this T[] array, Type type) {
			return typeof(T) == typeof(Type) ? array.Any(t => object.Equals(t, type)) : array.Any(t => t.GetType() == type);
		}
	
		public static void Clear<T>(this T[] array) {
			Array.Clear(array, 0, array.Length);
		}
	
		public static T Pop<T>(this T[] array, int index, out T[] remaining) {
			List<T> list = new List<T>(array);
			T item = list.Pop(index);
			remaining = list.ToArray();
			return item;
		}
	
		public static T Pop<T>(this T[] array, T element, out T[] remaining) {
			return array.Pop(Array.IndexOf(array, element), out remaining);
		}
	
		public static T Pop<T>(this T[] array, out T[] remaining) {
			return array.Pop(0, out remaining);
		}
	
		public static T PopRandom<T>(this T[] array, out T[] remaining) {
			return array.Pop(UnityEngine.Random.Range(0, array.Length), out remaining);
		}
	
		public static T[] PopRange<T>(this T[] array, int startIndex, int count, out T[] remaining) {
			List<T> list = new List<T>(array);
			T[] popped = list.PopRange(startIndex, count).ToArray();
			remaining = list.ToArray();
			return popped;
		}
	
		public static T[] PopRange<T>(this T[] array, int count, out T[] remaining) {
			return array.PopRange(0, count, out remaining);
		}
	
		public static T First<T>(this IList<T> array) {
			return array != null && array.Count > 0 ? array[0] : default(T);
		}
	
		public static T Last<T>(this IList<T> array) {
			return array != null && array.Count > 0 ? array[array.Count - 1] : default(T);
		}
	
		public static T GetRandom<T>(this IList<T> array) {
			if (array == null || array.Count == 0) return default(T);
		
			return array[UnityEngine.Random.Range(0, array.Count)];
		}
	
		public static void Move<T>(this IList<T> array, int sourceIndex, int targetIndex) {
			int delta = Mathf.Abs(targetIndex - sourceIndex);
			
			if (delta == 0) {
				return;
			}
			
			int direction = (targetIndex - sourceIndex) / delta;
		
			for (int i = 0; i < delta; i++) {
				T sourceObject = array[sourceIndex];
				T targetObject = array[sourceIndex + direction];
				array[sourceIndex + direction] = sourceObject;
				array[sourceIndex] = targetObject;
				sourceIndex += direction;
			}
		}
	
		public static T[] Slice<T>(this T[] array, int startIndex) {
			return array.Slice(startIndex, array.Length - startIndex);
		}
	
		public static T[] Slice<T>(this T[] array, int startIndex, int count) {
			T[] slicedArray = new T[count];
			
			for (int i = 0; i < count; i++) {
				slicedArray[i] = array[i + startIndex];
			}
			
			return slicedArray;
		}
				
		public static void ForEachReversed<T>(this IList<T> array, Action<T> action) {
			for (int i = array.Count - 1; i >= 0; i--) {
				action(array[i]);
			}
		}
		
		public static void Reverse<T>(this IList<T> array) {
			for (int i = 0; i < array.Count / 2; i++) {
				T temp = array[i];
				array[i] = array[array.Count - i - 1];
				array[array.Count - i - 1] = temp;
			}
		}

		public static bool ContentEquals(this IList array, IList otherArray) {
			if (otherArray == null || array.Count != otherArray.Count) {
				return false;
			}
		
			for (int i = 0; i < array.Count; i++) {
				if (!Equals(array[i], otherArray[i])) {
					return false;
				}
			}
			return true;
		}
	
		public static int[] ToIntArray<T>(this IList<T> array) {
			int[] intArray = new int[array.Count];
			for (int i = 0; i < array.Count; i++) {
				intArray[i] = array[i].GetHashCode();
			}
			return intArray;
		}
	
		public static float[] ToFloatArray<T>(this IList<T> array) {
			float[] floatArray = new float[array.Count];
			for (int i = 0; i < array.Count; i++) {
				floatArray[i] = (float)(array[i].GetHashCode());
			}
			return floatArray;
		}
	
		public static double[] ToDoubleArray<T>(this IList<T> array) {
			double[] doubleArray = new double[array.Count];
			for (int i = 0; i < array.Count; i++) {
				doubleArray[i] = (double)(array[i].GetHashCode());
			}
			return doubleArray;
		}
	
		public static string[] ToStringArray<T>(this IList<T> array) {
			string[] stringArray = new string[array.Count];
			for (int i = 0; i < array.Count; i++) {
				stringArray[i] = array[i].ToString();
			}
			return stringArray;
		}
	}
}
