using TestAssembly;

namespace IsolatedTest;

[IsolatedTestFixture]
public class IsolatedTest1
{
    [Test]
    public void Test1()
    {
        var testClass = new TestClass();
        Assert.AreEqual(testClass.Increment(5), 5);
    }
}