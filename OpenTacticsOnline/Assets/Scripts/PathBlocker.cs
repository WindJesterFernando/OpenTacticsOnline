using System;
using UnityEngine;

[Flags]
public enum PathBlocker : byte
{
    None = 0,
    Ally = 1 << 0, //0001
    Opponent = 1 << 1, //0010
    Terrain = 1 << 2 //0100
}

[Flags]
public enum TargetType : byte
{
    None = 0,
    Ally = 1 << 0,
    Opponent = 1 << 1,
    KnockedOutAllies = 1 << 2,
    EmptyTile = 1 << 3,
    AnyTile = byte.MaxValue
}

public readonly struct BitFlag<T> where T : Enum
{
    private readonly byte flagContainer;

    private BitFlag(T flag)
    {
        flagContainer = ToByte(flag);
    }

    private BitFlag(int flag)
    {
        flagContainer = ToByte(flag);
    }

    // Overload assignment operator (=)
    public static implicit operator BitFlag<T>(T newFlag)
    {
        return new BitFlag<T>(newFlag);
    }

    public static BitFlag<T> operator +(BitFlag<T> leftFlag, BitFlag<T> rightFlag)
    {
        int flags = ToByte(leftFlag) | ToByte(rightFlag);
        return new BitFlag<T>(flags);
    }

    public static BitFlag<T> operator -(BitFlag<T> leftFlag, BitFlag<T> rightFlag)
    {
        // remove thing
        // a = 100101
        // b = 000110
        // t = a & b // 000100
        // r = a & ~t // 100001

        int flags = ToByte(leftFlag) & ~(ToByte(leftFlag) & ToByte(rightFlag));

        return new BitFlag<T>(flags);
    }

    public static bool operator ==(BitFlag<T> leftFlag, BitFlag<T> rightFlag)
    {
        return ToByte(leftFlag) == ToByte(rightFlag);
    }

    public static bool operator !=(BitFlag<T> leftFlag, BitFlag<T> rightFlag)
    {
        return ToByte(leftFlag) != ToByte(rightFlag);
    }

    public bool Contains(BitFlag<T> queriedFlag)
    {
        return (ToByte(flagContainer) & ToByte(queriedFlag)) == ToByte(queriedFlag);
    }

    public bool ContainsAny(BitFlag<T> queriedFlag)
    {
        return (ToByte(flagContainer) & ToByte(queriedFlag)) > 0;
    }

    #region wrappers

    public static byte ToByte(T flag)
    {
        return Convert.ToByte(flag);
    }
    public static byte ToByte(BitFlag<T> flag)
    {
        return Convert.ToByte(flag.flagContainer);
    }
    public static byte ToByte(int flag)
    {
        return Convert.ToByte(flag);
    }

    #endregion

    #region auto generated

    public bool Equals(BitFlag<T> other)
    {
        return ToByte(flagContainer) == ToByte(other.flagContainer);
    }

    public override bool Equals(object obj)
    {
        return obj is BitFlag<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return ToByte(flagContainer);
    }

    #endregion
}