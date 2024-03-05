public class HeroTurnActionState : AbstractGameState
{
    private Hero attackingHero;
    
    public HeroTurnActionState(Hero attackingHero) : base(GameState.PerformingAction)
    {
        this.attackingHero = attackingHero;
    }

    public override void OnStateEnter()
    {
        attackingHero.visualRepresentation.GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Attacking, true);
        
        ActionQueue.EnqueueAction(new ActionWaitContainer(0.5f));
        ActionQueue.EnqueueAction(new ExecuteTurnActionContainer(null));
        ActionQueue.EnqueueAction(new ActionWaitContainer(0.5f));
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