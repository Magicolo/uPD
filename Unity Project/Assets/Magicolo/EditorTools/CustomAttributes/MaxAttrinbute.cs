using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class MaxAttribute : CustomAttributeBase {
	
	public float max = 1;
	
	public MaxAttribute() {
	}
	
	public MaxAttribute(float max) {
		this.max = max;
	}
}
