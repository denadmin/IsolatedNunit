using TestAssembly;

namespace IsolatedTest;

[IsolatedTestFixture]
public class IsolatedTest2
{
    [Test]
    public void Test2()
    {
        var testClass = new TestClass();
        Assert.AreEqual(testClass.Increment(10), 10);
    }
}