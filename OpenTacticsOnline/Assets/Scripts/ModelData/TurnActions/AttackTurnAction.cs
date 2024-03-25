
public class AttackTurnAction : TurnAction
{
    public AttackTurnAction(Hero owner, int range = 1, string name = "Attack") : base(owner, name, range,
        new PathfindingOptions(false, TargetType.Foe, PathBlocker.Terrain))
    {
    }
    
    public override void ApplyEffectToModelData(GridCoord target)
    {
        UnityEngine.Debug.Log("Attacking");
        
        // process damage effect
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
       owner.visualRepresentation.GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Attacking, true);
       
       VisualTaskQueue.EnqueueAction(new WaitVisualTask(0.5f));
       VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
}
