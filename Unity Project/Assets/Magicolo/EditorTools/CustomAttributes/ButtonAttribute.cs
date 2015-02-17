using UnityEngine;
using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ButtonAttribute : CustomAttributeBase {
	
	public string label = "";
	public string methodName = "";
	public string indexVariableName = "";
	public GUIStyle style;
	
	public ButtonAttribute() {
	}
	
	public ButtonAttribute(string label) {
		this.label = label;
	}
	
	public ButtonAttribute(string label, string methodName) {
		this.label = label;
		this.methodName = methodName;
	}
	
	public ButtonAttribute(string label, string methodName, string styleName) {
		this.label = label;
		this.methodName = methodName;
		this.style = new GUIStyle(styleName);
	}
	
	public ButtonAttribute(string label, string methodName, string styleName, string indexVariableName) {
		this.label = label;
		this.methodName = methodName;
		this.style = new GUIStyle(styleName);
		this.indexVariableName = indexVariableName;
	}
}
