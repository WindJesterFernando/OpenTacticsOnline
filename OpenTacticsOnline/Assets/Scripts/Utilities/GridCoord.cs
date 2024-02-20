public struct GridCoord
{
    public int x;

    public int y;

    public GridCoord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //public static readonly GridCoord One = new GridCoord(1, 1);

    public static readonly GridCoord Up = new GridCoord(0, 1);

    public static readonly GridCoord Down = new GridCoord(0, -1);

    public static readonly GridCoord Left = new GridCoord(-1, 0);

    public static readonly GridCoord Right = new GridCoord(1, 0);

    public static bool operator ==(GridCoord lhs, GridCoord rhs)
    {
        return lhs.x == rhs.x && lhs.y == rhs.y;
    }

    public static bool operator !=(GridCoord lhs, GridCoord rhs)
    {
        return !(lhs == rhs);
    }

}