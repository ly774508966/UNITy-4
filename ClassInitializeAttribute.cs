namespace UNITy {
	using System;

	[AttributeUsage(AttributeTargets.Method)]
	public class ClassInitializeAttribute : Attribute {
		public string Scene { get; set; }
	}
}