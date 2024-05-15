using UnityEngine;

public class TitleState : AbstractGameState
{
    public TitleState()
        : base(GameState.Title)
    {

    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //StateManager.PushGameState(new GameRoomState());
        }
    }

    public override void OnStateEnter()
    {
        UIManager.EnableMenuButtons();
    }

    public override void OnStateContinue()
    {
        UIManager.EnableMenuButtons();
    }

    public override void OnStatePause()
    {
        UIManager.DisableMenuButtons();
    }
}