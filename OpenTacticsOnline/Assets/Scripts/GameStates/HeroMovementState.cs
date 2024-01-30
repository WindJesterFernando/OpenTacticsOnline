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

        foreach (Vector2Int t in path)
        {
            QueueTest.instance.EnqueueAction(new ActionChangeTileContainer(t, 101));
            QueueTest.instance.EnqueueAction(new ActionWaitContainer(0.25f));
        }
        

        //pop the two states to get back to main
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
    }

    public override void Update()
    {
        
    }
}