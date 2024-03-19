using UnityEngine;

public class MainSystemBooter : MonoBehaviour
{
    [SerializeField] private GameObject BattleUICanvas;
    
    void Start()
    {
        ActionQueue.Init();
        StateManager.Init();
        //BattleGridModelData.Init();
        ContentLoader.Init();
        UIManager.Init(BattleUICanvas);
        
        StateManager.PushGameState(new TitleState());
    }

    void Update()
    {
        StateManager.Update();
        ActionQueue.Update();
    }
}