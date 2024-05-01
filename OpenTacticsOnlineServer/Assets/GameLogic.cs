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

    public void MessageGot(int clientId, Message msg)
    {
        gameRooms[clientId].MessageGot(clientId, msg);
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
        // game is not started 
        if (!gameRooms.ContainsKey(clientId))
            return;
        
        if (!gameRooms[clientId].HasGameStarted())
        {
            gameRoomToJoin = null;
            gameRooms.Remove(clientId);
            return;
        }

        GameRoom room = gameRooms[clientId];
        room.HandleDisconnect(clientId);
        gameRooms.Remove(room.PlayerId1);
        gameRooms.Remove(room.PlayerId2);
    }

    public void ClientConnected(int clientId)
    {
        CreateRoom(clientId);
    }
}
