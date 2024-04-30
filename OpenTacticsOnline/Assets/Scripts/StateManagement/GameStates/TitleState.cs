using UnityEngine;

public class TitleState : AbstractGameState
{
    public TitleState()
        : base(GameState.Title)
    {

    }

    public override void OnStateEnter()
    {
        Debug.Log("Press Left mouse to play AI, Press Right mouse to connect");
    }
    
    public override void Update()
    {
        if (Input.GetMouseButtonDown(MouseButton.Left))
        {
            StateManager.PushGameState(new LocalRoomState());
        }
        if (Input.GetMouseButtonDown(MouseButton.Right))
        {
            StateManager.PushGameState(new GameRoomState());
        }
    }
}