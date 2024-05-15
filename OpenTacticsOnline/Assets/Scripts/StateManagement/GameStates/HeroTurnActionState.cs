public class HeroTurnActionState : AbstractGameState
{
    private TurnAction turnAction;
    private GridCoord target;
    
    public HeroTurnActionState(TurnAction action, GridCoord target)
    {
        turnAction = action;
        this.target = target;
    }

    public override void OnStateEnter()
    {
        turnAction.EnqueueVisualSequence(target);
    }

    public override void Update()
    {
        if (VisualTaskQueue.GetActionCount() == 0)
        {
            StateManager.PopGameStateUntilStateIs(typeof(MainBattleState));
        }
    }
}