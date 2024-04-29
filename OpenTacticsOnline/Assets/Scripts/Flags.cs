public struct TestFlag
{
    private PathBlocker value;

    public TestFlag(PathBlocker blocker)
    {
        value = blocker;
    }

    public static TestFlag operator +(PathBlocker a, TestFlag b)
    {
        return new TestFlag(a| b.value);
    }
    
    public static TestFlag operator +(TestFlag a, PathBlocker b)
    {
        return new TestFlag(a.value | b);
    }

    public static TestFlag operator -(TestFlag a, PathBlocker b)
    {
        // remove thing
        // a = 100101
        // b = 000110
        // t = a & b // 000100
        // r = a & ~t // 100001
        return new TestFlag(a.value & ~(a.value & b));
    }
    
    public static TestFlag operator -(PathBlocker a, TestFlag  b)
    {
        return new TestFlag(a& ~(a & b.value));
    }

    public bool Contains(PathBlocker blocker)
    {
        return (value & blocker) == blocker;
    }
    
    public bool Contains(TestFlag blocker)
    {
        return (value & blocker.value) == blocker.value;
    }
}