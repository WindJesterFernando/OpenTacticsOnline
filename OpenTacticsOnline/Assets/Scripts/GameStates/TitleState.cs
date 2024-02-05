using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleState : AbstractGameState
{
    public TitleState()
        : base(GameState.Title)
    {

    }

    public override void OnStateEnter()
    {
        Debug.Log("OVERRIDED On Enter: " + GetGameState());
    }
    
    public override void Update()
    {
        //Debug.Log("Sanity check!!!");
        if (Input.GetMouseButtonDown(0))
        {
            StateManager.PushGameState(new MainBattleState());
        }
    }

}