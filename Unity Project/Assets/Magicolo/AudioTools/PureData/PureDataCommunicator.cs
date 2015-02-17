using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using LibPDBinding;
using Magicolo.GeneralTools;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataCommunicator {
		
		public PureData pureData;

		Queue<PureDataReceiver> queuedReceivers = new Queue<PureDataReceiver>();
		Dictionary<string, List<PureDataBangReceiver>> sendNameBangReceiverDict = new Dictionary<string, List<PureDataBangReceiver>>();
		Dictionary<string, List<PureDataFloatReceiver>> sendNameFloatReceiverDict = new Dictionary<string, List<PureDataFloatReceiver>>();
		Dictionary<string, List<PureDataSymbolReceiver>> sendNameSymbolReceiverDict = new Dictionary<string, List<PureDataSymbolReceiver>>();
		Dictionary<string, List<PureDataListReceiver>> sendNameListReceiverDict = new Dictionary<string, List<PureDataListReceiver>>();
		Dictionary<string, List<PureDataMessageReceiver>> sendNameMessageReceiverDict = new Dictionary<string, List<PureDataMessageReceiver>>();

		public PureDataCommunicator(PureData pureData) {
			this.pureData = pureData;
		}
	
		public void Initialize() {
			Send("BufferSize", pureData.bridge.bufferSize);
			Send("BufferAmount", pureData.bridge.bufferAmount);
			Send("SampleRate", pureData.bridge.sampleRate);
			Send("uresources_switch", 1);
			Send("umastervolume", pureData.generalSettings.MasterVolume);
			Send("uloadbang");
		}
	
		public void Update() {
			for (int i = queuedReceivers.Count - 1; i >= 0; i--) {
				queuedReceivers.Dequeue().Dequeue();
			}
		}
		
		public void SubscribeDebug() {
			LibPD.Subscribe("Debug");
			LibPD.Subscribe("Command");
			LibPD.Bang += ReceiveBang;
			LibPD.Float += ReceiveFloat;
			LibPD.Symbol += ReceiveSymbol;
			LibPD.List += ReceiveList;
			LibPD.Message += ReceiveMessage;
		}
	
		public void UnsubscribeDebug() {
			LibPD.Bang -= ReceiveBang;
			LibPD.Float -= ReceiveFloat;
			LibPD.Symbol -= ReceiveSymbol;
			LibPD.List -= ReceiveList;
			LibPD.Message -= ReceiveMessage;
		}
			
		#region Send
		public bool Send<T>(string receiverName, params T[] toSend) {
			return Send(receiverName, toSend as object);
		}
			
		public bool Send(string receiverName, params object[] toSend) {
			return Send(receiverName, toSend as object);
		}
		
		public bool Send(string receiverName, object toSend) {
			int success = -1;
			
			if (toSend is IList && ((IList)toSend).Count == 1) {
				toSend = ((IList)toSend)[0];
			}
			
			if (toSend is int) success = LibPD.SendFloat(receiverName, (float)((int)toSend));
			else if (toSend is int[]) success = LibPD.SendList(receiverName, ((int[])toSend).ToFloatArray());
			else if (toSend is float) success = LibPD.SendFloat(receiverName, (float)toSend);
			else if (toSend is float[]) success = LibPD.SendList(receiverName, (float[])toSend);
			else if (toSend is double) success = LibPD.SendFloat(receiverName, (float)((double)toSend));
			else if (toSend is double[]) success = LibPD.SendList(receiverName, ((double[])toSend).ToFloatArray());
			else if (toSend is bool) success = LibPD.SendFloat(receiverName, (float)((bool)toSend).GetHashCode());
			else if (toSend is bool[]) success = LibPD.SendList(receiverName, ((bool[])toSend).ToFloatArray());
			else if (toSend is char) success = LibPD.SendSymbol(receiverName, ((char)toSend).ToString());
			else if (toSend is char[]) success = LibPD.SendSymbol(receiverName, new string((char[])toSend));
			else if (toSend is string) success = LibPD.SendSymbol(receiverName, (string)toSend);
			else if (toSend is string[]) success = LibPD.SendList(receiverName, (string[])toSend);
			else if (toSend is System.Enum) success = LibPD.SendFloat(receiverName, (float)(toSend.GetHashCode()));
			else if (toSend is System.Enum[]) success = LibPD.SendList(receiverName, ((System.Enum[])toSend).ToFloatArray());
			else if (toSend is object[]) success = LibPD.SendList(receiverName, (object[])toSend);
			else if (toSend is Vector2) success = LibPD.SendList(receiverName, ((Vector2)toSend).x, ((Vector2)toSend).y);
			else if (toSend is Vector3) success = LibPD.SendList(receiverName, ((Vector3)toSend).x, ((Vector3)toSend).y, ((Vector3)toSend).z);
			else if (toSend is Vector4) success = LibPD.SendList(receiverName, ((Vector4)toSend).x, ((Vector4)toSend).y, ((Vector4)toSend).z, ((Vector4)toSend).w);
			else if (toSend is Quaternion) success = LibPD.SendList(receiverName, ((Quaternion)toSend).x, ((Quaternion)toSend).y, ((Quaternion)toSend).z, ((Quaternion)toSend).w);
			else if (toSend is Rect) success = LibPD.SendList(receiverName, ((Rect)toSend).x, ((Rect)toSend).y, ((Rect)toSend).width, ((Rect)toSend).height);
			else if (toSend is Bounds) success = LibPD.SendList(receiverName, ((Bounds)toSend).center.x, ((Bounds)toSend).center.y, ((Bounds)toSend).size.x, ((Bounds)toSend).size.y);
			else if (toSend is Color) success = LibPD.SendList(receiverName, ((Color)toSend).r, ((Color)toSend).g, ((Color)toSend).b, ((Color)toSend).a);
			else {
				Logger.LogError("Invalid type to send to Pure Data: " + toSend);
			}
			
			return success == 0;
		}
	
		public bool SendBang(string receiverName) {
			return LibPD.SendBang(receiverName) == 0;
		}
	
		public bool SendMessage<T>(string receiverName, string message, params T[] arguments) {
			return LibPD.SendMessage(receiverName, message, arguments) == 0;
		}
	
		public bool SendAftertouch(int channel, int value) {
			return LibPD.SendAftertouch(channel, value) == 0;
		}
	
		public bool SendControlChange(int channel, int controller, int value) {
			return LibPD.SendControlChange(channel, controller, value) == 0;
		}
	
		public bool SendMidiByte(int port, int value) {
			return LibPD.SendMidiByte(port, value) == 0;
		}
	
		public bool SendNoteOn(int channel, int pitch, int velocity) {
			return LibPD.SendNoteOn(channel, pitch, velocity) == 0;
		}
	
		public bool SendPitchbend(int channel, int value) {
			return LibPD.SendPitchbend(channel, value) == 0;
		}
	
		public bool SendPolyAftertouch(int channel, int pitch, int value) {
			return LibPD.SendPolyAftertouch(channel, pitch, value) == 0;
		}
	
		public bool SendProgramChange(int channel, int value) {
			return LibPD.SendProgramChange(channel, value) == 0;
		}
	
		public bool SendSysex(int port, int value) {
			return LibPD.SendSysex(port, value) == 0;
		}
	
		public bool SendSysRealtime(int port, int value) {
			return LibPD.SendSysRealtime(port, value) == 0;
		}
		
		public void SendDelayedMessage(string sendName, float delay, float arg1) {
			if (delay > 0) {
				Send("uresources_messagedelayer_f", arg1, sendName, delay * 1000);
			}
			else {
				Send(sendName, arg1);
			}
		}
		
		public void SendDelayedMessage(string sendName, float delay, float arg1, float arg2) {
			if (delay > 0) {
				Send("uresources_messagedelayer_ff", arg1, arg2, sendName, delay * 1000);
			}
			else {
				Send(sendName, arg1, arg2);
			}
		}
		
		public void SendDelayedMessage(string sendName, float delay, bool arg1) {
			SendDelayedMessage(sendName, delay, arg1.GetHashCode());
		}
		
		public void SendDelayedMessage(string sendName, float delay, string arg1) {
			if (delay > 0) {
				Send("uresources_messagedelayer_s", arg1, sendName, delay * 1000);
			}
			else {
				Send(sendName, arg1);
			}
		}
		#endregion

		#region Receive
		public void Receive(string sendName, BangReceiveCallback bangReceiver, bool asynchronous = false) {
			if (!sendNameBangReceiverDict.ContainsKey(sendName)) {
				LibPD.Subscribe(sendName);
				sendNameBangReceiverDict[sendName] = new List<PureDataBangReceiver>();
			}
			sendNameBangReceiverDict[sendName].Add(new PureDataBangReceiver(sendName, bangReceiver, asynchronous, pureData));
		}
		
		public void Receive(string sendName, FloatReceiveCallback floatReceiver, bool asynchronous = false) {
			if (!sendNameFloatReceiverDict.ContainsKey(sendName)) {
				LibPD.Subscribe(sendName);
				sendNameFloatReceiverDict[sendName] = new List<PureDataFloatReceiver>();
			}
			sendNameFloatReceiverDict[sendName].Add(new PureDataFloatReceiver(sendName, floatReceiver, asynchronous, pureData));
		}
		
		public void Receive(string sendName, SymbolReceiveCallback symbolReceiver, bool asynchronous = false) {
			if (!sendNameSymbolReceiverDict.ContainsKey(sendName)) {
				LibPD.Subscribe(sendName);
				sendNameSymbolReceiverDict[sendName] = new List<PureDataSymbolReceiver>();
			}
			sendNameSymbolReceiverDict[sendName].Add(new PureDataSymbolReceiver(sendName, symbolReceiver, asynchronous, pureData));
		}
		
		public void Receive(string sendName, ListReceiveCallback listReceiver, bool asynchronous = false) {
			if (!sendNameListReceiverDict.ContainsKey(sendName)) {
				LibPD.Subscribe(sendName);
				sendNameListReceiverDict[sendName] = new List<PureDataListReceiver>();
			}
			sendNameListReceiverDict[sendName].Add(new PureDataListReceiver(sendName, listReceiver, asynchronous, pureData));
		}
		
		public void Receive(string sendName, MessageReceiveCallback messageReceiver, bool asynchronous = false) {
			if (!sendNameMessageReceiverDict.ContainsKey(sendName)) {
				LibPD.Subscribe(sendName);
				sendNameMessageReceiverDict[sendName] = new List<PureDataMessageReceiver>();
			}
			sendNameMessageReceiverDict[sendName].Add(new PureDataMessageReceiver(sendName, messageReceiver, asynchronous, pureData));
		}
		
		void ReceiveBang(string sendName) {
			if (sendName == "Debug") {
				Logger.Log(string.Format("{0} received Bang", sendName));
			}
			else if (sendNameBangReceiverDict.ContainsKey(sendName)) {
				for (int i = sendNameBangReceiverDict[sendName].Count - 1; i >= 0; i--) {
					PureDataBangReceiver receiver = sendNameBangReceiverDict[sendName][i];
					
					if (receiver.asynchronous) {
						receiver.Receive();
					}
					else {
						receiver.Enqueue();
						queuedReceivers.Enqueue(receiver);
					}
				}
			}
		}
	
		void ReceiveFloat(string sendName, float value) {
			if (sendName == "Debug") {
				Logger.Log(string.Format("{0} received Float: {1}", sendName, value));
			}
			else if (sendNameFloatReceiverDict.ContainsKey(sendName)) {
				for (int i = sendNameFloatReceiverDict[sendName].Count - 1; i >= 0; i--) {
					PureDataFloatReceiver receiver = sendNameFloatReceiverDict[sendName][i];
					
					if (receiver.asynchronous) {
						receiver.Receive(value);
					}
					else {
						receiver.Enqueue(value);
						queuedReceivers.Enqueue(receiver);
					}
				}
			}
		}
	
		void ReceiveSymbol(string sendName, string value) {
			if (sendName == "Debug") {
				Logger.Log(string.Format("{0} received Symbol: {1}", sendName, value));
			}
			else if (sendNameSymbolReceiverDict.ContainsKey(sendName)) {
				for (int i = sendNameSymbolReceiverDict[sendName].Count - 1; i >= 0; i--) {
					PureDataSymbolReceiver receiver = sendNameSymbolReceiverDict[sendName][i];
					
					if (receiver.asynchronous) {
						receiver.Receive(value);
					}
					else {
						receiver.Enqueue(value);
						queuedReceivers.Enqueue(receiver);
					}
				}
			}
		}
	
		void ReceiveList(string sendName, object[] values) {
			if (sendName == "Debug") {
				Logger.Log(string.Format("{0} received List: {1}", sendName, Logger.ObjectToString(values)));
			}
			else if (sendNameListReceiverDict.ContainsKey(sendName)) {
				for (int i = sendNameListReceiverDict[sendName].Count - 1; i >= 0; i--) {
					PureDataListReceiver receiver = sendNameListReceiverDict[sendName][i];
					
					if (receiver.asynchronous) {
						receiver.Receive(values);
					}
					else {
						receiver.Enqueue(values);
						queuedReceivers.Enqueue(receiver);
					}
				}
			}
		}
	
		void ReceiveMessage(string sendName, string message, object[] values) {
			if (sendName == "Debug") {
				Logger.Log(string.Format("{0} received Message: {1} {2}", sendName, message, Logger.ObjectToString(values)));
			}
			else if (sendName == "Command") {
				pureData.commandParser.ParseCommand(message, values);
			}
			else if (sendNameMessageReceiverDict.ContainsKey(sendName)) {
				for (int i = sendNameMessageReceiverDict[sendName].Count - 1; i >= 0; i--) {
					PureDataMessageReceiver receiver = sendNameMessageReceiverDict[sendName][i];
					
					if (receiver.asynchronous) {
						receiver.Receive(message, values);
					}
					else {
						receiver.Enqueue(message, values);
						queuedReceivers.Enqueue(receiver);
					}
				}
			}
		}
		#endregion
		
		#region Release
		public void Release(PureDataBangReceiver bangReceiver) {
			sendNameBangReceiverDict[bangReceiver.sendName].Remove(bangReceiver);
		}
		
		public void Release(PureDataFloatReceiver floatReceiver) {
			sendNameFloatReceiverDict[floatReceiver.sendName].Remove(floatReceiver);
		}
		
		public void Release(PureDataSymbolReceiver symbolReceiver) {
			sendNameSymbolReceiverDict[symbolReceiver.sendName].Remove(symbolReceiver);
		}
		
		public void Release(PureDataListReceiver listReceiver) {
			sendNameListReceiverDict[listReceiver.sendName].Remove(listReceiver);
		}
		
		public void Release(PureDataMessageReceiver messageReceiver) {
			sendNameMessageReceiverDict[messageReceiver.sendName].Remove(messageReceiver);
		}
		
		public void Release(BangReceiveCallback bangCallback) {
			foreach (List<PureDataBangReceiver> bangReceivers in sendNameBangReceiverDict.Values) {
				for (int i = bangReceivers.Count - 1; i >= 0; i--) {
					if (bangReceivers[i].bangReceiver == bangCallback) {
						Release(bangReceivers[i]);
					}
				}
			}
		}
		
		public void Release(FloatReceiveCallback floatCallback) {
			foreach (List<PureDataFloatReceiver> floatReceivers in sendNameFloatReceiverDict.Values) {
				for (int i = floatReceivers.Count - 1; i >= 0; i--) {
					if (floatReceivers[i].floatReceiver == floatCallback) {
						Release(floatReceivers[i]);
					}
				}
			}
		}
		
		public void Release(SymbolReceiveCallback symbolCallback) {
			foreach (List<PureDataSymbolReceiver> symbolReceivers in sendNameSymbolReceiverDict.Values) {
				for (int i = symbolReceivers.Count - 1; i >= 0; i--) {
					if (symbolReceivers[i].symbolReceiver == symbolCallback) {
						Release(symbolReceivers[i]);
					}
				}
			}
		}
		
		public void Release(ListReceiveCallback listCallback) {
			foreach (List<PureDataListReceiver> listReceivers in sendNameListReceiverDict.Values) {
				for (int i = listReceivers.Count - 1; i >= 0; i--) {
					if (listReceivers[i].listReceiver == listCallback) {
						Release(listReceivers[i]);
					}
				}
			}
		}
		
		public void Release(MessageReceiveCallback messageCallback) {
			foreach (List<PureDataMessageReceiver> messageReceivers in sendNameMessageReceiverDict.Values) {
				for (int i = messageReceivers.Count - 1; i >= 0; i--) {
					if (messageReceivers[i].messageReceiver == messageCallback) {
						Release(messageReceivers[i]);
					}
				}
			}
		}

		public void ReleaseAll() {
			queuedReceivers.Clear();
			sendNameBangReceiverDict.Clear();
			sendNameFloatReceiverDict.Clear();
			sendNameSymbolReceiverDict.Clear();
			sendNameListReceiverDict.Clear();
			sendNameMessageReceiverDict.Clear();
		}
		#endregion
		
		#region Array
		public bool WriteArray(string arrayName, int offset, float[] data, int amountOfValues) {
			if (!LibPD.Exists(arrayName)) {
				return false;
			}
			
			if (LibPD.ArraySize(arrayName) != amountOfValues) {
				ResizeArray(arrayName, amountOfValues);
			}
			
			int success = LibPD.WriteArray(arrayName, offset, data, amountOfValues);
			return success == 0;
		}
	
		public bool WriteArray(string arrayName, int offset, float[] data) {
			return WriteArray(arrayName, offset, data, data.Length);
		}
	
		public bool WriteArray(string arrayName, float[] data) {
			return WriteArray(arrayName, 0, data, data.Length);
		}

		public bool ReadArray(string arrayName, float[] data) {
			return LibPD.ReadArray(data, arrayName, 0, data.Length) == 0;
		}

		public bool ResizeArray(string arrayName, int size) {
			return SendMessage(arrayName, "resize", size);
		}
		
		public int GetArraySize(string arrayName) {
			return LibPD.ArraySize(arrayName);
		}
		#endregion
	
		public void Start() {
			SubscribeDebug();
		}
	
		public void Stop() {
			UnsubscribeDebug();
		}
	}
}
