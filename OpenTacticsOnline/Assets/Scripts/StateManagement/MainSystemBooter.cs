using UnityEngine;

public class MainSystemBooter : MonoBehaviour
{
    [SerializeField] private GameObject TitleUICanvas;
    [SerializeField] private GameObject BattleUICanvas;
    
    public void Start()
    {
        VisualTaskQueue.Init();
        StateManager.Init();
        ContentLoader.Init();
        UIManager.Init(TitleUICanvas, BattleUICanvas);
        
        StateManager.PushGameState(new TitleState());
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (NetworkClientProcessing.IsConnectedToServer())
            {
                MessageBuilder mb = new MessageBuilder(NetworkSignifier.C_Disconnect);
                NetworkClientProcessing.SendMessageToServer(mb);
            }
            StateManager.PopGameStateUntilStateIs(typeof(TitleState));
        }
        StateManager.Update();
        VisualTaskQueue.Update();
    }
}