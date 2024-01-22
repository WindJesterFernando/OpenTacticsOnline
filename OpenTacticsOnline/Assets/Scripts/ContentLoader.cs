using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContentLoader
{
    static Sprite[] mapTileSprites;


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

    public static Sprite GetMapTileSprite(int spriteID)
    {
        return mapTileSprites[spriteID];
    }

}

