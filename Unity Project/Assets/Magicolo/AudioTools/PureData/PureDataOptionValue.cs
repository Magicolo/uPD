using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.AudioTools {
	[System.Serializable]
	public class PureDataOptionValue {

		public enum ValueTypes {
			Float,
			FloatArray,
			String,
			StringArray,
			Bool,
			BoolArray,
			Object,
			ObjectArray,
		}
		
		public ValueTypes type;
		
		[SerializeField] float floatValue;
		[SerializeField] float floatDefaultValue;
		[SerializeField] float[] floatArrayValue;
		[SerializeField] float[] floatArrayDefaultValue;
		[SerializeField] string stringValue;
		[SerializeField] string stringDefaultValue;
		[SerializeField] string[] stringArrayValue;
		[SerializeField] string[] stringArrayDefaultValue;
		[SerializeField] bool boolValue;
		[SerializeField] bool boolDefaultValue;
		[SerializeField] bool[] boolArrayValue;
		[SerializeField] bool[] boolArrayDefaultValue;
		[SerializeField] Object objectValue;
		[SerializeField] Object[] objectArrayValue;
		
		public PureDataOptionValue(object value, object defaultValue) {
			SetValue(value);
			SetDefaultValue(defaultValue);
		}
		
		public string GetValueDisplayName() {
			return Logger.ObjectToString(GetValue());
		}
	
		public T GetValue<T>() {
			return (T)GetValue();
		}
	
		public object GetValue() {
			object value;
			
			switch (type) {
				case ValueTypes.Float:
					value = floatValue;
					break;
				case ValueTypes.FloatArray:
					value = floatArrayValue;
					break;
				case ValueTypes.String:
					value = stringValue;
					break;
				case ValueTypes.StringArray:
					value = stringArrayValue;
					break;
				case ValueTypes.Bool:
					value = boolValue;
					break;
				case ValueTypes.BoolArray:
					value = boolArrayValue;
					break;
				case ValueTypes.Object:
					value = objectValue;
					break;
				case ValueTypes.ObjectArray:
					value = objectArrayValue;
					break;
				default:
					value = null;
					break;
			}
			
			return value;
		}

		public void SetValue(object value) {
			if (value is float) {
				floatValue = (float)value;
				type = ValueTypes.Float;
			}
			else if (value is float[]) {
				floatArrayValue = (float[])value;
				type = ValueTypes.FloatArray;
			}
			else if (value is string) {
				stringValue = (string)value;
				type = ValueTypes.String;
			}
			else if (value is string[]) {
				stringArrayValue = (string[])value;
				type = ValueTypes.StringArray;
			}
			else if (value is bool) {
				boolValue = (bool)value;
				type = ValueTypes.Bool;
			}
			else if (value is bool[]) {
				boolArrayValue = (bool[])value;
				type = ValueTypes.BoolArray;
			}
			else if (value is Object) {
				objectValue = (Object)value;
				type = ValueTypes.Object;
			}
			else if (value is Object[]) {
				objectArrayValue = (Object[])value;
				type = ValueTypes.ObjectArray;
			}
		}
	
		public void SetDefaultValue(object value) {
			if (value is float || value is System.Enum) {
				floatDefaultValue = (float)value;
			}
			else if (value is float[] || value is System.Enum[]) {
				floatArrayDefaultValue = (float[])value;
			}
			else if (value is string) {
				stringDefaultValue = (string)value;
			}
			else if (value is string[]) {
				stringArrayDefaultValue = (string[])value;
			}
			else if (value is bool) {
				boolDefaultValue = (bool)value;
			}
			else if (value is bool[]) {
				boolArrayDefaultValue = (bool[])value;
			}
			else if (value is Object) {
				objectValue = null;
			}
			else if (value is Object[]) {
				objectArrayValue = new Object[0];
			}
		}
	
		public void ResetValue() {
			switch (type) {
				case ValueTypes.Float:
					floatValue = floatDefaultValue;
					break;
				case ValueTypes.FloatArray:
					floatArrayValue = floatArrayDefaultValue;
					break;
				case ValueTypes.String:
					stringValue = stringDefaultValue;
					break;
				case ValueTypes.StringArray:
					stringArrayValue = stringArrayDefaultValue;
					break;
				case ValueTypes.Bool:
					boolValue = boolDefaultValue;
					break;
				case ValueTypes.BoolArray:
					boolArrayValue = boolArrayDefaultValue;
					break;
				case ValueTypes.Object:
					objectValue = null;
					break;
				case ValueTypes.ObjectArray:
					objectArrayValue = new Object[0];
					break;
			}
		}
	}
}
