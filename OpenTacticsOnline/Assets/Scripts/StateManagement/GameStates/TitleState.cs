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
        if (Input.GetMouseButtonDown(0))
        {
            StateManager.PushGameState(new MainBattleState());
        }
    }

}