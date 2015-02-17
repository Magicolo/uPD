using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public static class PureDataSwitcher {

		public static void Switch(PureData source, PureData target) {
			PureDataInfoManager.Switch(source.infoManager, target.infoManager);
			source.clipManager.InitializeClips();
			PureDataReferences.Switch(source.references, target.references);
			source.spatializerManager.InitializeSpatializers();
		}
	}
}

