public class HealTurnAction : TurnAction
{
    public HealTurnAction(Hero owner, int range = 1, string name = "Heal") : base(owner, name, range,
        new TargetingOptions(true, TargetType.Ally, PathBlocker.Terrain))
    {
    }

    public override void ApplyEffectToModelData(GridCoord target)
    {
        UnityEngine.Debug.Log("Healing");
        // process heal effect
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
        owner.GetVisualRepresentation().GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Casting, true);

        VisualTaskQueue.EnqueueAction(new WaitVisualTask(0.5f));
        VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
    
}