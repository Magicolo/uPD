using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class SingleLineVectorAttribute : CustomAttributeBase {
	
	public string x;
	public string y;
	public string z;
	public string w;
	
	public SingleLineVectorAttribute() {
		x = "X";
		y = "Y";
		z = "Z";
		w = "W";
	}
	
	public SingleLineVectorAttribute(string xName, string yName) {
		x = xName;
		y = yName;
		z = "Z";
		w = "W";
	}
	
	public SingleLineVectorAttribute(string xName, string yName, string zName) {
		x = xName;
		y = yName;
		z = zName;
		w = "W";
	}
	
	SingleLineVectorAttribute(string xName, string yName, string zName, string wName) {
		x = xName;
		y = yName;
		z = zName;
		w = wName;
	}
}
