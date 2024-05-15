using System;

[Flags]
public enum BlockerFlag : byte
{
    None = 0,
    Ally = 1 << 0, //0001
    Opponent = 1 << 1, //0010
    Terrain = 1 << 2 //0100
}

public struct PathBlocker
{
    private readonly BlockerFlag value;

    private PathBlocker(BlockerFlag blockerFlag)
    {
        value = blockerFlag;
    }

    // Overload assignment operator (=)
    public static implicit operator PathBlocker(BlockerFlag value)
    {
        return new PathBlocker(value);
    }

    public static PathBlocker operator +(PathBlocker a, PathBlocker b)
    {
        return new PathBlocker(a.value | b.value);
    }

    public static PathBlocker operator -(PathBlocker a, PathBlocker b)
    {
        // remove thing
        // a = 100101
        // b = 000110
        // t = a & b // 000100
        // r = a & ~t // 100001
        return new PathBlocker(a.value & ~(a.value & b.value));
    }

    public static bool operator ==(PathBlocker a, PathBlocker b)
    {
        return a.value == b.value;
    }

    public static bool operator !=(PathBlocker a, PathBlocker b)
    {
        return a.value != b.value;
    }

    public bool Contains(PathBlocker blocker)
    {
        return (value & blocker.value) == blocker.value;
    }

    public bool Equals(PathBlocker other)
    {
        return value == other.value;
    }

    public override bool Equals(object obj)
    {
        return obj is PathBlocker other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (int)value;
    }
}