using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveSeletionState : AbstractGameState
{
    private Hero heroBeingMoved;
    public HeroMoveSeletionState(Hero heroBeingMoved)
        : base(GameState.MoveSelection)
    {
        this.heroBeingMoved = heroBeingMoved;
    }

    public override void OnStateEnter()
    {
        //base.OnStateEnter();
        
        LinkedList<Vector2Int> tilesWithinSteps = BattleGridModelData.GetTilesWithinSteps(new Vector2Int(heroBeingMoved.x, heroBeingMoved.y), heroBeingMoved.maxSteps);

        foreach (Vector2Int t in tilesWithinSteps)
        {
            BattleGridModelData.ChangeTileID(t, 4);
        }


    }

    public override void OnStateExit()
    {
        
    }

    public override void Update()
    {
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