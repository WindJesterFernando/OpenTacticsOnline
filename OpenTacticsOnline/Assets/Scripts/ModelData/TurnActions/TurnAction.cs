
public abstract class TurnAction
{
    public Hero owner;
    public string name;
    public int steps;
    public TargetingOptions targetingOptions;

    protected TurnAction(Hero owner, string name, int steps, TargetingOptions targetingOptions)
    {
        this.owner = owner;
        this.name = name;
        this.steps = steps;
        this.targetingOptions = targetingOptions;
    }
    public abstract void ApplyEffectToModelData(GridCoord target);
    public abstract void EnqueueVisualSequence(GridCoord target);
}
