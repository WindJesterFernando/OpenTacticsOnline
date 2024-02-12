using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridVisuals
{
    static GameObject containerTileParent;
    static GameObject containerHeroParent;

    static GameObject[,] tileVisuals;
    private static LinkedList<GameObject> heroVisuals;

    public static void CreateBattleGridVisuals(BattleGridTile[,] battleGridTiles)
    {
        containerTileParent = new GameObject("TileContainerParent");
        containerHeroParent = new GameObject("HeroContainerParent");

        int xLength = battleGridTiles.GetLength(0);
        int yLength = battleGridTiles.GetLength(1);

        tileVisuals = new GameObject[xLength, yLength];

        float xOffSet = -((float)xLength - 1f) / 2f;
        float yOffSet = -((float)yLength - 1f) / 2f;

        foreach (BattleGridTile bgt in battleGridTiles)
        {
            GameObject tile;
            tile = ContentLoader.CreateGridTile(bgt.id);
            tile.transform.position = new Vector3(bgt.coord.x + xOffSet, bgt.coord.y + yOffSet, 0);
            tile.transform.parent = containerTileParent.transform;
            tileVisuals[bgt.coord.x, bgt.coord.y] = tile;
        }

        heroVisuals = new LinkedList<GameObject>();
        foreach (Hero h in BattleGridModelData.GetHeroes())
        {
            GameObject hGameObject = ContentLoader.CreateAnimatedSprite(h.jobClass);

            if (!h.isAlly)
            {
                hGameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            
            hGameObject.transform.position = new Vector3(h.coord.x + xOffSet, h.coord.y + yOffSet, 0);
            hGameObject.transform.parent = containerHeroParent.transform;
            heroVisuals.AddLast(hGameObject);
            h.visualRepresentation = hGameObject;
        }
    }

    public static void DestroyBattleGridVisuals()
    {
        foreach (Hero h in BattleGridModelData.GetHeroes())
            h.visualRepresentation = null;
        
        heroVisuals.Clear();
        heroVisuals = null;
        
        GameObject.Destroy(containerTileParent);
        GameObject.Destroy(containerHeroParent);
        
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

    public static GameObject[,] GetTileVisuals()
    {
        return tileVisuals;
    }

    public static LinkedList<GameObject> GetHeroVisuals()
    {
        return heroVisuals;
    }

    public static void ChangeColorOfTile(Vector2Int coord, Color color)
    {
        tileVisuals[coord.x, coord.y].GetComponent<SpriteRenderer>().color = color;
    }
    
}