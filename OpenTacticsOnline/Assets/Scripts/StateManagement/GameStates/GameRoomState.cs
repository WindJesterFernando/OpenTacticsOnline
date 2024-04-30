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

        BattleGridModelData.Init();

        NetworkPlayerController networkPlayerController = new NetworkPlayerController();
        LocalPlayerController localPlayerController = new LocalPlayerController();

        Hero h = new Hero(2, 2, HeroRole.BlackMage, 6, 20, isFirstPlayer);
        if (h.isAlly)
        {
            h.controller = localPlayerController;
        }
        else
        {
            h.controller = networkPlayerController;
        }
        h.ModifyHealth(-20);
        BattleGridModelData.AddHero(h);

        h = new Hero(3, 2, HeroRole.RedMage, 6, 20, isFirstPlayer);
        if (h.isAlly)
        {
            h.controller = localPlayerController;
        }
        else
        {
            h.controller = networkPlayerController;
        }
        BattleGridModelData.AddHero(h);

        h = new Hero(3, 3, HeroRole.WhiteMage, 6, 20, isFirstPlayer);
        if (h.isAlly)
        {
            h.controller = localPlayerController;
        }
        else
        {
            h.controller = networkPlayerController;
        }
        BattleGridModelData.AddHero(h);

        h = new Hero(15, 7, HeroRole.Fighter, 8, 20, !isFirstPlayer);
        if (h.isAlly)
        {
            h.controller = localPlayerController;
        }
        else
        {
            h.controller = networkPlayerController;
        }
        BattleGridModelData.AddHero(h);

        h = new Hero(15, 6, HeroRole.Monk, 8, 20, !isFirstPlayer);
        if (h.isAlly)
        {
            h.controller = localPlayerController;
        }
        else
        {
            h.controller = networkPlayerController;
        }
        h.ModifyHealth(-20);
        BattleGridModelData.AddHero(h);

        h = new Hero(15, 5, HeroRole.Thief, 8, 20, !isFirstPlayer);
        if (h.isAlly)
        {
            h.controller = localPlayerController;
        }
        else
        {
            h.controller = networkPlayerController;
        }
        BattleGridModelData.AddHero(h);

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