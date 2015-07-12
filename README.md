# UNITy
## How to use UNITy
Mark your test classes and test methods in your code like the included Sample.cs file:

```C#
[TestClass]
public class Sample {
	[TestMethod]
	public static void Test() {
		var numbers = new int[10];
		Assert.AreEqual(numbers.Length, 10);
	}
	...
}
```

1. Inside Unity, choose `UNITy` from the `Window` menu.
2. Click `Scan for Tests`.
3. Click `Run All Tests` or `Run` on the specific test you want to run.

## Supported Attributes
- `TestClass`
- `TestMethod`

## Supported Asserts
+ `IsTrue(bool test)`
+ `AreEqual(object a, object b)`
+ `AreEqual(IComparable expected, IComparable actual)`