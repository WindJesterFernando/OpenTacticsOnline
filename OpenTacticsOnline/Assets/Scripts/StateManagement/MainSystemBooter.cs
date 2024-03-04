using System.Collections.Generic;
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
        print("");
        StateManager.Update();
        ActionQueue.Update();
        if (Input.GetKeyDown(KeyCode.A))
        {
            List<TurnAction> actions = new List<TurnAction>();
            actions.Add(new AttackTurnAction());
            actions.Add(new MoveTurnAction());
            UIManager.EnableButtons(actions);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UIManager.DisableButtons();
        }
    }
}