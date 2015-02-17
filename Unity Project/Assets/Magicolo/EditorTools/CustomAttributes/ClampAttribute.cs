using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class ClampAttribute : CustomAttributeBase {
	
	public float min = 0;
	public float max = 1;
	
	public ClampAttribute() {
	}
	
	public ClampAttribute(float min, float max) {
		this.min = min;
		this.max = max;
	}
}
