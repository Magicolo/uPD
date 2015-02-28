using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class DictionaryExtensions {

		public static void SwitchKeys<T, U>(this IDictionary<T, U> dictionary, T key1, T key2) {
			U value1 = dictionary.ContainsKey(key1) ? dictionary[key1] : default(U);
			U value2 = dictionary.ContainsKey(key2) ? dictionary[key2] : default(U);
			dictionary[key1] = value2;
			dictionary[key2] = value1;
		}
	
		public static T GetRandomKey<T, U>(this IDictionary<T, U> dictionary) {
			return new List<T>(dictionary.Keys).GetRandom();
		}
	
		public static U GetRandomValue<T, U>(this IDictionary<T, U> dictionary) {
			return new List<U>(dictionary.Values).GetRandom();
		}
	
		public static void GetOrderedKeysValues<T, U>(this IDictionary<T, U> dictionary, out List<T> keys, out List<U> values) {
			keys = new List<T>(dictionary.Keys);
			values = new List<U>();
			for (int i = 0; i < keys.Count; i++) {
				values.Add(dictionary[keys[i]]);
			}
		}
	
		public static void GetOrderedKeysValues<T, U>(this IDictionary<T, U> dictionary, out T[] keys, out U[] values) {
			List<T> keyList = new List<T>();
			List<U> valueList = new List<U>();
			dictionary.GetOrderedKeysValues(out keyList, out valueList);
			keys = keyList.ToArray();
			values = valueList.ToArray();
		}
		
		public static T[] GetKeyArray<T, U>(this IDictionary<T, U> dictionary) {
			return new List<T>(dictionary.Keys).ToArray();
		}
		
		public static U[] GetValueArray<T, U>(this IDictionary<T, U> dictionary) {
			return new List<U>(dictionary.Values).ToArray();
		}
	}
}
