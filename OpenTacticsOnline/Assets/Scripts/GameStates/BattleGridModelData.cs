using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public static class BattleGridModelData
{
    const int gridSizeX = 20, gridSizeY = 10;

    static BattleGridTile[,] battleGridTiles;

    const int MoveCost = 1;


    public static void Init()
    {
        battleGridTiles = new BattleGridTile[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                battleGridTiles[x, y].isWalkable = true;
            }
        }


        battleGridTiles[1, 0].isWalkable = false;
        battleGridTiles[1, 1].isWalkable = false;
        battleGridTiles[1, 2].isWalkable = false;
        battleGridTiles[1, 3].isWalkable = false;
        battleGridTiles[1, 4].isWalkable = false;
        battleGridTiles[1, 5].isWalkable = false;
        battleGridTiles[1, 6].isWalkable = false;

        battleGridTiles[0, 8].isWalkable = false;
        battleGridTiles[1, 8].isWalkable = false;
        battleGridTiles[2, 8].isWalkable = false;
        battleGridTiles[3, 8].isWalkable = false;
        battleGridTiles[4, 8].isWalkable = false;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                battleGridTiles[x, y].x = x;
                battleGridTiles[x, y].y = y;

                if (battleGridTiles[x, y].isWalkable)
                    battleGridTiles[x, y].id = 1;
                else
                    battleGridTiles[x, y].id = 54;
            }
        }

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

    public static void DoTheAStarThingMyGuy(Vector2Int start, Vector2Int end)
    {
        ChangeTileID(start, 104);
        ChangeTileID(end, 107);
    }

    public static int GetDistance(Vector2Int start, Vector2Int end)
    {
        Vector2Int dif = end - start;
        return Math.Abs(dif.x) + Math.Abs(dif.y);
    }

    public static LinkedList<Vector2Int> GetWalkableNeighbours(Vector2Int coord)
    {
        LinkedList<Vector2Int> walkableNeighbours = new LinkedList<Vector2Int>();

        Vector2Int leftCoord = new Vector2Int(coord.x - 1, coord.y);
        Vector2Int rightCoord = new Vector2Int(coord.x + 1, coord.y);
        Vector2Int topCoord = new Vector2Int(coord.x, coord.y + 1);
        Vector2Int bottomCoord = new Vector2Int(coord.x, coord.y - 1);

        if (coord.x > 0)
        {
            if (battleGridTiles[leftCoord.x, leftCoord.y].isWalkable)
            {
                walkableNeighbours.AddLast(leftCoord);
            }
        }

        if (coord.y > 0)
        {
            if (battleGridTiles[bottomCoord.x, bottomCoord.y].isWalkable)
            {
                walkableNeighbours.AddLast(bottomCoord);
            }
        }

        if (coord.x < gridSizeX - 1)
        {
            if (battleGridTiles[rightCoord.x, rightCoord.y].isWalkable)
            {
                walkableNeighbours.AddLast(rightCoord);
            }
        }

        if (coord.y < gridSizeY - 1)
        {
            if (battleGridTiles[topCoord.x, topCoord.y].isWalkable)
            {
                walkableNeighbours.AddLast(topCoord);
            }
        }

        return walkableNeighbours;

    }

}

public struct BattleGridTile
{
    public int x, y;
    public int id;
    public bool isWalkable;

}

