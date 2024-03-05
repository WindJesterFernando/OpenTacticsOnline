using System;
using System.Collections.Generic;

public static partial class BattleGridModelData
{
    const int MoveCost = 1;
    const float DelayBetweenMoves = 0;
    const int UninitializedDistance = -1;

    public static LinkedList<GridCoord> DoTheAStarThingMyGuy(GridCoord start, GridCoord end, bool isPlayerTeam)
    {
        LinkedList<GridCoord> visitedTiles = new LinkedList<GridCoord>();
        LinkedList<GridCoord> neighbourTiles = new LinkedList<GridCoord>();

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

        foreach (GridCoord neighbour in GetTraversableNeighboursTiles(start, isPlayerTeam))
        {
            neighbourTiles.AddLast(neighbour);
            travelDistancesFromStart[neighbour.x, neighbour.y] = MoveCost;
        }

        bool isFound = false;

        while (neighbourTiles.Count > 0)
        {
            GridCoord tileToEvaluate = neighbourTiles.First.Value;
            int currentlyFoundMinDistance = GetDistance(tileToEvaluate, end) + travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];

            foreach (GridCoord neighbour in neighbourTiles)
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

            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(tileToEvaluate, isPlayerTeam))
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

        GridCoord currentTile = end;
        LinkedList<GridCoord> path = new LinkedList<GridCoord>();

        while (currentTile != start)
        {
            path.AddFirst(currentTile);
            int smallestDistanceToStart = Int32.MaxValue;
            GridCoord nextCoord = new GridCoord();

            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(currentTile, isPlayerTeam))
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

    public static int GetDistance(GridCoord start, GridCoord end)
    {
        GridCoord dif = end - start;
        return Math.Abs(dif.x) + Math.Abs(dif.y);
    }

    public static LinkedList<GridCoord> GetTraversableNeighboursTiles(GridCoord coord, bool isPlayerTeam)
    {
        LinkedList<GridCoord> walkableNeighbours = new LinkedList<GridCoord>();

        GridCoord leftCoord = coord + GridCoord.Left;
        GridCoord rightCoord = coord + GridCoord.Right;
        GridCoord topCoord = coord + GridCoord.Up;
        GridCoord bottomCoord = coord + GridCoord.Down;

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

    private static bool IsTileTraversable(GridCoord coord, bool isPlayerTeam)
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

        
        // isPlayerTeam: true == Player team
        //               false == Enemy team
        isTileBlockedByFoe = hero.isAlly != isPlayerTeam;

        if (isTileBlockedByFoe)
            return false;
        else
            return true;
    }
    
    public static LinkedList<GridCoord> GetTilesWithinSteps(GridCoord start, int steps, bool isPlayerTeam)
    {
        LinkedList<GridCoord> visitedTiles = new LinkedList<GridCoord>();
        LinkedList<GridCoord> neighbourTiles = new LinkedList<GridCoord>();

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

        foreach (GridCoord neighbour in GetTraversableNeighboursTiles(start, isPlayerTeam))
        {
            neighbourTiles.AddLast(neighbour);
            travelDistancesFromStart[neighbour.x, neighbour.y] = MoveCost;
        }

        while (neighbourTiles.Count > 0)
        {
            GridCoord tileToEvaluate = neighbourTiles.First.Value;
            int currentlyFoundMinDistance = travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];

            foreach (GridCoord neighbour in neighbourTiles)
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

            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(tileToEvaluate, isPlayerTeam))
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

        LinkedList<GridCoord> tilesWithinSteps = new LinkedList<GridCoord>();
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if(travelDistancesFromStart[x, y] == UninitializedDistance)
                    continue;
                if(travelDistancesFromStart[x, y] > steps)
                    continue;
                tilesWithinSteps.AddLast(new GridCoord(x, y));
            }
        }
        
        return tilesWithinSteps;
    }

    // TODO think about name 
    public static LinkedList<GridCoord> GetNonOccupiedTilesWithinSteps(GridCoord start, int steps, bool isPlayerTeam)
    {
        LinkedList<GridCoord> tilesWithinSteps = GetTilesWithinSteps(start, steps, isPlayerTeam);
        foreach (Hero h in heroes)
        {
            if (tilesWithinSteps.Contains(h.coord))
            {
                tilesWithinSteps.Remove(h.coord);
            }
        }

        return tilesWithinSteps;
    }
    
    public static LinkedList<GridCoord> GetHeroesWithinSteps(GridCoord start, int steps, bool isTargetingFoe)
    {
         LinkedList<GridCoord> tilesWithinSteps = GetTilesWithinSteps(start, steps, !isTargetingFoe);
         LinkedList<GridCoord> targetableHeroes = new LinkedList<GridCoord>();
         foreach (Hero h in heroes)
         {
             if (h.coord == start)
                 continue;
             
             if (tilesWithinSteps.Contains(h.coord))
             {
                 targetableHeroes.AddLast(h.coord);
             }
         }
 
         return targetableHeroes;
    }
     
    // target self                    : yes/no
    // target                         : ally/foe/anyone/empty/anytile
    // can be blocked by              : ally/foe/anyone/none
    // can be blocked by terrain      : yes/no
    // need line of sight             : yes/no
}
