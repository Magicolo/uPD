using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class RectExtensions {

		public static Rect Round(this Rect rect, double step) {
			if (step <= 0) return rect;
			rect.x = (float)(Mathf.Round((float)(rect.x * (1D / step))) / (1D / step));
			rect.y = (float)(Mathf.Round((float)(rect.y * (1D / step))) / (1D / step));
			rect.width = (float)(Mathf.Round((float)(rect.width * (1D / step))) / (1D / step));
			rect.height = (float)(Mathf.Round((float)(rect.height * (1D / step))) / (1D / step));
			return rect;
		}
	
		public static Rect Round(this Rect rect) {
			return rect.Round(1);
		}
	
		public static bool Intersects(this Rect rect, Rect otherRect) {
			return !((rect.x > otherRect.xMax) || (rect.xMax < otherRect.x) || (rect.y > otherRect.yMax) || (rect.yMax < otherRect.y));
		}
	
		public static Rect Intersect(this Rect rect, Rect otherRect) {
			float x = Mathf.Max((sbyte)rect.x, (sbyte)otherRect.x);
			float num2 = Mathf.Min(rect.x + rect.width, otherRect.x + otherRect.width);
			float y = Mathf.Max((sbyte)rect.y, (sbyte)otherRect.y);
			float num4 = Mathf.Min(rect.y + rect.height, otherRect.y + otherRect.height);
			if ((num2 >= x) && (num4 >= y)) {
				return new Rect(x, y, num2 - x, num4 - y);
			}

			return new Rect();
		}
	}
}
