namespace UNITy.Editor {
	using System.Collections;
	using UnityEditor;
	using UnityEngine;

	public class UNITyWindow : EditorWindow {
		private bool running;

		private Runner runner;
		private IEnumerator enumerator;

		[MenuItem("Window/UNITy")]
		private static void Init() {
			var window = GetWindow<UNITyWindow>();
			window.title = "UNITy";
		}

		private void OnGUI() {
			if (!EditorApplication.isCompiling) {
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Scan for Tests", GUILayout.Width(100), GUILayout.Height(20))) {
					runner = new Runner();
					runner.Scan();
				}

				if (null != runner) {
					if (GUILayout.Button("Run all Tests", GUILayout.Width(100), GUILayout.Height(20))) {
						enumerator = runner.Run();
						running = true;
					}

					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.Label(string.Empty, GUILayout.Width(35));
					GUILayout.Label("Test", GUILayout.Width(200));
					GUILayout.Label("Pass?", GUILayout.Width(35));
					GUILayout.Label("Message");
					GUILayout.EndHorizontal();

					foreach (Result result in runner.Tests) {
						GUILayout.BeginHorizontal();
						bool finishHorizontal = true;

						if (GUILayout.Button("Run", GUILayout.Width(35), GUILayout.Height(15))) {
							finishHorizontal = false;

							result.Pass = null;
							result.Message = null;
							runner.Run(result);
						}

						GUILayout.Label(string.Format("{0}.{1}", result.Method.DeclaringType.Name, result.Method.Name), GUILayout.Width(200));
						GUILayout.Label(null == result.Pass ? string.Empty : result.Pass.Value ? "Pass" : "Fail", GUILayout.Width(35));
						GUILayout.Label(result.Message);

						if (finishHorizontal) {
							GUILayout.EndHorizontal();
						}
					}
				}
			}
		}

		private void Update() {
			if (null != runner && running && null != enumerator) {
				if (!enumerator.MoveNext()) {
					running = false;
				}
			}

			Repaint();
		}
	}
}