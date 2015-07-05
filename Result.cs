namespace UNITy {
	using System.Reflection;

	public class Result {
		public string Message;
		public bool? Pass;

		public Result(MethodInfo method) {
			Method = method;
		}

		public MethodInfo Method { get; private set; }
	}
}