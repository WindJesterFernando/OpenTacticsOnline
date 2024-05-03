using UnityEngine;

public class MainSystemBooter : MonoBehaviour
{
    [SerializeField] private GameObject BattleUICanvas;
    
    public void Start()
    {
        VisualTaskQueue.Init();
        StateManager.Init();
        //BattleGridModelData.Init();
        ContentLoader.Init();
        UIManager.Init(BattleUICanvas);
        
        // NetworkClientProcessing.SetMainSystemBooter(this);
        StateManager.PushGameState(new TitleState());
        
        // var mb = new MessageBuilder(NetworkSignifier.C_CreateRoom);
        // mb.AddValue(1.1f).AddValue(true).AddValue("Hello World").AddValue(5).AddValue(new GridCoord(1, 2));
        //
        // string message = mb.GetMessage();
        // Debug.Log(message);
        // Message msg = new Message(message);
        // Debug.Log(float.Parse(msg.values[0]));
        // Debug.Log(bool.Parse(msg.values[1]));
        // Debug.Log((msg.values[2]));
        // Debug.Log(int.Parse(msg.values[3]));
        // Debug.Log(GridCoord.Parse(msg.values[4]));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MessageBuilder mb = new MessageBuilder(NetworkSignifier.C_Disconnect);
            NetworkClientProcessing.SendMessageToServer(mb);
            StateManager.PopGameStateUntilStateIs(GameState.Title);
        }
        StateManager.Update();
        VisualTaskQueue.Update();

    }
}