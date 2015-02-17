using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class MinAttribute : CustomAttributeBase {
	
	public float min = 0;
	
	public MinAttribute() {
	}
	
	public MinAttribute(float min) {
		this.min = min;
	}
}
