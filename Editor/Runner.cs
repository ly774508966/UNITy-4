namespace UNITy.Editor {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;
	using UnityObject = UnityEngine.Object;

	public class Runner {
		private const string DLL_FOLDER = "/../Library/ScriptAssemblies/";

		public readonly List<Result> Tests = new List<Result>();
		private readonly Dictionary<Type, MethodInfo> inits = new Dictionary<Type, MethodInfo>();
		private readonly Dictionary<Type, bool> initComplete = new Dictionary<Type, bool>();

		public void Scan() {
			string dllFolder = Application.dataPath + DLL_FOLDER;

			foreach (string file in Directory.GetFiles(dllFolder, "*.dll")) {
				Assembly assembly = Assembly.LoadFile(file);

				foreach (Type type in assembly.GetTypes()) {
					if (!Attribute.IsDefined(type, typeof (TestClassAttribute))) {
						continue;
					}

					MethodInfo[] methods = type.GetMethods();

					foreach (MethodInfo method in methods) {
						var initMethod = Attribute.GetCustomAttribute(method, typeof (ClassInitializeAttribute), false) as ClassInitializeAttribute;
						var testMethod = Attribute.GetCustomAttribute(method, typeof (TestMethodAttribute), false) as TestMethodAttribute;

						if (initMethod != null) {
							inits.Add(method.DeclaringType, method);
							initComplete[method.DeclaringType] = false;
						} else if (testMethod != null) {
							Tests.Add(new Result(method));
						}
					}
				}
			}
		}

		public IEnumerator Run() {
			foreach (Result result in Tests) {
				Init(result.Method.DeclaringType);
				Run(result);

				yield return null;
			}
		}

		public IEnumerator Run(Type type) {
			foreach (Result result in Tests) {
				if (type == result.Method.DeclaringType) {
					Init(type);
					Run(result);
				}

				yield return null;
			}
		}

		public void Run(Result test) {
			Init(test.Method.DeclaringType);

			test.Pass = true;
			test.Message = string.Empty;
			try {
				test.Method.Invoke(test.Method, new object[] { });
			} catch (TargetException) {
				test.Message = "TargetException detected. Please ensure test methods are marked as static";
				test.Pass = null;
			} catch (Exception e) {
				if (null != e.InnerException) {
					e = e.InnerException;
				}

				test.Pass = false;
				test.Message = e.Message;

				Debug.LogException(e);
			}
		}

		private void Init(Type type) {
			if (inits.ContainsKey(type) && !initComplete[type]) {
				MethodInfo init = inits[type];

				try {
					// If the init has a scene set, load it
					var attribute = (ClassInitializeAttribute)Attribute.GetCustomAttribute(init, typeof (ClassInitializeAttribute));

					if (!string.IsNullOrEmpty(attribute.Scene)) {
						EditorApplication.NewScene();
						EditorApplication.OpenScene(attribute.Scene);
					} else if (attribute.NewScene) {
						EditorApplication.NewScene();
					}

					init.Invoke(init, new object[] { });
				} catch (Exception e) {
					if (null != e.InnerException) {
						e = e.InnerException;
					}

					Debug.LogException(e);
				}
			}

			// reset the init of other tests in case they need to reload their own scene
			var keys = new Type[initComplete.Count];
			initComplete.Keys.CopyTo(keys, 0);
			foreach (Type key in keys) {
				initComplete[key] = false;
			}

			initComplete[type] = true;
		}
	}
}