namespace UNITy.Tests {
	[TestClass]
	public class Sample {
		[TestMethod]
		public static void Test() {
			var numbers = new int[10];
			Assert.AreEqual(numbers.Length, 10);
		}

		[TestMethod]
		public static void ForcePass() {
			Assert.Pass();
		}

		[TestMethod]
		public static void ForceFail() {
			Assert.Fail();
		}

		[TestMethod]
		public void MarkAsStatic() {
			Assert.Pass();
		}
	}
}