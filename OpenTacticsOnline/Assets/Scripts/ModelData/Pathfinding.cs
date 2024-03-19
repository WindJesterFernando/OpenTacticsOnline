using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public static partial class BattleGridModelData
{
    const int MoveCost = 1;
    const float DelayBetweenMoves = 0;
    const int UninitializedDistance = -1;

    public static List<GridCoord> DoTheAStarThingMyGuy(GridCoord start, GridCoord end, bool isPlayerTeam)
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

        PathBlocker pathBlocker;
        
        if (isPlayerTeam)
            pathBlocker = PathBlocker.Foe;
        else
            pathBlocker = PathBlocker.Ally;
        
        pathBlocker = pathBlocker | PathBlocker.Terrain;
        
        foreach (GridCoord neighbour in GetTraversableNeighboursTiles(start, pathBlocker))
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

            neighbourTiles.Remove(tileToEvaluate);
            visitedTiles.AddLast(tileToEvaluate);
                   
            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(tileToEvaluate, pathBlocker))
            {
                if (!neighbourTiles.Contains(neighbour) && !visitedTiles.Contains(neighbour))
                {
                    neighbourTiles.AddLast(neighbour);
                }

                int prevTileMoveCost = travelDistancesFromStart[tileToEvaluate.x, tileToEvaluate.y];
                int neighbourTileMoveCost = prevTileMoveCost + MoveCost;

                bool isDistanceUninitialized = travelDistancesFromStart[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isNewMoveCostCheaper = travelDistancesFromStart[neighbour.x, neighbour.y] > neighbourTileMoveCost;

                if (isDistanceUninitialized || isNewMoveCostCheaper)
                {
                    travelDistancesFromStart[neighbour.x, neighbour.y] = neighbourTileMoveCost;
                }


                if (end == neighbour)
                {
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

            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(currentTile, pathBlocker))
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

        List<GridCoord> pathAsList = new List<GridCoord>(path);
        
        return pathAsList;
    }

    public static int GetDistance(GridCoord start, GridCoord end)
    {
        GridCoord dif = end - start;
        return Math.Abs(dif.x) + Math.Abs(dif.y);
    }
    
    private static LinkedList<GridCoord> GetTilesWithinSteps(GridCoord start, int steps, PathBlocker blocker)
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

        foreach (GridCoord neighbour in GetTraversableNeighboursTiles(start, blocker))
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

            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(tileToEvaluate, blocker))
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
    
    public static List<GridCoord> GetStuffInArea(GridCoord start, int steps, PathfindingOptions options)
    {
        LinkedList<GridCoord> tilesWithinSteps = GetTilesWithinSteps(start, steps,options.pathBlockers);
        if (!options.canTargetSelf) 
            tilesWithinSteps.Remove(start);
        LinkedList<GridCoord> filteredByType = FilterByType(tilesWithinSteps, options.targetType);

        List<GridCoord> resultAsList = new List<GridCoord>(filteredByType);
        
        return resultAsList;
    }

    private static LinkedList<GridCoord> FilterByType(LinkedList<GridCoord> initial, TargetType type)
    {
        if (type == TargetType.AnyTile) 
            return initial;
        else if (type == TargetType.Ally || type == TargetType.Foe || type == TargetType.AnyHero)
            return FilterHeroesInTiles(initial, type);
        else // if (type == TargetType.EmptyTile)
            return FilterEmptyTiles(initial);
    }
    private static LinkedList<GridCoord> GetTraversableNeighboursTiles(GridCoord coord, PathBlocker blocker)
    {
        LinkedList<GridCoord> walkableNeighbours = new LinkedList<GridCoord>();

        GridCoord leftCoord = coord + GridCoord.Left;
        GridCoord rightCoord = coord + GridCoord.Right;
        GridCoord topCoord = coord + GridCoord.Up;
        GridCoord bottomCoord = coord + GridCoord.Down;

        if (IsTileTraversable(leftCoord, blocker))
        {
            walkableNeighbours.AddLast(leftCoord);
        }

        if (IsTileTraversable(bottomCoord, blocker))
        {
            walkableNeighbours.AddLast(bottomCoord);
        }

        if (IsTileTraversable(rightCoord, blocker))
        {
            walkableNeighbours.AddLast(rightCoord);
        }

        if (IsTileTraversable(topCoord, blocker))
        {
            walkableNeighbours.AddLast(topCoord);
        }

        return walkableNeighbours;
    }

    private static LinkedList<GridCoord> FilterHeroesInTiles(LinkedList<GridCoord> tiles, TargetType type)
    {
        LinkedList<GridCoord> result = new LinkedList<GridCoord>();
        foreach (Hero h in heroes)
        {
            if (type == TargetType.AnyHero)
            {
                if (tiles.Contains(h.coord))
                {
                    result.AddLast(h.coord);
                }    
            }
            else if (type == TargetType.Foe)
            {
                if (!h.isAlly && tiles.Contains(h.coord))
                {
                    result.AddLast(h.coord);
                }
            }
            else if (type == TargetType.Ally)
            {
                 if (h.isAlly && tiles.Contains(h.coord))
                 {
                     result.AddLast(h.coord);
                 }   
            }
        }

        return result;
    }
    
    private static bool IsTileTraversable(GridCoord coord, PathBlocker blocker)
    {
        bool isTileInBounds;
        bool isTileWalkable;
        bool isTileEmpty;
        bool isTileBlockedByFoe;
        bool isTileBlockedByAlly;
        
        bool inBoundsX = coord.x >= 0 && coord.x < gridSizeX;
        bool inBoundsY = coord.y >= 0 && coord.y < gridSizeY;
        isTileInBounds = inBoundsX && inBoundsY;

        if (!isTileInBounds) 
            return false;

        if ((blocker & PathBlocker.Terrain) != 0)
            isTileWalkable = battleGridTiles[coord.x, coord.y].isWalkable;
        else
            isTileWalkable = true;
        
        if (!isTileWalkable)
            return false;

        if ((blocker & (PathBlocker.Ally | PathBlocker.Foe)) == 0)
        {
            return true;
        }
        
        Hero hero = GetHeroAtCoord(coord);
        isTileEmpty = (hero == null);

        if (isTileEmpty) 
            return true;

        isTileBlockedByAlly = false;
        isTileBlockedByFoe = false;
        if ((blocker & PathBlocker.Ally) == PathBlocker.Ally)
        {
            if (hero.isAlly)
                isTileBlockedByAlly = true;
        }
        if ((blocker & PathBlocker.Foe) == PathBlocker.Foe)
        {
            if (!hero.isAlly)
                isTileBlockedByFoe = true;
        }

        if (isTileBlockedByAlly)
            return false;
        if (isTileBlockedByFoe)
            return false;
        
        return true;
    }
 
    private static LinkedList<GridCoord> FilterEmptyTiles(LinkedList<GridCoord> tiles)
    {
        foreach (Hero h in heroes)
        {
            if (tiles.Contains(h.coord))
            {
                tiles.Remove(h.coord);
            }
        }

        return tiles;
    }
    
    
}

public enum TargetType
{
    None,
    Ally,
    Foe, 
    AnyHero,
    EmptyTile,
    AnyTile
}

[Flags]
public enum PathBlocker // todo 
{
    None     = 0, 
    Ally     = 1 << 0,
    Foe      = 1 << 1,
    Terrain  = 1 << 2
}
public class PathfindingOptions
{
    public bool canTargetSelf;
    public TargetType targetType;
    public PathBlocker pathBlockers;
    // public bool needLineOfSight;

    public PathfindingOptions(bool canTargetSelf, TargetType targetType, PathBlocker pathBlocker)
    {
        this.canTargetSelf = canTargetSelf;
        this.targetType = targetType;
        this.pathBlockers = pathBlocker;
    }

    public PathfindingOptions()
    {
        canTargetSelf = true;
        targetType = TargetType.None;
        pathBlockers = PathBlocker.None;
    }
}
