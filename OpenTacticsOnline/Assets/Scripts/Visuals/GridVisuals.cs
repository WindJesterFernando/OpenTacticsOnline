using System.Collections.Generic;
using UnityEngine;

public static class GridVisuals
{
    static GameObject tileParent;
    static GameObject heroParent;

    static GameObject[,] tileVisuals;
    private static LinkedList<GameObject> heroVisuals;

    public static void CreateBattleGridVisuals(BattleGridTile[,] battleGridTiles)
    {
        tileParent = new GameObject("TileParent");
        heroParent = new GameObject("HeroParent");

        int xLength = battleGridTiles.GetLength(0);
        int yLength = battleGridTiles.GetLength(1);

        tileVisuals = new GameObject[xLength, yLength];

        float xOffSet = -(xLength - 1) / 2f;
        float yOffSet = -(yLength - 1) / 2f;

        foreach (BattleGridTile bgt in battleGridTiles)
        {
            GameObject tile;
            tile = ContentLoader.CreateGridTile(bgt.id);
            tile.name = "Tile (" + bgt.coord.x + ", " + bgt.coord.y + ")";
            tile.transform.position = new Vector3(bgt.coord.x + xOffSet, bgt.coord.y + yOffSet, 0);
            tile.transform.parent = tileParent.transform;
            tileVisuals[bgt.coord.x, bgt.coord.y] = tile;
        }

        heroVisuals = new LinkedList<GameObject>();
        foreach (Hero h in BattleGridModelData.GetHeroes())
        {
            GameObject hGameObject = ContentLoader.CreateAnimatedSprite(h.role);

            if (h.isAlly)
            {
                hGameObject.name = "Ally " + h.role;
            }
            else
            {
                hGameObject.name = "Foe " + h.role;
                hGameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            
            hGameObject.transform.position = new Vector3(h.coord.x + xOffSet, h.coord.y + yOffSet, 0);
            hGameObject.transform.parent = heroParent.transform;
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
        
        GameObject.Destroy(tileParent);
        GameObject.Destroy(heroParent);
        
        tileVisuals = null;
    }

    public static GameObject UpdateGridTileSprite(GridCoord coord, int spriteID)
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

    public static void ChangeColorOfTile(GridCoord coord, Color color)
    {
        tileVisuals[coord.x, coord.y].GetComponent<SpriteRenderer>().color = color;
    }
    
}