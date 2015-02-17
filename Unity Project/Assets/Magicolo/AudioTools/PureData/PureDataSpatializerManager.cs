using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataSpatializerManager : ScriptableObject {

		public PureDataSpatializer[] spatializers = new PureDataSpatializer[0];
		public PureData pureData;
		
		Dictionary<string, PureDataSpatializer> nameSpatializerDict;
		
		public void Initialize(PureData pureData) {
			this.pureData = pureData;
			
			foreach (PureDataSpatializer spatializer in spatializers) {
				spatializer.Initialize(pureData);
			}
		}
		
		public void InitializeSpatializers() {
			foreach (PureDataSpatializer spatializer in spatializers) {
				spatializer.Source = null;
			}
		}
		
		public void Start() {
			InitializeSpatializers();
			BuildSpatializerDict();
			
			foreach (PureDataSpatializer spatializer in spatializers) {
				spatializer.Start();
			}
		}
		
		public void Update() {
			foreach (PureDataSpatializer spatializer in spatializers) {
				spatializer.Update();
			}
		}
		
		public void OnDrawGizmos() {
			foreach (PureDataSpatializer spatializer in spatializers) {
				spatializer.OnDrawGizmos();
			}
		}

		public PureDataSpatializer GetSpatializer(string spatializerName) {
			return nameSpatializerDict[spatializerName];
		}
		
		public void BuildSpatializerDict() {
			nameSpatializerDict = new Dictionary<string, PureDataSpatializer>();
			
			foreach (PureDataSpatializer spatializer in spatializers) {
				nameSpatializerDict[spatializer.Name] = spatializer;
			}
		}
				
		public static PureDataSpatializerManager Create(string path) {
			return HelperFunctions.GetOrAddAssetOfType<PureDataSpatializerManager>("Spatializers", path);
		}
	}
}
