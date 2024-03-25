using System.Collections.Generic;
using UnityEngine;

public abstract class TurnAction
{
    public Hero owner;
    public string name;
    public int steps;
    public PathfindingOptions pathfindingOptions;
    public abstract void ApplyEffectToModelData(GridCoord target);
    public abstract void EnqueueVisualSequence(GridCoord target);
}


public class MoveTurnAction : TurnAction
{
    public MoveTurnAction(Hero owner)
    {
        pathfindingOptions = new PathfindingOptions(false, TargetType.EmptyTile, PathBlocker.Foe | PathBlocker.Terrain);
        this.owner = owner;
        name = "Move";
        steps = owner.maxSteps;
    }
    
    public override void ApplyEffectToModelData(GridCoord target)
    {
        UnityEngine.Debug.Log("Moving");
        owner.coord = target;
        // StateManager.PushGameState(new HeroMoveSelectionState(owner));
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
        GridCoord start = owner.coord;
        List<GridCoord> path = BattleGridModelData.DoTheAStarThingMyGuy(start, target, owner.isAlly);
        GameObject[,] bgVisuals = GridVisuals.GetTileVisuals();

        Vector3 startPos = bgVisuals[start.x, start.y].transform.position;
        
        foreach (GridCoord t in path)
        {
            Vector3 endPos = bgVisuals[t.x, t.y].transform.position;
            VisualTaskQueue.EnqueueAction(new MoveSpriteVisualTask(owner.visualRepresentation, startPos, endPos, 0.25f ));

            startPos = endPos;
        }
        VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
}

public class AttackTurnAction : TurnAction
{
    public AttackTurnAction(Hero owner, int range = 1, string name = "Attack")
    {
        pathfindingOptions = new PathfindingOptions(false, TargetType.Foe, PathBlocker.Terrain);
        this.owner = owner;
        this.steps = range;
        base.name = name;
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

public class HealTurnAction : TurnAction
{
    public HealTurnAction(Hero owner, int range = 1, string name = "Heal")
    {
        pathfindingOptions = new PathfindingOptions(true, TargetType.Ally, PathBlocker.Terrain);
        this.owner = owner;
        this.steps = range;
        base.name = name;
    }

    public override void ApplyEffectToModelData(GridCoord target)
    {
        Debug.Log("Healing");
        // process heal effect
    }

    public override void EnqueueVisualSequence(GridCoord target)
    {
        owner.visualRepresentation.GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Casting, true);

        VisualTaskQueue.EnqueueAction(new WaitVisualTask(0.5f));
        VisualTaskQueue.EnqueueAction(new ApplyEffectToModelDataVisualTask(this, target));
    }
    
}