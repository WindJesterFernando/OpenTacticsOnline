public class LocalRoomState : AbstractGameState
{
    public LocalRoomState() : base(GameState.LocalRoom)
    {
    }

    public override void OnStateEnter()
    {
        // Skip this state 
        StateManager.PopGameState();
        StateManager.PushGameState(new MainBattleState());
    }

    public override void OnStateExit()
    {
        
    }
}