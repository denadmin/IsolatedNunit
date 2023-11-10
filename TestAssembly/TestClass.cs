namespace TestAssembly;

public class TestClass
{
    private static int _value;

    public int Increment(int count)
    {
        return _value += count;
    }
}