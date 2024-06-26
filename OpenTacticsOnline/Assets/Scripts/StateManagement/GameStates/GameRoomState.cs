using UnityEngine;

public class GameRoomState : AbstractGameState
{
    private bool isFirstPlayer;
    private NetworkPlayerController networkPlayerController;
    private LocalPlayerController localPlayerController;

    public override void OnStateEnter()
    {
        UIManager.EnablePopupText("Waiting for opponent...");
        GameObject go = new GameObject("NetworkClient");
        go.AddComponent<NetworkClient>();
        NetworkClientProcessing.SetMessageReceiver(OnMessageReceived);
    }

    public override void OnStatePause()
    {
        UIManager.DisablePopupText();
    }

    private void OnMessageReceived(Message msg)
    {

        if (msg.signifier == NetworkSignifier.S_SelfJoined)
        {
            isFirstPlayer = msg.values[0] == "0";
            return;
        }
        
        if (msg.signifier == NetworkSignifier.S_RoomFilled)
        {
            SyncedRandomGenerator.Reload(int.Parse(msg.values[0]));
            StartGame();
        }
    }

    private void StartGame()
    {
        BattleGridModelData.Init();
        NetworkClientProcessing.ClearMessageReceiver();
        networkPlayerController = new NetworkPlayerController();
        localPlayerController = new LocalPlayerController();

        //TODO Add some functionality to customize this and send the data about what we choose to the other client
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
        
        StateManager.PushGameState(new MainBattleState());
    }

    public override void OnStateExit()
    {
        UIManager.DisablePopupText();
        GameObject.Destroy(NetworkClientProcessing.GetNetworkedClient().gameObject);
        NetworkClientProcessing.SetNetworkedClient(null);
        NetworkClientProcessing.ClearMessageReceiver();
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