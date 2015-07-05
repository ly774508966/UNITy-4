namespace UNITy.Tests {
	[TestClass]
	public class Sample {
		[TestMethod]
		public static void Test() {
			var numbers = new int[10];
			Assert.AreEqual(numbers.Length, 10);
		}

		[TestMethod]
		public static void ForcePass() {}

		[TestMethod]
		public static void ForceFail() {
			var numbers = new int[10];
			if (numbers.Length > 10) {
				Assert.Fail();
			}
		}

		[TestMethod]
		public void MarkAsStatic() {}
	}
}