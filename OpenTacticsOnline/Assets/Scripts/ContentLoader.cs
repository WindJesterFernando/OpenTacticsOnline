using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContentLoader
{
    static Sprite[] mapTileSprites;
    static GameObject containerParent;
    
    static GameObject[,] tileVisuals;

    public static void Init()
    {
        mapTileSprites = Resources.LoadAll<Sprite>("tileSet_64x64");
    }

    public static GameObject CreateGridTile(int spriteID)
    {
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = mapTileSprites[spriteID];
        return go;
    }

    public static void CreateBattleGridVisuals(BattleGridTile[,] battleGridTiles)
    {
        
        containerParent = new GameObject("TileContainerParent");

        int xLength = battleGridTiles.GetLength(0);
        int yLength = battleGridTiles.GetLength(1);

        tileVisuals = new GameObject[xLength, yLength];

        float xOffSet = -((float)xLength - 1f) / 2f;
        float yOffSet = -((float)yLength - 1f) / 2f;

        foreach (BattleGridTile bgt in battleGridTiles)
        {
            GameObject tile;
            tile = ContentLoader.CreateGridTile(bgt.id);
            tile.transform.position = new Vector3(bgt.x + xOffSet, bgt.y + yOffSet, 0);
            tile.transform.parent = containerParent.transform;
            tileVisuals[bgt.x, bgt.y] = tile;
        }

    }

    public static void DestroyBattleGridVisuals()
    {
        GameObject.Destroy(containerParent);
        tileVisuals = null;
    }


    public static GameObject UpdateGridTileSprite(int x, int y, int spriteID)
    {
        GameObject t = tileVisuals[x,y];
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        sr.sprite = mapTileSprites[spriteID];
        return t;
    }

}

