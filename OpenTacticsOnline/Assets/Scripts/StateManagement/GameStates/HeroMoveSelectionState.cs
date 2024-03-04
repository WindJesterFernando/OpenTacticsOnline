//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveSelectionState : AbstractGameState
{
    private Hero heroBeingMoved;

    private LinkedList<GridCoord> tilesThatCanBeMovedTo;
    
    public HeroMoveSelectionState(Hero heroBeingMoved)
        : base(GameState.MoveSelection)
    {
        this.heroBeingMoved = heroBeingMoved;
    }

    public override void OnStateEnter()
    {
        tilesThatCanBeMovedTo = BattleGridModelData.GetNonOccupiedTilesWithinSteps(heroBeingMoved.coord, heroBeingMoved.maxSteps, heroBeingMoved.isAlly);

        if (tilesThatCanBeMovedTo.Count == 0)
        {
            Debug.Log("No tiles to move to");
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
                StateManager.PushGameState(new HeroMovementState(heroBeingMoved, coord));
                heroBeingMoved.coord = coord;
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