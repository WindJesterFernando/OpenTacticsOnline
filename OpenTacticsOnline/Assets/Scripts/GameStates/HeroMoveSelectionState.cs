//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveSeletionState : AbstractGameState
{
    private Hero heroBeingMoved;

    private LinkedList<Vector2Int> tilesThatCanBeMovedTo;
    
    public HeroMoveSeletionState(Hero heroBeingMoved)
        : base(GameState.MoveSelection)
    {
        this.heroBeingMoved = heroBeingMoved;
    }

    public override void OnStateEnter()
    {
        
        LinkedList<Vector2Int> tilesWithinSteps = BattleGridModelData.GetTilesWithinSteps(heroBeingMoved.coord, heroBeingMoved.maxSteps);

        tilesThatCanBeMovedTo = new LinkedList<Vector2Int>();
        
        foreach (Vector2Int t in tilesWithinSteps)
        {
            Hero h = BattleGridModelData.GetHeroAtCoord(t);

            if (h == null)
            {
                GridVisuals.ChangeColorOfTile(t, Color.magenta);
                tilesThatCanBeMovedTo.AddLast(t);
            }
        }
    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int coord = BattleGridModelData.GetTileUnderMouse();

            if (tilesThatCanBeMovedTo.Contains(coord))
            {
                StateManager.PushGameState(new HeroMovementState(heroBeingMoved, coord));
                heroBeingMoved.coord = coord;
            }
            else
            {
                StateManager.PopGameState();
            }
            
            foreach (Vector2Int t in tilesThatCanBeMovedTo)
            {
                GridVisuals.ChangeColorOfTile(t, Color.white);
            }
        }
    }
}