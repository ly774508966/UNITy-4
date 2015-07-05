namespace UNITy {
	using System;

	[AttributeUsage(AttributeTargets.Method)]
	public class ClassInitializeAttribute : Attribute {
		public bool NewScene;
		public string Scene;
	}
}