namespace UNITy {
	using System;

	public static class Assert {
		public static void IsTrue(bool test) {
			if (test) {
				return;
			}

			throw new Exception("IsTrue failed");
		}

		public static void AreEqual(object a, object b) {
			if (a == b) {
				return;
			}

			throw new Exception(string.Format("Expected: {0}\tActual: {1}", a, b));
		}

		public static void AreEqual(IComparable a, IComparable b) {
			if (Equals(a, b)) {
				return;
			}

			throw new Exception(string.Format("Expected: {0}\tActual: {1}", a, b));
		}

		public static void Pass() { }

		public static void Fail() {
			throw new Exception("Failed");
		}
	}
}