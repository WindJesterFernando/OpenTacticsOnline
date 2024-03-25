public class ApplyEffectToModelDataVisualTask : VisualTask
{
    private readonly TurnAction actionToExecute;
    private GridCoord target;

    public ApplyEffectToModelDataVisualTask(TurnAction action, GridCoord target)
    {
        actionToExecute = action;
        this.target = target;
    }
    
    public override void Update()
    {
        actionToExecute.ApplyEffectToModelData(target);
        IsDone = true;
    }
}
