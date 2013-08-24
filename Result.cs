namespace UNITy {
	using System.Reflection;

	public class Result {
		public bool? Pass;
		public string Message;

		public MethodInfo Method { get; private set; }

		public Result(MethodInfo method) {
			Method = method;
		}
	}
}