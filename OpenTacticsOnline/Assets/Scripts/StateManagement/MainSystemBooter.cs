using UnityEngine;

public class MainSystemBooter : MonoBehaviour
{
    [SerializeField] private GameObject BattleUICanvas;
    
    void Start()
    {
        VisualTaskQueue.Init();
        StateManager.Init();
        //BattleGridModelData.Init();
        ContentLoader.Init();
        UIManager.Init(BattleUICanvas);
        
        // NetworkClientProcessing.SetMainSystemBooter(this);
        StateManager.PushGameState(new TitleState());
        
    }

    void Update()
    {
        StateManager.Update();
        VisualTaskQueue.Update();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            NetworkClientProcessing.SendMessageToServer($"{ClientToServerSignifiers.Disconnect}");
            StateManager.PopGameStateUntilStateIs(GameState.Title);
            Destroy(NetworkClientProcessing.GetNetworkedClient());
        }
    }
}