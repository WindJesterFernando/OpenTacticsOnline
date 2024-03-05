using System.Collections.Generic;
using UnityEngine;

public class HeroAttackSelectionState : AbstractGameState
{
    private LinkedList<GridCoord> tilesThatCanBeMovedTo;

    private TurnAction turnAction;
    
    public HeroAttackSelectionState(TurnAction action) : base(GameState.AttackSelection)
    {
        turnAction = action;
    }

    public override void OnStateEnter()
    {
        if (!turnAction.isMoving)
        {
            tilesThatCanBeMovedTo = BattleGridModelData.GetHeroesWithinSteps(turnAction.owner.coord, turnAction.steps, turnAction.isTargetingFoe);
        }
        else
        {
            tilesThatCanBeMovedTo = BattleGridModelData.GetNonOccupiedTilesWithinSteps(turnAction.owner.coord,
                turnAction.owner.maxSteps, turnAction.owner.isAlly);
        }

        if (tilesThatCanBeMovedTo.Count == 0)
        {
            Debug.Log("No Heroes to attack");
            //TODO
            // BattleSystemModelData.AdvanceCurrentHeroTurnIndex();
            
            StateManager.PopGameState();
        }
        
        foreach (GridCoord t in tilesThatCanBeMovedTo)
        {
            GridVisuals.ChangeColorOfTile(t, Color.magenta);
        }
    }
    
    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridCoord coord = BattleGridModelData.GetTileUnderMouse();

            if (tilesThatCanBeMovedTo.Contains(coord))
            {
                
                StateManager.PushGameState(new HeroTurnActionState(turnAction, coord));
            }
            else
            {
                StateManager.PopGameState();
            }
            
            foreach (GridCoord t in tilesThatCanBeMovedTo)
            {
                GridVisuals.ChangeColorOfTile(t, Color.white);
            }
        }
    }
}