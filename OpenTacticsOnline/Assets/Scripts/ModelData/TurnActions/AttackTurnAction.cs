
public class AttackTurnAction : TurnAction
{
    public AttackTurnAction(Hero owner, int range = 1, string name = "Attack") : base(owner, name, range,
        new TargetingOptions(false, TargetType.Opponent, PathBlocker.Terrain))
    {
        
    }
    
    public override void ApplyEffectToModelData(GridCoord target)
    {
        UnityEngine.Debug.Log("Attacking");
        
        // process damage effect
        Hero hero = BattleGridModelData.GetHeroAtCoord(target); 
        hero.ModifyHealth(-5);
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
       owner.visualRepresentation.GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Attacking, true);
       
       VisualTaskQueue.EnqueueAction(new WaitVisualTask(0.5f));
       VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
}
