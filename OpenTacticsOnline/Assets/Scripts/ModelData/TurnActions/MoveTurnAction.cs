using System.Collections.Generic;
using UnityEngine;

public class MoveTurnAction : TurnAction
{
    public MoveTurnAction(Hero owner) : base(owner, "Move", owner.maxSteps,
        new PathfindingOptions(false, TargetType.EmptyTile, PathBlocker.Foe | PathBlocker.Terrain))
    {
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
        Debug.Log("hit 1, target == " + target);
        
        List<GridCoord> path = BattleGridModelData.DoTheAStarThingMyGuy(start, target, owner.isAlly);
        Debug.Log("hit 2, path count == " + path.Count);
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
