using UnityEngine;

public abstract class TurnAction
{
    public Hero owner;
    public string name;
    public int steps;
    public PathfindingOptions pathfindingOptions;

    protected TurnAction(Hero owner, string name, int steps, PathfindingOptions pathfindingOptions)
    {
        this.owner = owner;
        this.name = name;
        this.steps = steps;
        this.pathfindingOptions = pathfindingOptions;
    }
    public abstract void ApplyEffectToModelData(GridCoord target);
    public abstract void EnqueueVisualSequence(GridCoord target);
}
