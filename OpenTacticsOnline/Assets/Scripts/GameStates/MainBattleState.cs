using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class MainBattleState : AbstractGameState
{
    public MainBattleState()
        : base(GameState.MainPlay)
    {
    }

    public override void OnStateEnter()
    {
        BattleSystemModelData.RandomlyOrderTurns();
    }

    public override void OnStateContinue()
    {
        base.OnStateContinue();
    }

    public override void Update()
    {
        Hero nextHero = BattleSystemModelData.GetActiveHero();

        if (!nextHero.IsAlive())
        {
            nextHero.visualRepresentation.GetComponent<SpriteRenderer>().color = Color.black;
            
            BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
            return;
        }
        
        if (nextHero.isAlly)
        {
            StateManager.PushGameState(new HeroMoveSeletionState(nextHero));
        }
        else
        {
            List<Vector2Int> nearTiles =
                BattleGridModelData.GetNonOccupiedTilesWithinSteps(nextHero.coord, nextHero.maxSteps).ToList();
            
            int randomIndex = RandomGenerator.random.Next(nearTiles.Count);
            StateManager.PushGameState(new HeroMovementState(nextHero, nearTiles[randomIndex]));
            nextHero.coord = nearTiles[randomIndex];
        }
        
        //TODO: refactor!!
        
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     
        //     GameObject[,] tileVisuals = GridVisuals.GetTileVisuals();
        //
        //     foreach (Hero h in BattleGridModelData.GetHeroes())
        //     {
        //         GameObject bgt = tileVisuals[h.coord.x, h.coord.y];
        //         Bounds b = bgt.GetComponent<SpriteRenderer>().bounds;
        //
        //         mouseWorldPoint.z = b.center.z;
        //
        //         if (b.Contains(mouseWorldPoint))
        //         {
        //             Debug.Log("Tile Hit @ " + h.coord);
        //             StateManager.PushGameState(new HeroMoveSeletionState(h));
        //         }
        //     }
        // }
    }
}