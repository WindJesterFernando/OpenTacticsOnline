public class AttackTurnAction : TurnAction
{
    public AttackTurnAction(Hero owner, int range = 1, string name = "Attack") : base(owner, name, range,
        new TargetingOptions(false, TargetType.Opponent, BlockerFlag.Terrain))
    {
        
    }
    
    public override void ApplyEffectToModelData(GridCoord target)
    {
        Hero hero = BattleGridModelData.GetHeroAtCoord(target); 
        hero.ModifyHealth(-5);
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
       owner.GetVisualRepresentation().GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Attacking, true);
       
       VisualTaskQueue.EnqueueAction(new WaitVisualTask(0.5f));
       VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
}