using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Magicolo {
	public static class StringExtensions {

		public static char Pop(this string s, int index, out string remaining) {
			char c = s[0];
			remaining = s.Remove(index, 1);
			return c;
		}
	
		public static char Pop(this string s, out string remaining) {
			return s.Pop(0, out remaining);
		}
	
		public static char PopRandom(this string s, out string remaining) {
			return s.Pop(UnityEngine.Random.Range(0, s.Length), out remaining);
		}
	
		public static string PopRange(this string s, int startIndex, char stopCharacter, out string remaining) {
			string popped = "";
			int maximumIterations = s.Length;
		
			for (int i = 0; i < maximumIterations - startIndex; i++) {
				char c = s.Pop(startIndex, out s);
	       	
				if (c == stopCharacter) {
					break;
				}
				popped += c;
			}
		
			remaining = s;
			return popped;
		}
	
		public static string PopRange(this string s, char stopCharacter, out string remaining) {
			return s.PopRange(0, stopCharacter, out remaining);
		}
	
		public static string PopRange(this string s, int startIndex, int count, out string remaining) {
			string popped = "";
		
			for (int i = 0; i < count; i++) {
				popped += s.Pop(startIndex, out s);
			}
		
			remaining = s;
			return popped;
		}
	
		public static string PopRange(this string s, int count, out string remaining) {
			return s.PopRange(0, count, out remaining);
		}

		public static string GetRange(this string s, int startIndex, char stopCharacter) {
			string substring = "";
		
			for (int i = 0; i < s.Length - startIndex; i++) {
				char c = s[i + startIndex];
				if (c == stopCharacter) {
					break;
				}
				substring += c;
			}
			return substring;
		}
			
		public static string GetRange(this string s, int startIndex) {
			string substring = "";
		
			for (int i = 0; i < s.Length - startIndex; i++) {
				char c = s[i + startIndex];
				substring += c;
			}
			return substring;
		}
	
		public static string GetRange(this string s, char stopCharacter) {
			return s.GetRange(0, stopCharacter);
		}
	
		public static string Reverse(this string s) {
			string reversed = "";
			for (int i = s.Length - 1; i >= 0; i--) {
				reversed += s[i];
			}
			return reversed;
		}
	
		public static string Capitalized(this string s) {
			string capitalized = "";
			
			if (!string.IsNullOrEmpty(s)) {
				if (s.Length == 1) {
					capitalized = char.ToUpper(s[0]).ToString();
				}
				else {
					capitalized = char.ToUpper(s[0]) + s.Substring(1);
				}
			}
			
			return capitalized;
		}
	
		public static T Capitalized<T>(this T stringArray) where T : IList<string> {
			for (int i = 0; i < stringArray.Count; i++) {
				stringArray[i] = stringArray[i].Capitalized();
			}
			
			return stringArray;
		}
	
		public static string Concat(this IList<string> stringArray, string separator) {
			string concat = "";
		
			for (int i = 0; i < stringArray.Count; i++) {
				concat += stringArray[i];
				if (i < stringArray.Count - 1) {
					concat += separator;
				}
			}
			return concat;
		}
		
		public static string Concat(this IList<string> stringArray, char separator) {
			return stringArray.Concat(separator.ToString());
		}
	
		public static string Concat(this IList<string> stringArray) {
			return stringArray.Concat("");
		}
	
		public static float GetWidth(this string s, Font font) {
			float widthSum = 0;
		
			foreach (var letter in s) {
				CharacterInfo charInfo;
				font.GetCharacterInfo(letter, out charInfo);
				widthSum += charInfo.width;
			}

			return widthSum;
		}
	
		public static Rect GetRect(this string s, Font font, int size = 10, FontStyle fontStyle = FontStyle.Normal) {
			float width = 0;
			float height = 0;
			float lineWidth = 0;
			float lineHeight = 0;
		
			foreach (char letter in s) {
				CharacterInfo charInfo;
				font.GetCharacterInfo(letter, out charInfo, size, fontStyle);
        	
				if (letter == '\n') {
					if (lineHeight == 0) lineHeight = size;
					width = Mathf.Max(width, lineWidth);
					height += lineHeight;
					lineWidth = 0;
					lineHeight = 0;
				}
				else {
					lineWidth += charInfo.width;
					lineHeight = Mathf.Max(lineHeight, charInfo.size);
				}
			}
			width = Mathf.Max(width, lineWidth);
			height += lineHeight;
		
			return new Rect(0, 0, width, height);
		}
	
		public static GUIContent ToGUIContent(this string s, char labelTooltipSeparator) {
			string[] split = s.Split(labelTooltipSeparator);
			return new GUIContent(split[0], split[1]);
		}
	
		public static GUIContent ToGUIContent(this string s) {
			return new GUIContent(s);
		}
	
		public static GUIContent[] ToGUIContents(this IList<string> labels, char labelTooltipSeparator = '\0') {
			GUIContent[] guiContents = new GUIContent[labels.Count];
		
			for (int i = 0; i < labels.Count; i++) {
				if (labelTooltipSeparator != '\0') {
					string[] split = labels[i].Split(labelTooltipSeparator);
					if (split.Length == 1) guiContents[i] = new GUIContent(split[0]);
					else if (split.Length == 2) guiContents[i] = new GUIContent(split[0], split[1]);
					else guiContents[i] = new GUIContent(labels[i]);
				}
				else guiContents[i] = new GUIContent(labels[i]);
			}
			return guiContents;
		}
	
		public static GUIContent[] ToGUIContents(this IList<string> labels, IList<string> tooltips) {
			GUIContent[] guiContents = new GUIContent[labels.Count];
		
			for (int i = 0; i < labels.Count; i++) {
				guiContents[i] = new GUIContent(labels[i], tooltips[i]);
			}
			return guiContents;
		}
	}
}
