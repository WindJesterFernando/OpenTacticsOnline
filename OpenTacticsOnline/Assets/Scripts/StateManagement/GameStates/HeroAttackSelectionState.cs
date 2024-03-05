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
        tilesThatCanBeMovedTo =
            BattleGridModelData.GetStuffInArea(turnAction.owner.coord, turnAction.steps, turnAction.pathfindingOptions);

        if (tilesThatCanBeMovedTo.Count == 0)
        {
            Debug.Log("No accessible targets");
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