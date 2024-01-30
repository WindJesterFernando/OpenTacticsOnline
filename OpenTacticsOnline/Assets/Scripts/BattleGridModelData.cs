using System;
using System.Collections;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data.Common;
using System.Linq;
using UnityEngine;

public static partial class BattleGridModelData
{
    public const int gridSizeX = 20, gridSizeY = 10;
    static BattleGridTile[,] battleGridTiles;
    private static LinkedList<Hero> heroes;

    public static void Init()
    {
        #region Setup & isWalkable Default

        battleGridTiles = new BattleGridTile[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                battleGridTiles[x, y].isWalkable = true;
            }
        }

        #endregion

        #region Maze Content

        battleGridTiles[1, 0].isWalkable = false;
        battleGridTiles[1, 1].isWalkable = false;
        battleGridTiles[1, 2].isWalkable = false;
        battleGridTiles[1, 3].isWalkable = false;
        battleGridTiles[1, 4].isWalkable = false;
        battleGridTiles[1, 5].isWalkable = false;
        battleGridTiles[1, 6].isWalkable = false;

        //battleGridTiles[0, 7].isWalkable = false;

        battleGridTiles[0, 8].isWalkable = false;
        battleGridTiles[1, 8].isWalkable = false;
        battleGridTiles[2, 8].isWalkable = false;
        battleGridTiles[3, 8].isWalkable = false;
        battleGridTiles[4, 8].isWalkable = false;

        //battleGridTiles[7, 9].isWalkable = false;
        battleGridTiles[7, 8].isWalkable = false;
        battleGridTiles[7, 7].isWalkable = false;
        battleGridTiles[7, 6].isWalkable = false;
        battleGridTiles[7, 5].isWalkable = false;
        battleGridTiles[7, 4].isWalkable = false;
        battleGridTiles[7, 3].isWalkable = false;
        battleGridTiles[7, 2].isWalkable = false;
        battleGridTiles[7, 1].isWalkable = false;

        // battleGridTiles[8, 3].isWalkable = false;
        // battleGridTiles[9, 3].isWalkable = false;
        // battleGridTiles[10, 3].isWalkable = false;
        // battleGridTiles[11, 3].isWalkable = false;
        // battleGridTiles[12, 3].isWalkable = false;
        // battleGridTiles[13, 3].isWalkable = false;
        //battleGridTiles[14, 3].isWalkable = false;

        battleGridTiles[8, 7].isWalkable = false;
        battleGridTiles[9, 7].isWalkable = false;
        battleGridTiles[10, 7].isWalkable = false;
        battleGridTiles[11, 7].isWalkable = false;
        battleGridTiles[12, 7].isWalkable = false;
        battleGridTiles[13, 7].isWalkable = false;
        // battleGridTiles[14, 7].isWalkable = false;
        // battleGridTiles[15, 7].isWalkable = false;
        // battleGridTiles[16, 7].isWalkable = false;

        battleGridTiles[11, 6].isWalkable = false;
        battleGridTiles[11, 5].isWalkable = false;
        battleGridTiles[11, 4].isWalkable = false;

        #endregion

        SetAllTilesToDefault();

        heroes = new LinkedList<Hero>();

        Hero h = new Hero(2, 2, 1, 6);
        heroes.AddLast(h);
        
        h = new Hero(15, 6, 1, 8);
        heroes.AddLast(h);
        
    }

    public static BattleGridTile[,] GetBattleGridTiles()
    {
        return battleGridTiles;
    }

    public static void ChangeTileID(Vector2Int coord, int newID)
    {
        battleGridTiles[coord.x, coord.y].id = newID;
        GridVisuals.UpdateGridTileSprite(coord, newID);
    }

    private static void SetAllTilesToDefault()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                battleGridTiles[x, y].x = x;
                battleGridTiles[x, y].y = y;

                if (battleGridTiles[x, y].isWalkable)
                    ChangeTileID(new Vector2Int(x, y), 48);
                else
                    ChangeTileID(new Vector2Int(x, y), 54);

                //     battleGridTiles[x, y].id = 1;
                // else
                //     battleGridTiles[x, y].id = 54;
            }
        }
    }

    public static LinkedList<Hero> GetHeroes()
    {
        return heroes;
    }
    
}

public struct BattleGridTile
{
    public int x, y;
    public int id;
    public bool isWalkable;
}

public class Hero
{
    public int x, y;
    public int id;
    public int maxSteps;

    public Hero(int x, int y, int id, int maxSteps)
    {
        this.x = x;
        this.y = y;
        this.id = id;
        this.maxSteps = maxSteps;
    }
    
}