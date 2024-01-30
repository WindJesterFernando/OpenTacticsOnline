using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using UnityEngine;

public static partial class BattleGridModelData
{
    const int MoveCost = 1;
    const float DelayBetweenMoves = 0;
    const int UninitializedDistance = -1;

    public static List<Vector2Int> DoTheAStarThingMyGuy(Vector2Int start, Vector2Int end)
    {
        //SetAllTilesToDefault();

        // ChangeTileID(start, 104);
        // ChangeTileID(end, 107);

        LinkedList<Vector2Int> visitedTiles = new LinkedList<Vector2Int>();
        LinkedList<Vector2Int> neighbourTiles = new LinkedList<Vector2Int>();

        int[,] travelDistancesFromStart = new int[gridSizeX, gridSizeY];

        #region Set travelDistancesFromStart Elements to UninitializedDistance

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                travelDistancesFromStart[x, y] = UninitializedDistance;
            }
        }

        #endregion

        visitedTiles.AddLast(start);
        travelDistancesFromStart[start.x, start.y] = 0;

        foreach (Vector2Int neighbour in GetWalkableNeighbours(start))
        {
            neighbourTiles.AddLast(neighbour);
            travelDistancesFromStart[neighbour.x, neighbour.y] = MoveCost;
        }

        bool isFound = false;

        while (neighbourTiles.Count > 0)
        {
            Vector2Int tileToEvaluate = neighbourTiles.First.Value;
            int currentlyFoundMinDistance = GetDistance(tileToEvaluate, end) + travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];

            foreach (Vector2Int neighbour in neighbourTiles)
            {
                int neighbourDistance = GetDistance(neighbour, end) + travelDistancesFromStart[neighbour.x, neighbour.y];

                if (neighbourDistance < currentlyFoundMinDistance)
                {
                    currentlyFoundMinDistance = neighbourDistance;
                    tileToEvaluate = neighbour;
                }
            }

            // QueueTest.instance.EnqueueAction(new ActionChangeTileContainer(tileToEvaluate, 101));
            // QueueTest.instance.EnqueueAction(new ActionWaitContainer(DelayBetweenMoves));

            neighbourTiles.Remove(tileToEvaluate);
            visitedTiles.AddLast(tileToEvaluate);

            foreach (Vector2Int neighbour in GetWalkableNeighbours(tileToEvaluate))
            {
                if (!neighbourTiles.Contains(neighbour) && !visitedTiles.Contains(neighbour))
                {
                    neighbourTiles.AddLast(neighbour);
                    // QueueTest.instance.EnqueueAction(new ActionChangeTileContainer(neighbour, 106));
                    // QueueTest.instance.EnqueueAction(new ActionWaitContainer(DelayBetweenMoves));
                }

                int prevTileMoveCost = travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];
                int neighbourTileMoveCost = prevTileMoveCost + MoveCost;

                bool isDistanceUninitialized = travelDistancesFromStart[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isNewMoveCostCheaper = travelDistancesFromStart[neighbour.x, neighbour.y] > neighbourTileMoveCost;

                if (isDistanceUninitialized || isNewMoveCostCheaper)
                {
                    travelDistancesFromStart[neighbour.x, neighbour.y] = neighbourTileMoveCost;
                }

                //QueueTest.instance.EnqueueAction(new ActionDebugLogContainer(neighbour + " : " + travelDistancesFromStart[neighbour.x, neighbour.y]));

                if (end == neighbour)
                {
                    // Debug.Log("Found!");
                    isFound = true;
                    break;
                }
            }

            if (isFound)
                break;
        }

        if (!isFound)
            return null;

        Vector2Int currentTile = end;
        LinkedList<Vector2Int> path = new LinkedList<Vector2Int>();

        while (currentTile != start)
        {
            path.AddFirst(currentTile);
            int smallestDistanceToStart = Int32.MaxValue;
            Vector2Int nextCoord = new Vector2Int();

            foreach (Vector2Int neighbour in GetWalkableNeighbours(currentTile))
            {
                bool isDistanceUninitialized = travelDistancesFromStart[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isCurrentPathLonger = smallestDistanceToStart <= travelDistancesFromStart[neighbour.x, neighbour.y];

                if (isDistanceUninitialized || isCurrentPathLonger)
                    continue;

                smallestDistanceToStart = travelDistancesFromStart[neighbour.x, neighbour.y];
                nextCoord = neighbour;
            }

            currentTile = nextCoord;
        }

        // foreach (Vector2Int tile in path)
        // {
        //     QueueTest.instance.EnqueueAction(new ActionChangeTileContainer(tile, 17));
        //     QueueTest.instance.EnqueueAction(new ActionWaitContainer(DelayBetweenMoves));
        // }

        return path.ToList();
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

    
    public static LinkedList<Vector2Int> GetTilesWithinSteps(Vector2Int start, int steps)
    {
        LinkedList<Vector2Int> visitedTiles = new LinkedList<Vector2Int>();
        LinkedList<Vector2Int> neighbourTiles = new LinkedList<Vector2Int>();

        int[,] travelDistancesFromStart = new int[gridSizeX, gridSizeY];

        #region Set travelDistancesFromStart Elements to UninitializedDistance

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                travelDistancesFromStart[x, y] = UninitializedDistance;
            }
        }

        #endregion

        visitedTiles.AddLast(start);
        travelDistancesFromStart[start.x, start.y] = 0;

        foreach (Vector2Int neighbour in GetWalkableNeighbours(start))
        {
            neighbourTiles.AddLast(neighbour);
            travelDistancesFromStart[neighbour.x, neighbour.y] = MoveCost;
        }

        while (neighbourTiles.Count > 0)
        {
            Vector2Int tileToEvaluate = neighbourTiles.First.Value;
            int currentlyFoundMinDistance = travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];

            foreach (Vector2Int neighbour in neighbourTiles)
            {
                int neighbourDistance = travelDistancesFromStart[neighbour.x, neighbour.y];

                if (neighbourDistance < currentlyFoundMinDistance)
                {
                    currentlyFoundMinDistance = neighbourDistance;
                    tileToEvaluate = neighbour;
                }
            }

            neighbourTiles.Remove(tileToEvaluate);
            visitedTiles.AddLast(tileToEvaluate);

            foreach (Vector2Int neighbour in GetWalkableNeighbours(tileToEvaluate))
            {
                if (!neighbourTiles.Contains(neighbour) && !visitedTiles.Contains(neighbour))
                {
                    neighbourTiles.AddLast(neighbour);
                }

                int prevTileMoveCost = travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];
                int neighbourTileMoveCost = prevTileMoveCost + MoveCost;

                bool isDistanceUninitialized =
                    travelDistancesFromStart[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isNewMoveCostCheaper =
                    travelDistancesFromStart[neighbour.x, neighbour.y] > neighbourTileMoveCost;

                if (isDistanceUninitialized || isNewMoveCostCheaper)
                {
                    travelDistancesFromStart[neighbour.x, neighbour.y] = neighbourTileMoveCost;
                }
            }
        }

        LinkedList<Vector2Int> tilesWithinSteps = new LinkedList<Vector2Int>();
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if(travelDistancesFromStart[x, y] == UninitializedDistance)
                    continue;
                if(travelDistancesFromStart[x, y] > steps)
                    continue;
                tilesWithinSteps.AddLast(new Vector2Int(x, y));
            }
        }
        
        return tilesWithinSteps;
    }

}