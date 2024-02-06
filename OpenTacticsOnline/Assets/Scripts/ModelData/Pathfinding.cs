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

    public static LinkedList<Vector2Int> DoTheAStarThingMyGuy(Vector2Int start, Vector2Int end, bool isPlayerTeam)
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

        foreach (Vector2Int neighbour in GetTraversableNeighboursTiles(start, isPlayerTeam))
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

            // ActionQueue.instance.EnqueueAction(new ActionChangeTileContainer(tileToEvaluate, 101));
            // ActionQueue.instance.EnqueueAction(new ActionWaitContainer(DelayBetweenMoves));

            neighbourTiles.Remove(tileToEvaluate);
            visitedTiles.AddLast(tileToEvaluate);

            foreach (Vector2Int neighbour in GetTraversableNeighboursTiles(tileToEvaluate, isPlayerTeam))
            {
                if (!neighbourTiles.Contains(neighbour) && !visitedTiles.Contains(neighbour))
                {
                    neighbourTiles.AddLast(neighbour);
                    // ActionQueue.instance.EnqueueAction(new ActionChangeTileContainer(neighbour, 106));
                    // ActionQueue.instance.EnqueueAction(new ActionWaitContainer(DelayBetweenMoves));
                }

                int prevTileMoveCost = travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];
                int neighbourTileMoveCost = prevTileMoveCost + MoveCost;

                bool isDistanceUninitialized = travelDistancesFromStart[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isNewMoveCostCheaper = travelDistancesFromStart[neighbour.x, neighbour.y] > neighbourTileMoveCost;

                if (isDistanceUninitialized || isNewMoveCostCheaper)
                {
                    travelDistancesFromStart[neighbour.x, neighbour.y] = neighbourTileMoveCost;
                }

                //ActionQueue.instance.EnqueueAction(new ActionDebugLogContainer(neighbour + " : " + travelDistancesFromStart[neighbour.x, neighbour.y]));

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

            foreach (Vector2Int neighbour in GetTraversableNeighboursTiles(currentTile, isPlayerTeam))
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
        //     ActionQueue.instance.EnqueueAction(new ActionChangeTileContainer(tile, 17));
        //     ActionQueue.instance.EnqueueAction(new ActionWaitContainer(DelayBetweenMoves));
        // }

        return path;
    }

    public static int GetDistance(Vector2Int start, Vector2Int end)
    {
        Vector2Int dif = end - start;
        return Math.Abs(dif.x) + Math.Abs(dif.y);
    }

    public static LinkedList<Vector2Int> GetTraversableNeighboursTiles(Vector2Int coord, bool isPlayerTeam)
    {
        LinkedList<Vector2Int> walkableNeighbours = new LinkedList<Vector2Int>();

        Vector2Int leftCoord = coord + Vector2Int.left;
        Vector2Int rightCoord = coord + Vector2Int.right;
        Vector2Int topCoord = coord + Vector2Int.up;
        Vector2Int bottomCoord = coord + Vector2Int.down;

        if (IsTileTraversable(leftCoord, isPlayerTeam))
        {
            walkableNeighbours.AddLast(leftCoord);
        }

        if (IsTileTraversable(bottomCoord, isPlayerTeam))
        {
            walkableNeighbours.AddLast(bottomCoord);
        }

        if (IsTileTraversable(rightCoord, isPlayerTeam))
        {
            walkableNeighbours.AddLast(rightCoord);
        }

        if (IsTileTraversable(topCoord, isPlayerTeam))
        {
            walkableNeighbours.AddLast(topCoord);
        }

        return walkableNeighbours;
    }

    private static bool IsTileTraversable(Vector2Int coord, bool isPlayerTeam)
    {
        bool isTileInBounds;
        bool isTileWalkable;
        bool isTileEmpty;
        bool isTileBlockedByFoe;
        
        bool inBoundsX = coord.x >= 0 && coord.x < gridSizeX;
        bool inBoundsY = coord.y >= 0 && coord.y < gridSizeY;
        isTileInBounds = inBoundsX && inBoundsY;

        if (!isTileInBounds) 
            return false;

        isTileWalkable = battleGridTiles[coord.x, coord.y].isWalkable; 
        
        if (!isTileWalkable)
            return false;

        Hero hero = GetHeroAtCoord(coord);
        isTileEmpty = (hero == null);

        if (isTileEmpty) 
            return true;

        
        isTileBlockedByFoe = hero.isAlly != isPlayerTeam;

        if (isTileBlockedByFoe)
            return false;
        else
            return true;
    }
    
    public static LinkedList<Vector2Int> GetTilesWithinSteps(Vector2Int start, int steps, bool isPlayerTeam)
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

        foreach (Vector2Int neighbour in GetTraversableNeighboursTiles(start, isPlayerTeam))
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

            foreach (Vector2Int neighbour in GetTraversableNeighboursTiles(tileToEvaluate, isPlayerTeam))
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

    // TODO think about name 
    public static LinkedList<Vector2Int> GetNonOccupiedTilesWithinSteps(Vector2Int start, int steps, bool isPlayerTeam)
    {
        LinkedList<Vector2Int> tilesWithinSteps = GetTilesWithinSteps(start, steps, isPlayerTeam);
        foreach (Hero h in heroes)
        {
            if (tilesWithinSteps.Contains(h.coord))
            {
                tilesWithinSteps.Remove(h.coord);
            }
        }

        return tilesWithinSteps;
    }
}