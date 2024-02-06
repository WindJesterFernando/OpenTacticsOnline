using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainSystemBooter : MonoBehaviour
{
    void Start()
    {
        ActionQueue.Init();
        StateManager.Init();
        BattleGridModelData.Init();
        ContentLoader.Init();
        
        StateManager.PushGameState(new TitleState());
    }

    void Update()
    {
        StateManager.Update();
        ActionQueue.Update();
    }
}