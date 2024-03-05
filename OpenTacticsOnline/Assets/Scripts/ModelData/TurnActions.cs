using System.Collections.Generic;
using UnityEngine;

public abstract class TurnAction
{
    public Hero owner;
    public string name;
    public int steps;
    public bool isTargetingFoe;
    public BattleGridModelData.PathfindingOptions pathfindingOptions;
    public abstract void Execute(GridCoord target);
    public abstract void AddVisuals(GridCoord target);
}


public class MoveTurnAction : TurnAction
{
    public MoveTurnAction(Hero owner)
    {
        pathfindingOptions = new BattleGridModelData.PathfindingOptions()
        {
            pathBlockers = BattleGridModelData.PathBlocker.Foe 
                            | BattleGridModelData.PathBlocker.Terrain,
            canTargetSelf = false,
            targetType = BattleGridModelData.TargetType.EmptyTile
        };
        this.owner = owner;
        name = "Move";
        steps = owner.maxSteps;
    }
    
    public override void Execute(GridCoord target)
    {
        Debug.Log("Moving");
        owner.coord = target;
        // StateManager.PushGameState(new HeroMoveSelectionState(owner));
    }

    public override void AddVisuals(GridCoord target)
    {
        GridCoord start = owner.coord;
        LinkedList<GridCoord> path = BattleGridModelData.DoTheAStarThingMyGuy(start, target, owner.isAlly);
        GameObject[,] bgVisuals = GridVisuals.GetTileVisuals();

        Vector3 startPos = bgVisuals[start.x, start.y].transform.position;
        
        foreach (GridCoord t in path)
        {
            Vector3 endPos = bgVisuals[t.x, t.y].transform.position;
            ActionQueue.EnqueueAction(new ActionMoveSpriteContainer(owner.visualRepresentation, startPos, endPos, 0.25f ));

            startPos = endPos;
        }
        ActionQueue.EnqueueAction(new ExecuteTurnActionContainer(this, target));
    }
}

public class AttackTurnAction : TurnAction
{
    public AttackTurnAction(Hero owner, int range = 1, string name = "Attack", bool isTargetingFoe = true)
    {
        this.owner = owner;
        this.steps = range;
        base.name = name;
        this.isTargetingFoe = isTargetingFoe;
    }
    
    public override void Execute(GridCoord target)
    {
        Debug.Log("Attacking");
        
        // do damage    
        // StateManager.PushGameState(new HeroAttackSelectionState(this));
    }

    public override void AddVisuals(GridCoord target)
    {
       owner.visualRepresentation.GetComponent<FrameAnimator>()
            .StartAnimation(AnimationKey.Attacking, true);
       
       ActionQueue.EnqueueAction(new ActionWaitContainer(0.5f));
       ActionQueue.EnqueueAction(new ExecuteTurnActionContainer(this, target));
    }
}