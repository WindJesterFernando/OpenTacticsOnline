using UnityEngine;

public class GameRoom
{
    public const int nonInitializedPlayer = -1;
    
    private int _player1 = nonInitializedPlayer;
    private int _player2 = nonInitializedPlayer;

    public int PlayerId1 => _player1;
    public int PlayerId2 => _player2;

    public GameRoom(int player1)
    {
        _player1 = player1;
        // send you joined 
        NetworkServerProcessing.SendMessageToClient($"{ServerToClientSignifiers.SelfJoinedRoom},0", _player1);
        Debug.Log("new room created");
        Debug.Log("First player joined the room");
    }

    public void JoinRoom(int player2)
    {
        if (_player2 == nonInitializedPlayer)
        {
            _player2 = player2; 
            Debug.Log("Second player joined the room");
            NetworkServerProcessing.SendMessageToClient($"{ServerToClientSignifiers.SelfJoinedRoom},1", _player2);

            int seed = new System.Random().Next();

            NetworkServerProcessing.SendMessageToClient($"{ServerToClientSignifiers.RoomFilled},{seed}", _player1);
            NetworkServerProcessing.SendMessageToClient($"{ServerToClientSignifiers.RoomFilled},{seed}", _player2);
        }
        // send you joined 
    }

    public void MessageGot(int player, string msg)
    {
        int playerToSendTo = nonInitializedPlayer;
        if (_player1 == player)
        {
            playerToSendTo = _player2;
        }
        else if (_player2 == player)
        {
             playerToSendTo = _player1;
        }

        if (playerToSendTo == nonInitializedPlayer)
        {
            return;
        }
        
        NetworkServerProcessing.SendMessageToClient(msg, playerToSendTo);
    }
}