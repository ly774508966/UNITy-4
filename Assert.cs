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

		public static void AreEqual(IComparable expected, IComparable actual) {
			if (Equals(expected, actual)) {
				return;
			}

			throw new Exception(string.Format("Expected: {0}\tActual: {1}", expected, actual));
		}

		public static void Fail() {
			throw new Exception("Failed");
		}
	}
}