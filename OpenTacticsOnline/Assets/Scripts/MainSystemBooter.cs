using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystemBooter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StateManager.Init();

        BattleGridModelData.Init();

        ContentLoader.Init();

        StateManager.PushGameState(new TitleState());


        GridVisuals.CreateBattleGridVisuals(BattleGridModelData.GetBattleGridTiles());



        // GameObject tile;
        // tile = ContentLoader.CreateGridTile(1);
        // tile.transform.position = new Vector3(1,0,0);

        // tile = ContentLoader.CreateGridTile(1);
        // tile.transform.position = new Vector3(-1,0,0);

        // tile = ContentLoader.CreateGridTile(54);
        // tile.transform.position = new Vector3(0,0,0);

    }


    // Update is called once per frame
    void Update()
    {

        

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
