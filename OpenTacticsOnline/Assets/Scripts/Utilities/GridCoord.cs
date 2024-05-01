using System;

public struct GridCoord
{
    public int x;

    public int y;

    public GridCoord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static readonly GridCoord Up = new GridCoord(0, 1);

    public static readonly GridCoord Down = new GridCoord(0, -1);

    public static readonly GridCoord Left = new GridCoord(-1, 0);

    public static readonly GridCoord Right = new GridCoord(1, 0);

    public static readonly GridCoord Zero = new GridCoord(0, 0);

    public static bool operator ==(GridCoord lhs, GridCoord rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(GridCoord lhs, GridCoord rhs)
    {
        return !(lhs == rhs);
    }

    public static GridCoord operator +(GridCoord a, GridCoord b)
    {
        return new GridCoord(a.x + b.x, a.y + b.y);
    }

    public static GridCoord operator -(GridCoord a, GridCoord b)
    {
        return new GridCoord(a.x - b.x, a.y - b.y);
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }

    public override bool Equals(object other)
    {
        if (!(other is GridCoord))
        {
            return false;
        }

        return Equals((GridCoord)other);
    }

    public bool Equals(GridCoord other)
    {
        return x == other.x && y == other.y;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() ^ (y.GetHashCode() << 2);
    }

    public static int CardinalDistance(GridCoord start, GridCoord end)
    {
        GridCoord dif = end - start;
        return Math.Abs(dif.x) + Math.Abs(dif.y);
    }

    public static GridCoord Parse(string s)
    {
        string m = s[1..^1];
        string[] v = m.Split(',');
        return new GridCoord(int.Parse(v[0]), int.Parse(v[1]));
    }
}