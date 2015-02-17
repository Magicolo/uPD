using System;
using UnityEngine;

namespace Magicolo.EditorTools {
	public abstract class CustomAttributeBase : PropertyAttribute {
	
		public string PrefixLabel = "";
		public bool NoPrefixLabel;
		public bool NoFieldLabel;
		public bool NoIndex;
		public bool DisableOnPlay;
		public bool DisableOnStop;
	}
}
