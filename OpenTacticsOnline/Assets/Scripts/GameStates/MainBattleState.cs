using System.Collections;
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
        BattleSystemModelData.RandomlyOrderTurns();
        StateManager.PushGameState(new HeroMoveSeletionState(BattleSystemModelData.GetActiveHero()));
    }

    public override void OnStateContinue()
    {
        base.OnStateContinue();
        
        BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
        StateManager.PushGameState(new HeroMoveSeletionState(BattleSystemModelData.GetActiveHero()));
    }

    public override void Update()
    {
        
        
        
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