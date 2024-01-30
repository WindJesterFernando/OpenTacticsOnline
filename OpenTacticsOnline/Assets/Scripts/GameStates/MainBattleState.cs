using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBattleState : AbstractGameState
{
    public MainBattleState()
        : base(GameState.MainPlay)
    {
    }

    public override void Update()
    {
        //TODO: refactor!!
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //foreach (GameObject bgt in GridVisuals.GetTileVisuals())

            GameObject[,] tileVisuals = GridVisuals.GetTileVisuals();

            foreach (Hero h in BattleGridModelData.GetHeroes())
            {
                GameObject bgt = tileVisuals[h.x, h.y];
                Bounds b = bgt.GetComponent<SpriteRenderer>().bounds;

                mouseWorldPoint.z = b.center.z;

                if (b.Contains(mouseWorldPoint))
                {
                    Debug.Log("Tile Hit @ " + h.x + "," + h.y);
                    StateManager.PushGameState(new HeroMoveSeletionState(h));
                }
            }
        }
    }
}