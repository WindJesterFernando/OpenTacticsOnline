using UnityEngine;

public class GameRoom
{
    public const int nonInitializedPlayer = -1;
    
    private int playerId1 = nonInitializedPlayer;
    private int playerId2 = nonInitializedPlayer;

    public int PlayerId1 => playerId1;
    public int PlayerId2 => playerId2;

    public bool HasGameStarted()
    {
        return playerId2 != nonInitializedPlayer;
    }

    public GameRoom(int playerId1)
    {
        this.playerId1 = playerId1;
        // send you joined 
        MessageBuilder mb = new MessageBuilder(NetworkSignifier.S_SelfJoined).AddValue(0);
        
        NetworkServerProcessing.SendMessageToClient(mb, this.playerId1);
        
        Debug.Log("new room created");
        Debug.Log("First player joined the room");
    }

    public void JoinRoom(int player2)
    {
        if (this.playerId2 == nonInitializedPlayer)
        {
            this.playerId2 = player2; 
            Debug.Log("Second player joined the room");
            MessageBuilder mb = new MessageBuilder(NetworkSignifier.S_SelfJoined).AddValue(1);
            NetworkServerProcessing.SendMessageToClient(mb, this.playerId2);

            int seed = new System.Random().Next();
            
            //TODO remove this!!!!
            seed = 0;

            mb = new MessageBuilder(NetworkSignifier.S_RoomFilled).AddValue(seed);
            
            NetworkServerProcessing.SendMessageToClient(mb, playerId1);
            NetworkServerProcessing.SendMessageToClient(mb, this.playerId2);
        }
        // send you joined 
    }

    public void MessageGot(int player, Message msg)
    {
        int playerToSendTo = nonInitializedPlayer;
        if (playerId1 == player)
        {
            playerToSendTo = playerId2;
        }
        else if (playerId2 == player)
        {
             playerToSendTo = playerId1;
        }

        if (playerToSendTo == nonInitializedPlayer)
        {
            return;
        }
        
        NetworkServerProcessing.SendMessageToClient(new MessageBuilder(msg), playerToSendTo);
    }

    public void HandleDisconnect(int playerId)
    {
        int playerToSentTo = nonInitializedPlayer;
        if (playerId == playerId1)
        {
            playerToSentTo = playerId2;
        }
        else if (playerId == playerId2)
        {
            playerToSentTo = playerId1;
        }
        else
        {
            Debug.LogError("Trying to disconnect not related player to this room");
            return;
        }

        NetworkServerProcessing.SendMessageToClient(new MessageBuilder(NetworkSignifier.S_OpponentDisconnected), playerToSentTo);
    }
}