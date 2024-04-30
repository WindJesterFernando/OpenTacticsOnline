#region Protocol Signifiers
public static class ClientToServerSignifiers
{
    public const int CreateRoom = 1;


    public const int ActionUsed = 1001;
}

public static class ServerToClientSignifiers
{
    public const int RoomFilled = 101;
    public const int SelfJoinedRoom = 102;
}

#endregion