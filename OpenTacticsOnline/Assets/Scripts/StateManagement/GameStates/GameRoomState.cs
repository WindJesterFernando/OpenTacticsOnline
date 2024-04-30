using UnityEngine;

public class GameRoomState : AbstractGameState
{
    private bool isFirstPlayer;
    public GameRoomState() : base(GameState.GameRoom)
    {
        
    }

    public override void OnStateEnter()
    {
        GameObject go = new GameObject("NetworkClient");
        go.AddComponent<NetworkClient>();
        NetworkClientProcessing.onMessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(string msg)
    {
        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        if (signifier == ServerToClientSignifiers.SelfJoinedRoom)
        {
            Debug.Log("WE joined the room ");
            isFirstPlayer = csv[1] == "0";
            return;
        }
        
        if (signifier == ServerToClientSignifiers.RoomFilled)
        {
            
            Debug.Log("Other joined the room ");
            StartGame();
        }
    }

    private void StartGame()
    {
        if (isFirstPlayer)
        {
            Camera.main.backgroundColor = Color.red;
        }
        else
        {
            Camera.main.backgroundColor = Color.green;
        }
        
        StateManager.PopGameState();
        StateManager.PushGameState(new MainBattleState());
        
        
        NetworkClientProcessing.onMessageReceived -= OnMessageReceived;
    }
}