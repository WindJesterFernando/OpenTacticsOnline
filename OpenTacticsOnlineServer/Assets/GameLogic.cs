using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private Dictionary<int, GameRoom> gameRooms = new Dictionary<int, GameRoom>();
    private GameRoom gameRoomToJoin;
    
    private void Start()
    {
        NetworkServerProcessing.SetGameLogic(this);
    }

    public void MessageGot(int clientId, int signifier, string message)
    {
        gameRooms[clientId].MessageGot(clientId, message);
    }

    private void CreateRoom(int clientId)
    {
        if (gameRoomToJoin == null)
        {
            gameRoomToJoin = new GameRoom(clientId);
            gameRooms[gameRoomToJoin.PlayerId1] = gameRoomToJoin;
        }
        else
        {
            gameRoomToJoin.JoinRoom(clientId);
            
            gameRooms[gameRoomToJoin.PlayerId2] = gameRoomToJoin;
            
            gameRoomToJoin = null;
        }
    }

    public void ClientDisconnected(int clientId)
    {
        
    }

    public void ClientConnected(int clientId)
    {
        CreateRoom(clientId);
    }
}
