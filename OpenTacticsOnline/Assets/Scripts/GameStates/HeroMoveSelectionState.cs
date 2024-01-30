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
        //base.OnStateEnter();
        
        LinkedList<Vector2Int> tilesWithinSteps = BattleGridModelData.GetTilesWithinSteps(new Vector2Int(heroBeingMoved.x, heroBeingMoved.y), heroBeingMoved.maxSteps);

        tilesThatCanBeMovedTo = new LinkedList<Vector2Int>();
        
        foreach (Vector2Int t in tilesWithinSteps)
        {
            Hero h = BattleGridModelData.GetHeroAtCoord(t);

            if (h == null)
            {
                GridVisuals.ChangeColorOfTile(t, Color.magenta);
                tilesThatCanBeMovedTo.AddLast(t);
            }
            
            //BattleGridModelData.ChangeTileID(t, 4);
        }

    }

    public override void OnStateExit()
    {
        
    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int coord = BattleGridModelData.GetTileUnderMouse();

            if (tilesThatCanBeMovedTo.Contains(coord))
            {
                StateManager.PushGameState(new HeroMovementState(heroBeingMoved, coord));
                //move hero, load hero movement state
                
                //call A*
                //...
                //

                heroBeingMoved.x = coord.x;
                heroBeingMoved.y = coord.y;
            }
            else
            {
                //pop state
                StateManager.PopGameState();
                
            }
            
            foreach (Vector2Int t in tilesThatCanBeMovedTo)
            {
                GridVisuals.ChangeColorOfTile(t, Color.white);
            }
        }
        
        
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //
        //     //foreach (GameObject bgt in GridVisuals.GetTileVisuals())
        //
        //     GameObject[,] tileVisuals = GridVisuals.GetTileVisuals();
        //
        //     for (int x = 0; x < BattleGridModelData.gridSizeX; x++)
        //     {
        //         for (int y = 0; y < BattleGridModelData.gridSizeY; y++)
        //         {
        //             GameObject bgt = tileVisuals[x, y];
        //             Bounds b = bgt.GetComponent<SpriteRenderer>().bounds;
        //
        //             mouseWorldPoint.z = b.center.z;
        //
        //             if (b.Contains(mouseWorldPoint))
        //             {
        //                 Debug.Log("Tile Hit @ " + x + "," + y);
        //             }
        //         }
        //     }
        // }
    }
}