using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using UnityEngine;

public static class BattleGridModelData
{
    const int gridSizeX = 20, gridSizeY = 10;
    static BattleGridTile[,] battleGridTiles;

    const int MoveCost = 1;

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
        SetAllTilesToDefault();

        ChangeTileID(start, 104);
        ChangeTileID(end, 107);

        LinkedList<Vector2Int> visitedTiles = new LinkedList<Vector2Int>();
        LinkedList<Vector2Int> neighbourTiles = new LinkedList<Vector2Int>();

        int[,] travelDistancesFromStart = new int[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                travelDistancesFromStart[x,y] = -1;
            }
        }


        visitedTiles.AddLast(start);
        travelDistancesFromStart[start.x, start.y] = 0;

        foreach (Vector2Int n in GetWalkableNeighbours(start))
        {
            neighbourTiles.AddLast(n);
            travelDistancesFromStart[n.x, n.y] = MoveCost;
        }

        bool isFound = false;

        while (neighbourTiles.Count > 0)
        {
            Vector2Int tileToEvaluate = neighbourTiles.First.Value;
            int currentlyFoundMinDistance = GetDistance(tileToEvaluate, end);

            foreach (Vector2Int n in neighbourTiles)
            {
                int neighbourDistance = GetDistance(n, end);

                if (neighbourDistance < currentlyFoundMinDistance)
                {
                    currentlyFoundMinDistance = neighbourDistance;
                    tileToEvaluate = n;
                }
            }

            //Debug.Log("tile has dist == " + GetDistance(tileToEvaluate, end));

            QueueTest.instance.EnqueueAction(new ActionChangeTileContainer(tileToEvaluate, 101));
            QueueTest.instance.EnqueueAction(new ActionWaitContainer(0.125f));

            neighbourTiles.Remove(tileToEvaluate);
            visitedTiles.AddLast(tileToEvaluate);

            foreach (Vector2Int neighbour in GetWalkableNeighbours(tileToEvaluate))
            {
                if (!neighbourTiles.Contains(neighbour) && !visitedTiles.Contains(neighbour))
                {
                    neighbourTiles.AddLast(neighbour);
                    QueueTest.instance.EnqueueAction(new ActionChangeTileContainer(neighbour, 106));
                    QueueTest.instance.EnqueueAction(new ActionWaitContainer(0.125f / 2f));
                }

                int prevTileMoveCost =  travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];
                int neighbourTileMoveCost = prevTileMoveCost + MoveCost;

                if(travelDistancesFromStart[neighbour.x, neighbour.y] == -1)
                    travelDistancesFromStart[neighbour.x, neighbour.y] = neighbourTileMoveCost;
                else if(travelDistancesFromStart[neighbour.x, neighbour.y] > neighbourTileMoveCost)
                    travelDistancesFromStart[neighbour.x, neighbour.y] = neighbourTileMoveCost;

                QueueTest.instance.EnqueueAction(new ActionDebugLogContainer(neighbour + " : " + travelDistancesFromStart[neighbour.x, neighbour.y]));

                if (end == neighbour)
                {
                    Debug.Log("Found!");
                    isFound = true;
                    break;
                }
            }

            if (isFound)
                break;
        }
    }

    public static int GetDistance(Vector2Int start, Vector2Int end)
    {
        Vector2Int dif = end - start;
        return Math.Abs(dif.x) + Math.Abs(dif.y);
    }

    public static LinkedList<Vector2Int> GetWalkableNeighbours(Vector2Int coord)
    {
        LinkedList<Vector2Int> walkableNeighbours = new LinkedList<Vector2Int>();

        Vector2Int leftCoord = coord + Vector2Int.left;
        Vector2Int rightCoord = coord + Vector2Int.right;
        Vector2Int topCoord = coord + Vector2Int.up;
        Vector2Int bottomCoord = coord + Vector2Int.down;

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

    private static void SetAllTilesToDefault()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                battleGridTiles[x, y].x = x;
                battleGridTiles[x, y].y = y;

                if (battleGridTiles[x, y].isWalkable)
                    ChangeTileID(new Vector2Int(x, y), 1);
                else
                    ChangeTileID(new Vector2Int(x, y), 54);

                //     battleGridTiles[x, y].id = 1;
                // else
                //     battleGridTiles[x, y].id = 54;
            }
        }
    }

}

public struct BattleGridTile
{
    public int x, y;
    public int id;
    public bool isWalkable;
}

