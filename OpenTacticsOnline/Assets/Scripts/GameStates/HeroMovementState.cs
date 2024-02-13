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
        Vector2Int start = heroToMove.coord;
        LinkedList<Vector2Int> path = BattleGridModelData.DoTheAStarThingMyGuy(start, coordToMoveTo, heroToMove.isAlly);
        GameObject[,] bgVisuals = GridVisuals.GetTileVisuals();

        Vector3 startPos = bgVisuals[start.x, start.y].transform.position;
        
        foreach (Vector2Int t in path)
        {
            Vector3 endPos = bgVisuals[t.x, t.y].transform.position;
            //ActionQueue.instance.EnqueueAction(new ActionChangeTileContainer(t, 101));
            ActionQueue.EnqueueAction(new ActionMoveSpriteContainer(heroToMove.visualRepresentation, startPos, endPos, 0.25f ));
            //ActionQueue.instance.EnqueueAction(new ActionWaitContainer(0.25f));

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
        heroToMove.currentHealth = 0;
        base.OnStateExit();
        BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
    }
}