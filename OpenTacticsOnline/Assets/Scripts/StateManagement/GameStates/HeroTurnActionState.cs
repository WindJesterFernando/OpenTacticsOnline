public class HeroTurnActionState : AbstractGameState
{
    private TurnAction turnAction;
    private GridCoord target;
    
    public HeroTurnActionState(TurnAction action, GridCoord target) : base(GameState.PerformingAction)
    {
        turnAction = action;
        this.target = target;
    }

    public override void OnStateEnter()
    {
        turnAction.AddVisuals(target);
        
    }

    public override void Update()
    {
        if (ActionQueue.GetActionCount() == 0)
        {
            //mb here do damage
            StateManager.PopGameStateUntilStateIs(GameState.MainPlay);
        }
    }
}