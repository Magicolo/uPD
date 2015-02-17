using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataReferences {
		
		public List<Object> references;
		public List<int> ids;
		public int idCounter;
		public PureData pureData;

		public PureDataReferences(PureData pureData) {
			this.pureData = pureData;
			this.references = new List<Object>();
			this.ids = new List<int>();
		}
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
		}
		
		public Object GetObjectWithId(int id) {
			int index = ids.IndexOf(id);
			
			return index == -1 ? null : references[ids.IndexOf(id)];
		}
		
		public T GetObjectWithId<T>(int id) where T : Object {
			return (T)GetObjectWithId(id);
		}
		
		public int AddReference(Object reference) {
			references.Add(reference);
		
			int id = GetUniqueId();
			ids.Add(id);
			
			return id;
		}
		
		public void RemoveReference(Object reference) {
			int index = references.IndexOf(reference);
			
			if (index == -1) {
				return;
			}
			
			references.RemoveAt(index);
			ids.RemoveAt(index);
		}
		
		public void RemoveReference(int id) {
			int index = ids.IndexOf(id);
			
			if (index == -1) {
				return;
			}
			
			references.RemoveAt(index);
			ids.RemoveAt(index);
		}
		
		int GetUniqueId() {
			while (idCounter == 0 || ids.Contains(idCounter)) {
				idCounter += 1;
			}
			
			return idCounter;
		}
		
		public static void Switch(PureDataReferences source, PureDataReferences target) {
			source.references = target.references;
			source.ids = target.ids;
		}
	}
}