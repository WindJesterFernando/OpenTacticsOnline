public class HealTurnAction : TurnAction
{
    public HealTurnAction(Hero owner, int range = 1, string name = "Heal") : base(owner, name, range,
        new TargetingOptions(true, TargetType.Ally, BlockerFlag.Terrain))
    {
    }

    public override void ApplyEffectToModelData(GridCoord target)
    {
        Hero hero = BattleGridModelData.GetHeroAtCoord(target);
        hero.ModifyHealth(10);
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
        owner.GetVisualRepresentation().GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Casting, true);

        VisualTaskQueue.EnqueueAction(new WaitVisualTask(0.5f));
        VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
}