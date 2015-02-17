using System;
using Magicolo.EditorTools;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PropertyFieldAttribute : CustomAttributeBase {
	
	public Type attributeType;
	public object[] arguments;
	
	public PropertyFieldAttribute(Type attributeType, params object[] arguments) {
		this.attributeType = attributeType;
		this.arguments = arguments;
	}
	
	public PropertyFieldAttribute() {
	}
}
