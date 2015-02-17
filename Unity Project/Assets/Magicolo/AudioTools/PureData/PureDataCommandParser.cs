using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataCommandParser {

		public PureData pureData;
		
		public PureDataCommandParser(PureData pureData) {
			this.pureData = pureData;
		}
		
		public void ParseCommand(string commandName, object[] arguments) {
			if (commandName == "Play") {
				pureData.itemManager.Play((string)arguments[0], pureData.listener, arguments.Length > 1 ? (float)arguments[1] : 0);
			}
			else if (commandName == "PlayContainer") {
				pureData.itemManager.PlayContainer((string)arguments[0], pureData.listener, arguments.Length > 1 ? (float)arguments[1] : 0);
			}
			else if (commandName == "PlaySequence") {
				pureData.itemManager.PlaySequence((string)arguments[0], pureData.listener, arguments.Length > 1 ? (float)arguments[1] : 0);
			}
			else if (commandName == "SetMasterVolume") {
				pureData.generalSettings.SetMasterVolume((float)arguments[0], arguments.Length > 1 ? (float)arguments[1] : 0, arguments.Length > 2 ? (float)arguments[2] : 0);
			}
			else if (commandName == "SetBusVolume") {
				pureData.busManager.GetBus((string)arguments[0]).SetVolume((float)arguments[1], arguments.Length > 2 ? (float)arguments[2] : 0, arguments.Length > 3 ? (float)arguments[3] : 0);
			}
			else if (commandName == "SetSequenceVolume") {
				pureData.sequenceManager.GetSequence((string)arguments[0]).SetVolume((float)arguments[1], arguments.Length > 2 ? (float)arguments[2] : 0, arguments.Length > 3 ? (float)arguments[3] : 0);
			}
		}
	}
}