using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayState : AbstractGameState
{
    public MainPlayState()
        : base(GameState.MainPlay)
    {

    }

    public override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //foreach (GameObject bgt in GridVisuals.GetTileVisuals())

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
                    }
                }
            }
        }
    }
}