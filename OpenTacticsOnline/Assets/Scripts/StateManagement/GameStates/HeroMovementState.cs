using System.Collections.Generic;
using UnityEngine;

public class HeroMovementState : AbstractGameState
{
    private Hero heroToMove;
    private GridCoord coordToMoveTo;
    
    public HeroMovementState(Hero heroToMove, GridCoord coordToMoveTo)
        : base(GameState.PerformingMove)
    {
        this.heroToMove = heroToMove;
        this.coordToMoveTo = coordToMoveTo;
    }

    public override void OnStateEnter()
    {
        GridCoord start = heroToMove.coord;
        List<GridCoord> path = BattleGridModelData.DoTheAStarThingMyGuy(start, coordToMoveTo, heroToMove.isAlly);
        GameObject[,] bgVisuals = GridVisuals.GetTileVisuals();

        Vector3 startPos = bgVisuals[start.x, start.y].transform.position;
        
        foreach (GridCoord t in path)
        {
            Vector3 endPos = bgVisuals[t.x, t.y].transform.position;
            ActionQueue.EnqueueAction(new ActionMoveSpriteContainer(heroToMove.visualRepresentation, startPos, endPos, 0.25f ));
            startPos = endPos;
        }
    }

    public override void Update()
    {
        if (ActionQueue.GetActionCount() == 0)
        {
            StateManager.PopGameStateUntilStateIs(GameState.MainPlay);
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }
}