using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeroMovementState : AbstractGameState
{
    private Hero heroToMove;
    private Vector2Int coordToMoveTo;
    
    public HeroMovementState(Hero heroToMove, Vector2Int coordToMoveTo)
        : base(GameState.PerformingMove)
    {
        this.heroToMove = heroToMove;
        this.coordToMoveTo = coordToMoveTo;
    }

    public override void OnStateEnter()
    {
        Vector2Int start = new Vector2Int(heroToMove.x, heroToMove.y);
        
        LinkedList<Vector2Int> path = BattleGridModelData.DoTheAStarThingMyGuy(start, coordToMoveTo);

        //GridVisuals.GetHeroVisuals[heroToMove.x, heroToMove.y];

        GameObject[,] bgVisuals = GridVisuals.GetTileVisuals();

        Vector3 startPos = bgVisuals[start.x, start.y].transform.position;
        
        foreach (Vector2Int t in path)
        {
            Vector3 endPos = bgVisuals[t.x, t.y].transform.position;
            //QueueTest.instance.EnqueueAction(new ActionChangeTileContainer(t, 101));
            QueueTest.instance.EnqueueAction(new ActionMoveSpriteContainer(heroToMove.visualRepresentation, startPos, endPos, 0.25f ));
            //QueueTest.instance.EnqueueAction(new ActionWaitContainer(0.25f));

            startPos = endPos;
        }
        

        //pop the two states to get back to main
        //update model data hero coord
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override void Update()
    {
        if (QueueTest.instance.GetActionCount() == 0)
        {
            StateManager.PopGameStateUntilStateIs(GameState.MainPlay);
        }
    }
}