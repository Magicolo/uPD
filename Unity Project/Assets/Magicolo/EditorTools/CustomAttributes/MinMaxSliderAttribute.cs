using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class MinMaxSliderAttribute : CustomAttributeBase {
	
	public float min = 0;
	public float max = 1;
	public string minLabel;
	public string maxLabel;
	
	public MinMaxSliderAttribute() {
	}
	
	public MinMaxSliderAttribute(float min, float max) {
		this.min = min;
		this.max = max;
	}
	
	public MinMaxSliderAttribute(float min, float max, string minLabel, string maxLabel) {
		this.min = min;
		this.max = max;
		this.minLabel = minLabel;
		this.maxLabel = maxLabel;
	}
}
