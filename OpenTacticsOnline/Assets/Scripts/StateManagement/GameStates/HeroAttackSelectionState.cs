using System.Collections.Generic;
using UnityEngine;

public class HeroAttackSelectionState : AbstractGameState
{
    private List<GridCoord> tilesThatCanBeMovedTo;

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
            GridCoord? coord = GetTileUnderMouse();

            if (coord.HasValue && tilesThatCanBeMovedTo.Contains(coord.Value))
            {
                StateManager.PushGameState(new HeroTurnActionState(turnAction, coord.Value));
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

    private GridCoord? GetTileUnderMouse()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject[,] tileVisuals = GridVisuals.GetTileVisuals();

        for (int x = 0; x < BattleGridModelData.gridSizeX; x++)
        {
            for (int y = 0; y < BattleGridModelData.gridSizeY; y++)
            {
                GameObject bgt = tileVisuals[x, y];
                Bounds b = bgt.GetComponent<SpriteRenderer>().bounds;

                mouseWorldPoint.z = b.center.z;

                if (b.Contains(mouseWorldPoint))
                {
                    Debug.Log("Tile Hit @ " + x + "," + y);
                    return new GridCoord(x, y);
                }
            }
        }

        return null;
    }
}