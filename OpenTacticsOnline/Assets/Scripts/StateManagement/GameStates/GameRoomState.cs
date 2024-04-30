using UnityEngine;

public class GameRoomState : AbstractGameState
{
    private bool isFirstPlayer;
    NetworkPlayerController networkPlayerController = new NetworkPlayerController();
    LocalPlayerController localPlayerController = new LocalPlayerController();

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
            SyncedRandomGenerator.Reload(int.Parse(csv[1]));
            Debug.Log("Other joined the room ");
            StartGame();
        }
    }

    private void StartGame()
    {
        BattleGridModelData.Init();

        AddHero(new Hero(2, 2, HeroRole.BlackMage, 6, 20, isFirstPlayer));
        AddHero(new Hero(3, 2, HeroRole.RedMage, 6, 20, isFirstPlayer));
        AddHero(new Hero(3, 3, HeroRole.WhiteMage, 6, 20, isFirstPlayer));

        AddHero(new Hero(15, 7, HeroRole.Fighter, 8, 20, !isFirstPlayer));
        AddHero(new Hero(15, 6, HeroRole.Monk, 8, 20, !isFirstPlayer));
        AddHero(new Hero(15, 5, HeroRole.Thief, 8, 20, !isFirstPlayer));

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

    private void AddHero(Hero hero)
    {
        if (hero.isAlly)
        {
            hero.controller = localPlayerController;
        }
        else
        {
            hero.controller = networkPlayerController;
        }
        BattleGridModelData.AddHero(hero);
    }
}