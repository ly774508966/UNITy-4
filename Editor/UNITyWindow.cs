namespace UNITy.Editor {
	using System;
	using System.Collections;
	using UnityEditor;
	using UnityEngine;

	public class UNITyWindow : EditorWindow {
		public bool Running;
		public Runner Runner;
		public IEnumerator Enumerator;

		public Vector2 ScrollPos = Vector2.zero;

		[MenuItem("Window/UNITy")]
		private static void Init() {
			var window = GetWindow<UNITyWindow>();
			window.title = "UNITy";
		}

		private void OnGUI() {
			ScrollPos = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), ScrollPos, new Rect(0, 0, 2000, 2000));
			
			if (EditorApplication.isCompiling) {
				GUI.EndScrollView();
				return;
			}

			if (null == Runner) {
				EditorGUILayout.BeginHorizontal(GUILayout.Width(2000));

				if (GUILayout.Button("Scan for Tests", GUILayout.Width(100), GUILayout.Height(20))) {
					Runner = new Runner();
					Runner.Scan();
				}

				EditorGUILayout.EndHorizontal();
			} else {
				EditorGUILayout.BeginHorizontal(GUILayout.Width(2000));

				if (GUILayout.Button("Run all Tests", GUILayout.Width(100), GUILayout.Height(20))) {
					Enumerator = Runner.Run();
					Running = true;
				}

				EditorGUILayout.EndHorizontal();

				GUILayout.BeginHorizontal(GUILayout.Width(2000));
				EditorGUILayout.LabelField(string.Empty, GUILayout.Width(35));
				EditorGUILayout.LabelField("Test", GUILayout.Width(200));
				EditorGUILayout.LabelField("Pass?", GUILayout.Width(35));
				EditorGUILayout.LabelField("Message");
				GUILayout.EndHorizontal();

				Type lastType = null;
				foreach (Result result in Runner.Tests) {
					bool finishHorizontal = true;

					if (result.Method.DeclaringType != lastType) {
						GUILayout.Box(string.Empty, new[] { GUILayout.Width(2000), GUILayout.Height(1) });
						EditorGUILayout.BeginHorizontal(GUILayout.Width(2000));

						if (GUILayout.Button("Run", GUILayout.Width(35), GUILayout.Height(15))) {
							finishHorizontal = false;

							result.Pass = null;
							result.Message = null;
							Enumerator = Runner.Run(result.Method.DeclaringType);
							Running = true;
						}

						EditorGUILayout.LabelField(string.Format("{0}.*", result.Method.DeclaringType.Name));
						EditorGUILayout.EndHorizontal();
					}

					EditorGUILayout.BeginHorizontal(GUILayout.Width(2000));

					if (GUILayout.Button("Run", GUILayout.Width(35), GUILayout.Height(15))) {
						finishHorizontal = false;

						result.Pass = null;
						result.Message = null;
						Runner.Run(result);
					}

					EditorGUILayout.LabelField(string.Format("{0}.{1}", result.Method.DeclaringType.Name, result.Method.Name), GUILayout.Width(200));

					Color contentColor = GUI.contentColor;
					if (null != result.Pass) {
						GUI.contentColor = result.Pass.Value ? Color.green : Color.red;
					}

					EditorGUILayout.LabelField(null == result.Pass ? string.Empty : result.Pass.Value ? "Pass" : "Fail", GUILayout.Width(35));

					if (null != result.Message) {
						GUI.contentColor = Color.Lerp(Color.black, Color.red, 0.95f);
					}

					EditorGUILayout.LabelField(result.Message);

					GUI.contentColor = contentColor;

					if (finishHorizontal) {
						EditorGUILayout.EndHorizontal();
					}

					lastType = result.Method.DeclaringType;
				}
			}

			GUI.EndScrollView();
		}

		private void Update() {
			if (null != Runner && Running && null != Enumerator) {
				if (!Enumerator.MoveNext()) {
					Running = false;
				}
			}

			Repaint();
		}
	}
}