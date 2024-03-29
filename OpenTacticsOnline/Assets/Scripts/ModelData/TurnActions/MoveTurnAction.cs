using System.Collections.Generic;
using UnityEngine;

public class MoveTurnAction : TurnAction
{
    public MoveTurnAction(Hero owner) : base(owner, "Move", owner.maxSteps,
        new TargetingOptions(false, TargetType.EmptyTile, PathBlocker.Foe | PathBlocker.Terrain))
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
        
        List<GridCoord> path = BattleGridModelData.FindShortestPath(start, target, owner.isAlly);
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
