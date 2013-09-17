namespace UNITy {
	using System;

	[AttributeUsage(AttributeTargets.Method)]
	public class ClassInitializeAttribute : Attribute {
		public string Scene;
		public bool NewScene;
	}
}