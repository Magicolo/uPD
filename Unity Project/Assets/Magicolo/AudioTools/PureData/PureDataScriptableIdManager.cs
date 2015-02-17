using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	public class PureDataScriptableIdManager<T> : ScriptableObject where T : IIdentifiable {

		Dictionary<int, T> idIdentifiableDict = new Dictionary<int, T>();
		Dictionary<int, T> IdIdentifiableDict {
			get {
				idIdentifiableDict = idIdentifiableDict ?? new Dictionary<int, T>();
				return idIdentifiableDict;
			}
		}
		
		int idCounter;
		
		public T GetIdentifiableWithId(int id) {
			return IdIdentifiableDict.ContainsKey(id) ? IdIdentifiableDict[id] : default(T);
		}
		
		public int GetUniqueId() {
			idCounter += 1;
			return idCounter;
		}
		
		public void SetUniqueId(T identifiable) {
			idCounter += 1;
			identifiable.Id = idCounter;
			IdIdentifiableDict[idCounter] = identifiable;
		}
		
		public void SetUniqueIds(IList<T> identifiables) {
			foreach (T identifiable in identifiables) {
				SetUniqueId(identifiable);
			}
		}
		
		public void ResetUniqueIds(IList<T> identifiables) {
			RemoveAllIds();
			SetUniqueIds(identifiables);
		}
		
		public void AddId(int id, T identifiable) {
			IdIdentifiableDict[id] = identifiable;
		}
		
		public void RemoveId(int id) {
			IdIdentifiableDict.Remove(id);
		}
		
		public void RemoveAllIds() {
			IdIdentifiableDict.Clear();
			idCounter = 0;
		}
	}
}

