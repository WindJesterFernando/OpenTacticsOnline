
public abstract class TurnAction
{
    public readonly Hero owner;
    public readonly string name;
    public readonly int steps;
    public readonly TargetingOptions targetingOptions;
    public readonly bool requiresTarget;

    protected TurnAction(Hero owner, string name, int steps, TargetingOptions targetingOptions)
    {
        this.owner = owner;
        this.name = name;
        this.requiresTarget = true;
        this.steps = steps;
        this.targetingOptions = targetingOptions;
    }
    protected TurnAction(Hero owner, string name)
    {
        this.owner = owner;
        this.name = name;
        this.requiresTarget = false;
        this.steps = 0;
        this.targetingOptions = null;
    }


    public abstract void ApplyEffectToModelData(GridCoord target);
    public abstract void EnqueueVisualSequence(GridCoord target);
}