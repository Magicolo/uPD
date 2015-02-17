using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

public static class Logger {
	
	static bool logToScreen;
	public static bool LogToScreen {
		get {
			return logToScreen;
		}
		set {
			logToScreen = value;
			ScreenLogger.Initialize();
		}
	}
	
	static bool logToConsole = true;
	public static bool LogToConsole {
		get {
			return logToConsole;
		}
		set {
			logToConsole = value;
		}
	}
	
	static Dictionary<System.Type, int> instanceDict = new Dictionary<System.Type, int>();
	
	public static float RoundPrecision = 0.001F;
	
	public static void Log(params object[] toLog) {
		string log = "";
		
		if (toLog != null) {
			foreach (object item in toLog) {
				log += ObjectToString(item);
				log += ", ";
			}
			
			if (!string.IsNullOrEmpty(log)) {
				log = log.Substring(0, log.Length - 2);
			}
		}
			
		if (LogToScreen) {
			ScreenLogger.Log(log);
		}
		
		if (logToConsole) {
			Debug.Log(log);
		}
	}
		
	public static void LogWarning(params object[] toLog) {
		string log = "";
		
		if (toLog != null) {
			foreach (object item in toLog) {
				log += ObjectToString(item);
				log += ", ";
			}
			
			if (!string.IsNullOrEmpty(log)) {
				log = log.Substring(0, log.Length - 2);
			}
		}
		
		if (LogToScreen) {
			ScreenLogger.LogWarning(log);
		}
		
		if (logToConsole) {
			Debug.LogWarning(log);
		}
	}
		
	public static void LogError(params object[] toLog) {
		string log = "";
		
		if (toLog != null) {
			foreach (object item in toLog) {
				log += ObjectToString(item);
				log += ", ";
			}
			
			if (!string.IsNullOrEmpty(log)) {
				log = log.Substring(0, log.Length - 2);
			}
		}
		
		if (LogToScreen) {
			ScreenLogger.LogError(log);
		}

		if (logToConsole) {
			Debug.LogError(log);
		}
	}
			
	public static void LogSingleInstance(Object instanceToLog, params object[] toLog) {
		if (instanceDict.ContainsKey(instanceToLog.GetType())) {
			if (instanceDict[instanceToLog.GetType()] == instanceToLog.GetInstanceID()) {
				Log(toLog);
			}
		}
		else {
			instanceDict[instanceToLog.GetType()] = instanceToLog.GetInstanceID();
			Log(toLog);
		}
	}
	
	public static string ObjectToString(object obj) {
		string str = "";
		
		if (obj is System.Array) {
			str += "(";
			foreach (object item in (System.Array) obj) {
				str += ObjectToString(item) + ", ";
			}
			
			if (str.Length > 1) {
				str = str.Substring(0, str.Length - 2);
			}
			str += ")";
		}
		else if (obj is IList) {
			str += "[";
			foreach (object item in (IList) obj) {
				str += ObjectToString(item) + ", ";
			}
			
			if (str.Length > 1) {
				str = str.Substring(0, str.Length - 2);
			}
			str += "]";
		}
		else if (obj is IDictionary) {
			str += "{";
			foreach (object key in ((IDictionary) (IDictionary) obj).Keys) {
				str += ObjectToString(key) + " : " + ObjectToString(((IDictionary)obj)[key]) + ", ";
			}
			
			if (str.Length > 1) {
				str = str.Substring(0, str.Length - 2);
			}
			str += "}";
		}
		else if (obj is IEnumerator) {
			str += ObjectToString(((IEnumerator)obj).Current);
		}
		else if (obj is Vector2 || obj is Vector3 || obj is Vector4 || obj is Color || obj is Quaternion || obj is Rect) {
			str += VectorToString(obj);
		}
		else if (obj is LayerMask) {
			str += ((LayerMask)obj).value.ToString();
		}
		else if (obj != null) {
			str += obj.ToString();
		}
		else {
			str += "null";
		}
		
		return str;
	}
		
	public static string VectorToString(object obj) {
		string str = "";
		
		if (obj is Vector2) {
			Vector2 vector2 = ((Vector2)obj).Round(RoundPrecision);
			;
			str += "Vector2(" + vector2.x + ", " + vector2.y;
		}
		else if (obj is Vector3) {
			Vector3 vector3 = ((Vector3)obj).Round(RoundPrecision);
			;
			str += "Vector3(" + vector3.x + ", " + vector3.y + ", " + vector3.z;
		}
		else if (obj is Vector4) {
			Vector4 vector4 = ((Vector4)obj).Round(RoundPrecision);
			str += "Vector4(" + vector4.x + ", " + vector4.y + ", " + vector4.z + ", " + vector4.w;
		}
		else if (obj is Quaternion) {
			Quaternion quaternion = ((Quaternion)obj).Round(RoundPrecision);
			str += "Quaternion(" + quaternion.x + ", " + quaternion.y + ", " + quaternion.z + ", " + quaternion.w;
		}
		else if (obj is Color) {
			Color color = ((Color)obj).Round(RoundPrecision);
			str += "Color(" + color.r + ", " + color.g + ", " + color.b + ", " + color.a;
		}
		else if (obj is Rect) {
			Rect rect = ((Rect)obj).Round(RoundPrecision);
			str += "Rect(" + rect.x + ", " + rect.y + ", " + rect.width + ", " + rect.height;
		}
		return str + ")";
	}
}
