using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystemBooter : MonoBehaviour
{

    void Start()
    {
        StateManager.Init();

        BattleGridModelData.Init();

        ContentLoader.Init();

        StateManager.PushGameState(new TitleState());

        GridVisuals.CreateBattleGridVisuals(BattleGridModelData.GetBattleGridTiles());

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BattleGridModelData.DoTheAStarThingMyGuy(new Vector2Int(0, 0), new Vector2Int(10, 5));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(BattleGridModelData.GetDistance(new Vector2Int(0, 0), new Vector2Int(19, 9)));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Vector2Int t in BattleGridModelData.GetWalkableNeighbours(new Vector2Int(0, 0)))
                Debug.Log(t);

            Debug.Log("------");

            foreach (Vector2Int t in BattleGridModelData.GetWalkableNeighbours(new Vector2Int(19, 9)))
                Debug.Log(t);

            Debug.Log("------");

            foreach (Vector2Int t in BattleGridModelData.GetWalkableNeighbours(new Vector2Int(10, 5)))
                Debug.Log(t);
        }

        //StateManager.Update();

        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     GridVisuals.DestroyBattleGridVisuals();
        // }

        // if (Input.GetKeyDown(KeyCode.W))
        // {
        //     BattleGridModelData.ChangeTileID(2, 2, 7);

        //     //ContentLoader.DestroyBattleGridVisuals();
        //     //ContentLoader.CreateBattleGridVisuals(BattleGridModelData.GetBattleGridTiles());

        //     //change a single tile
        //     //
        // }
    }
}
