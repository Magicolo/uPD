using System.Collections.Generic;
using Magicolo.EditorTools;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Magicolo.AudioTools {
	[CustomEditor(typeof(PureDataSetup)), CanEditMultipleObjects]
	public class PureDataSetupEditor : CustomEditorBase {
	
		PureDataSetup setup;
		PureDataInfo info;
		SerializedProperty infoProperty;
		AudioClip clip;
		AnimationCurve curveLeft;
		AnimationCurve curveRight;
		
		public override void OnEnable() {
			base.OnEnable();
			
			setup = (PureDataSetup)target;
			info = setup.Info;
			infoProperty = serializedObject.FindProperty("info");
			clip = setup.Clip;
		}
		
		public override void OnDisable() {
			base.OnDisable();
			
			if (setup != null) {
				clip = null;
			}
		}
		
		public override void OnInspectorGUI() {
			Begin();
			
			EditorGUI.BeginChangeCheck();
			
			ShowGeneralSettings();
			ShowSpatialSetting();
			ShowClipSettings();
			
			if (EditorGUI.EndChangeCheck()) {
				foreach (GameObject gameObject in Selection.gameObjects) {
					PureDataSetup selectedSetup = gameObject.GetComponent<PureDataSetup>();
					
					if (selectedSetup != null) {
						selectedSetup.UpdateInfo();
					}
				}
			}
			
			End();
		}

		void ShowGeneralSettings() {
			ShowWaveCurves();
			EditorGUILayout.Space();
			ShowOutput();
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("loadOnAwake"));
			Separator();
			ShowFadeIn();
			ShowFadeOut();
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("volume"));
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("pitch"));
			if (EditorGUI.EndChangeCheck()) {
				ClampFadeIn();
				ClampFadeOut();
			}
			
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("randomVolume"));
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("randomPitch"));
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("loop"));
		}

		void ShowOutput() {
			List<string> options = new List<string>();
			options.Add("Master");
			options.AddRange(setup.pureData.busManager.buses.GetNames());
			
			EditorGUI.BeginChangeCheck();
			
			info.output = Popup(info.output, options.ToArray(), "Output".ToGUIContent());
		
			if (EditorGUI.EndChangeCheck()) {
				for (int i = 0; i < targets.Length; i++) {
					PureDataSetup selectedSetup = targets[i] as PureDataSetup;
					
					if (selectedSetup != null && selectedSetup != setup) {
						selectedSetup.Info.output = info.output;
					}
				}
			}
		}
		
		void ShowFadeIn() {
			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("fadeIn"));
			
			if (info.adjustedLength <= 0) {
				return;
			}
			
			if (EditorGUI.EndChangeCheck()) {
				ClampFadeIn();
			}
		}
		
		void ClampFadeIn() {
			serializedObject.ApplyModifiedProperties();
			info.fadeOut = Mathf.Clamp(info.fadeOut, 0, info.adjustedLength - info.fadeIn);
			info.fadeIn = Mathf.Clamp(info.fadeIn, 0, info.adjustedLength);
			info.fadeOut = Mathf.Clamp(info.fadeOut, 0, info.adjustedLength);
			
			for (int i = 0; i < targets.Length; i++) {
				PureDataSetup selectedSetup = targets[i] as PureDataSetup;
					
				if (selectedSetup != null && selectedSetup != setup) {
					selectedSetup.Info.fadeOut = Mathf.Clamp(info.fadeOut, 0, selectedSetup.Info.adjustedLength - selectedSetup.Info.fadeIn);
					selectedSetup.Info.fadeIn = Mathf.Clamp(selectedSetup.Info.fadeIn, 0, selectedSetup.Info.adjustedLength);
					selectedSetup.Info.fadeOut = Mathf.Clamp(selectedSetup.Info.fadeOut, 0, selectedSetup.Info.adjustedLength);
				}
			}
		}
		
		void ShowFadeOut() {
			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("fadeOut"));
			
			if (info.adjustedLength <= 0) {
				return;
			}
			
			if (EditorGUI.EndChangeCheck()) {
				ClampFadeOut();
			}
		}

		void ClampFadeOut() {
			serializedObject.ApplyModifiedProperties();
			info.fadeIn = Mathf.Clamp(info.fadeIn, 0, info.adjustedLength - info.fadeOut);
			info.fadeIn = Mathf.Clamp(info.fadeIn, 0, info.adjustedLength);
			info.fadeOut = Mathf.Clamp(info.fadeOut, 0, info.adjustedLength);
			
			for (int i = 0; i < targets.Length; i++) {
				PureDataSetup selectedSetup = targets[i] as PureDataSetup;
					
				if (selectedSetup != null && selectedSetup != setup) {
					selectedSetup.Info.fadeIn = Mathf.Clamp(info.fadeIn, 0, selectedSetup.Info.adjustedLength - selectedSetup.Info.fadeOut);
					selectedSetup.Info.fadeIn = Mathf.Clamp(selectedSetup.Info.fadeIn, 0, selectedSetup.Info.adjustedLength);
					selectedSetup.Info.fadeOut = Mathf.Clamp(selectedSetup.Info.fadeOut, 0, selectedSetup.Info.adjustedLength);
				}
			}
		}
		
		void ShowWaveCurves() {
			if (curveLeft == null || curveRight == null) {
				float[] dataLeft;
				float[] dataRight;
				clip.GetUntangledData(out dataLeft, out dataRight);
				
				curveLeft = curveLeft ?? GetCurveFromData(dataLeft, 10000);
				curveRight = curveRight ?? GetCurveFromData(dataRight, 10000);
			}
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField(string.Format("Start: {0} ({1}s) | End: {2} ({3}s)", info.playRangeStart.Round(0.001), (info.adjustedPlayRangeStart * clip.length).Round(0.001), info.playRangeEnd.Round(0.001), (info.adjustedPlayRangeEnd * clip.length).Round(0.001)));
			EditorGUILayout.Space();
			
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 40;
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("isFixed"), "Fixed".ToGUIContent(), GUILayout.Width(60));
			EditorGUIUtility.labelWidth = labelWidth;
			
			EditorGUILayout.EndHorizontal();
			EditorGUI.BeginChangeCheck();
			
			EditorGUILayout.MinMaxSlider(ref info.playRangeStart, ref info.playRangeEnd, 0, 1);
			info.playRangeStart = float.IsNaN(info.playRangeStart) ? 0 : Mathf.Clamp(info.playRangeStart, 0, info.playRangeEnd);
			info.playRangeEnd = float.IsNaN(info.playRangeEnd) ? 1 : Mathf.Clamp(info.playRangeEnd, info.playRangeStart, 1);
			
			if (EditorGUI.EndChangeCheck()) {
				info.playRangeStart = float.IsNaN(info.playRangeStart) ? 0 : Mathf.Clamp(info.playRangeStart, 0, info.playRangeEnd);
				info.playRangeEnd = float.IsNaN(info.playRangeEnd) ? 1 : Mathf.Clamp(info.playRangeEnd, info.playRangeStart, 1);
			
				for (int i = 0; i < targets.Length; i++) {
					PureDataSetup selectedSetup = targets[i] as PureDataSetup;
					
					if (selectedSetup != null && selectedSetup != setup) {
						selectedSetup.Info.playRangeStart = float.IsNaN(selectedSetup.Info.playRangeStart) ? 0 : Mathf.Clamp(info.playRangeStart, 0, selectedSetup.Info.playRangeEnd);
						selectedSetup.Info.playRangeEnd = float.IsNaN(selectedSetup.Info.playRangeEnd) ? 1 : Mathf.Clamp(info.playRangeEnd, selectedSetup.Info.playRangeStart, 1);
					}
				}
			}
			
			if (clip.channels > 1) {
				ShowWaveCurve(curveLeft, 24);
				ShowWaveCurve(curveRight, 24);
			}
			else {
				ShowWaveCurve(curveLeft, 48);
			}
		}
		
		AnimationCurve GetCurveFromData(float[] data, int resolution) {
			int increment = Mathf.Max(clip.samples / resolution, 1);
			Keyframe[] keys = new Keyframe[data.Length];
			
			for (int i = 0; i < data.Length; i += increment) {
				keys[i] = new Keyframe(i * clip.length / clip.samples, data[i]);
			}
			
			return new AnimationCurve(keys);
		}
		
		void ShowWaveCurve(AnimationCurve curve, float height) {
			Rect rect = EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup(true);

			EditorGUILayout.CurveField(curve, Color.green, new Rect(0, -1, clip.length, 2), GUILayout.Height(height * Screen.width / 200));
			
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.EndHorizontal();
			
			EditorGUI.BeginDisabledGroup(!info.isFixed);
			
			rect.y += 1;
			rect.height -= 2;
			GUIStyle style = new GUIStyle("LODBlackBox");
			GUI.Box(new Rect(rect.x, rect.y, rect.width * info.playRangeStart, rect.height), "", style);
			GUI.Box(new Rect(rect.x + rect.width * info.playRangeEnd, rect.y, rect.width * (1 - info.playRangeEnd), rect.height), "", style);
			
			EditorGUI.EndDisabledGroup();
		}
		
		void ShowSpatialSetting() {
			EditorGUILayout.Space();
			
			BeginBox();
			
			EditorGUILayout.LabelField("3D Sound Settings", new GUIStyle("boldLabel"));
			
			EditorGUI.indentLevel += 1;
				
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("dopplerLevel"));
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("volumeRolloffMode"));
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("minDistance"));
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("maxDistance"));
			EditorGUILayout.PropertyField(infoProperty.FindPropertyRelative("panLevel"));
				
			Separator();
			EditorGUI.indentLevel -= 1;
			
			EndBox();
		}
		
		void ShowClipSettings() {
			BeginBox();
			
			EditorGUILayout.LabelField("Clip Info", new GUIStyle("boldLabel"));
			
			EditorGUI.indentLevel += 1;
				
			GUIStyle style = EditorStyles.boldLabel;
			EditorGUILayout.LabelField("Name:", info.Name, style);
			EditorGUILayout.LabelField("Path:", info.path, style);
			EditorGUILayout.LabelField("Channels:", info.channels.ToString(), style);
			EditorGUILayout.LabelField("Frequency:", info.frequency.ToString().Substring(0, 2) + " " + info.frequency.ToString().Substring(2, 3) + " Hz", style);
			EditorGUILayout.LabelField("Length:", info.adjustedLength + " seconds", style);
			EditorGUILayout.LabelField("Samples:", info.samples.ToString(), style);
				
			Separator();
			EditorGUI.indentLevel -= 1;
			
			EndBox();
		}
	}
}