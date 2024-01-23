using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridVisuals
{
    static GameObject containerParent;

    static GameObject[,] tileVisuals;

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

    public static GameObject UpdateGridTileSprite(Vector2Int coord, int spriteID)
    {
        GameObject tile = null;

        if (tileVisuals != null)
        {
            tile = tileVisuals[coord.x, coord.y];
            SpriteRenderer sReader = tile.GetComponent<SpriteRenderer>();
            sReader.sprite = ContentLoader.GetMapTileSprite(spriteID);
        }
        
        return tile;
    }

}