public class ReviveTurnAction : TurnAction
{
    public ReviveTurnAction(Hero owner, int range = 10, string name = "Revive") : base(owner, name, range,
        new TargetingOptions(true, TargetType.KnockedOutAllies, PathBlocker.Terrain, true))
    {
    }

    public override void ApplyEffectToModelData(GridCoord target)
    {
        Hero hero = BattleGridModelData.GetHeroAtCoord(target);
        if(!hero.IsAlive())
            hero.ModifyHealth(5);
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
        owner.GetVisualRepresentation().GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Casting, true);

        VisualTaskQueue.EnqueueAction(new WaitVisualTask(0.5f));
        VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
}