using System.Collections.Generic;
using UnityEngine;

public class MainBattleState : AbstractGameState
{
    public MainBattleState()
        : base(GameState.MainPlay)
    {
    }

    public override void OnStateEnter()
    {
        BattleGridModelData.Init();
        GridVisuals.CreateBattleGridVisuals(BattleGridModelData.GetBattleGridTiles());
        BattleSystemModelData.RandomlyOrderTurns();
    }

    public override void OnStateContinue()
    {
        base.OnStateContinue();
        BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
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
        
        if (CheckBattleEndConditions()) 
            return; 

        if (nextHero.isAlly)
        {
            StateManager.PushGameState(new SelectActionUIState(nextHero));
            // StateManager.PushGameState(new HeroMoveSeletionState(nextHero));
        }
        else
        {
            List<GridCoord> nearTiles =
                BattleGridModelData.GetNonOccupiedTilesWithinSteps(nextHero.coord, nextHero.maxSteps, nextHero.isAlly);

            if (nearTiles.Count == 0)
            {
                BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
                return;
            }
            
            int randomIndex = RandomGenerator.random.Next(nearTiles.Count);
            GridCoord randomGridCoord = nearTiles[randomIndex];

            StateManager.PushGameState(new HeroMovementState(nextHero, randomGridCoord));
            nextHero.coord = randomGridCoord;
        }
        
        
        
        /* Mouse click on hero
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
        */
    }

    private static bool CheckBattleEndConditions()
    {
        if (IsTeamDead(BattleGridModelData.GetAllyHeroes()))
        {
            StateManager.PushGameState(new GameResultsState(false));
            return true;
        }

        if (IsTeamDead(BattleGridModelData.GetFoeHeroes()))
        {
            StateManager.PushGameState(new GameResultsState(true));
            return true;
        }

        return false;
    }

    private static bool IsTeamDead(LinkedList<Hero> team)
    {
        foreach (Hero h in team)
        {
            if (h.IsAlive())
            {
                return false;
            }
        }
        return true;
    }


    public override void OnStateExit()
    {
        GridVisuals.DestroyBattleGridVisuals();
    }
}