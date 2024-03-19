using System.Collections.Generic;
using UnityEngine;

public class HeroMoveSelectionState : AbstractGameState
{
    private Hero heroBeingMoved;

    private List<GridCoord> tilesThatCanBeMovedTo;
    
    public HeroMoveSelectionState(Hero heroBeingMoved)
        : base(GameState.MoveSelection)
    {
        this.heroBeingMoved = heroBeingMoved;
    }

    public override void OnStateEnter()
    {
        tilesThatCanBeMovedTo = 
            BattleGridModelData.GetNonOccupiedTilesWithinSteps(heroBeingMoved.coord, heroBeingMoved.maxSteps, heroBeingMoved.isAlly);

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
            GridCoord coord = GetTileUnderMouse();

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

    private GridCoord GetTileUnderMouse()
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

        return GridCoord.Zero;
    }
}