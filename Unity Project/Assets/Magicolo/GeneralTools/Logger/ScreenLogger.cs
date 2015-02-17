using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.GeneralTools {
	public class ScreenLogger : MonoBehaviour {
		
		public static int FontSize = 12;
		public static float Brightness = 0.75F;
		public static float Alpha = 0.9F;
		public static int MaxLines = 100;
		public static float FadeOutTime = 1;
		public static float LifeTime = 60;
		
		readonly List<GUIText> lines = new List<GUIText>();
		
		static ScreenLogger instance;
		static ScreenLogger Instance {
			get {
				if (instance == null) {
					instance = FindObjectOfType<ScreenLogger>();
				}
				
				if (instance == null && Application.isPlaying) {
					Initialize();
				}
				return instance;
			}
		}
		
		static Queue<ScreenLoggerLine> QueuedLines = new Queue<ScreenLoggerLine>();
		
		public static void Initialize() {
			if (instance == null) {
				GameObject gameObject = new GameObject("ScreenLogger");
				gameObject.transform.Reset();
				gameObject.hideFlags = HideFlags.HideInHierarchy;
				instance = gameObject.AddComponent<ScreenLogger>();
			}
		}
		
		public static void Log(string toLog) {
			foreach (string line in toLog.Split('\n')) {
				QueuedLines.Enqueue(new ScreenLoggerLine(line, new Color(Brightness, Brightness, Brightness, Alpha)));
			}
		}
				
		public static void LogWarning(string toLog) {
			foreach (string line in toLog.Split('\n')) {
				QueuedLines.Enqueue(new ScreenLoggerLine(line, new Color(Brightness, Brightness, 0, Alpha)));
			}
		}
				
		public static void LogError(string toLog) {
			foreach (string line in toLog.Split('\n')) {
				QueuedLines.Enqueue(new ScreenLoggerLine(line, new Color(Brightness, 0, 0, Alpha)));
			}
		}
		
		void Update() {
			for (int i = QueuedLines.Count - 1; i >= 0; i--) {
				AddLine(QueuedLines.Dequeue());
			}
		}
		
		void AddLine(ScreenLoggerLine line) {
			if (!Application.isPlaying) {
				Logger.LogError("Can not log to screen while application is not playing.");
				return;
			}
			
			GameObject child = this.AddChild("Line");
			child.transform.Reset();
			
			GUIText text = child.AddComponent<GUIText>();
			text.pixelOffset = new Vector2(5, 5);
			text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text.fontSize = FontSize;
			text.anchor = TextAnchor.LowerLeft;
			text.alignment = TextAlignment.Left;
			text.color = line.color;
			text.text = line.text;
			
			StartCoroutine(Fade(text));
			
			foreach (GUIText t in lines) {
				if (t != null) {
					t.pixelOffset += new Vector2(0, FontSize);
				}
			}
			
			lines.Add(text);
			if (lines.Count > MaxLines) {
				RemoveLine(lines[0]);
			}
		}
		
		void RemoveLine(GUIText text) {
			lines.Remove(text);
			text.gameObject.Remove();
		}
		
		IEnumerator Fade(GUIText text) {
			float counter = 0;
			
			yield return new WaitForSeconds(LifeTime);
			
			while (counter < FadeOutTime && text != null) {
				counter += Time.deltaTime;
				text.SetColor((1 - (counter / FadeOutTime)) * Alpha, "A");
				yield return null;
			}
			
			if (text != null) {
				text.SetColor(0, "A");
				RemoveLine(text);
			}
		}
	}
}

