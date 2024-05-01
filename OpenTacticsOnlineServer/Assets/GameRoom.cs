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
        MessageBuilder mb = new MessageBuilder(NetworkSignifier.S_SelfJoined).AddValue(0);
        
        NetworkServerProcessing.SendMessageToClient(mb.GetMessage(), _player1);
        
        Debug.Log("new room created");
        Debug.Log("First player joined the room");
    }

    public void JoinRoom(int player2)
    {
        if (_player2 == nonInitializedPlayer)
        {
            _player2 = player2; 
            Debug.Log("Second player joined the room");
            MessageBuilder mb = new MessageBuilder(NetworkSignifier.S_SelfJoined).AddValue(1);
            NetworkServerProcessing.SendMessageToClient(mb.GetMessage(), _player2);

            int seed = new System.Random().Next();
            
            //TODO remove this!!!!
            seed = 0;

            mb = new MessageBuilder(NetworkSignifier.S_RoomFilled).AddValue(seed);
            
            NetworkServerProcessing.SendMessageToClient(mb.GetMessage(), _player1);
            NetworkServerProcessing.SendMessageToClient(mb.GetMessage(), _player2);
        }
        // send you joined 
    }

    public void MessageGot(int player, Message msg)
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
        
        NetworkServerProcessing.SendMessageToClient(new MessageBuilder(msg).GetMessage(), playerToSendTo);
    }
}