public class HeroTurnActionState : AbstractGameState
{
    private Hero attackingHero;
    
    public HeroTurnActionState(Hero attackingHero) : base(GameState.PerformingAction)
    {
    }

    public override void OnStateEnter()
    {
    }

    public override void Update()
    {
        if (ActionQueue.GetActionCount() == 0)
        {
            StateManager.PopGameStateUntilStateIs(GameState.MainPlay);
        }
    }
}