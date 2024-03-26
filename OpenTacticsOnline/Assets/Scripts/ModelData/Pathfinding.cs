using System;
using System.Collections.Generic;

public static partial class BattleGridModelData
{
    private const int MoveCost = 1;
    private const int UninitializedDistance = -1;
    private const int TileIsBeingBlocked = -2;


    private static int[,] SetupStepCosts(PathBlocker pathBlockers)
    {
        int[,] stepCosts = new int[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (IsTileBlocked(new GridCoord(x, y), pathBlockers))
                {
                    stepCosts[x, y] = TileIsBeingBlocked;
                    //UnityEngine.Debug.Log("setting blocked");
                }
                else
                {
                    stepCosts[x, y] = UninitializedDistance;
                    //UnityEngine.Debug.Log("Not blocked");
                }
            }
        }

        return stepCosts;
    }



    public static List<GridCoord> DoTheAStarThingMyGuy(GridCoord start, GridCoord end, bool isPlayerTeam)
    {
        PathBlocker pathBlocker;

        if (isPlayerTeam)
            pathBlocker = PathBlocker.Foe;
        else
            pathBlocker = PathBlocker.Ally;

        pathBlocker = pathBlocker | PathBlocker.Terrain;

        int[,] stepCosts = SetupStepCosts(pathBlocker);

        LinkedList<GridCoord> visitedTiles = new LinkedList<GridCoord>();
        LinkedList<GridCoord> neighbourTiles = new LinkedList<GridCoord>();

        visitedTiles.AddLast(start);
        stepCosts[start.x, start.y] = 0;

        foreach (GridCoord neighbour in GetTraversableNeighboursTiles(start, stepCosts))
        {
            neighbourTiles.AddLast(neighbour);
            stepCosts[neighbour.x, neighbour.y] = MoveCost;
        }

        bool isFound = false;

        while (neighbourTiles.Count > 0)
        {
            GridCoord tileToEvaluate = neighbourTiles.First.Value;
            int currentlyFoundMinDistance = GetDistance(tileToEvaluate, end) + stepCosts[tileToEvaluate.x, tileToEvaluate.y];

            foreach (GridCoord neighbour in neighbourTiles)
            {
                int neighbourDistance = GetDistance(neighbour, end) + stepCosts[neighbour.x, neighbour.y];

                if (neighbourDistance < currentlyFoundMinDistance)
                {
                    currentlyFoundMinDistance = neighbourDistance;
                    tileToEvaluate = neighbour;
                }
            }

            neighbourTiles.Remove(tileToEvaluate);
            visitedTiles.AddLast(tileToEvaluate);

            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(tileToEvaluate, stepCosts))
            {
                if (!neighbourTiles.Contains(neighbour) && !visitedTiles.Contains(neighbour))
                {
                    neighbourTiles.AddLast(neighbour);
                }

                int prevTileMoveCost = stepCosts[tileToEvaluate.x, tileToEvaluate.y];
                int neighbourTileMoveCost = prevTileMoveCost + MoveCost;

                bool isBlockingTile = stepCosts[neighbour.x, neighbour.y] == TileIsBeingBlocked;
                
                bool isDistanceUninitialized = stepCosts[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isNewMoveCostCheaper = stepCosts[neighbour.x, neighbour.y] > neighbourTileMoveCost;

                if (!isBlockingTile && (isDistanceUninitialized || isNewMoveCostCheaper))
                {
                    stepCosts[neighbour.x, neighbour.y] = neighbourTileMoveCost;
                    UnityEngine.Debug.Log("Setting cost == " + neighbourTileMoveCost);
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
        {
            UnityEngine.Debug.Log("returning null, path not found");
            return null;
        }

        GridCoord currentTile = end;
        LinkedList<GridCoord> path = new LinkedList<GridCoord>();

        while (currentTile != start)
        {
            path.AddFirst(currentTile);
            int smallestDistanceToStart = Int32.MaxValue;
            GridCoord nextCoord = new GridCoord();

            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(currentTile, stepCosts))
            {
                bool isBlockingTile = stepCosts[neighbour.x, neighbour.y] == TileIsBeingBlocked;
                bool isDistanceUninitialized = stepCosts[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isCurrentPathLonger = smallestDistanceToStart <= stepCosts[neighbour.x, neighbour.y];

                if (isDistanceUninitialized || isCurrentPathLonger || isBlockingTile)
                    continue;

                smallestDistanceToStart = stepCosts[neighbour.x, neighbour.y];
                nextCoord = neighbour;
            }

            currentTile = nextCoord;
        }

        List<GridCoord> pathAsList = new List<GridCoord>(path);

        return pathAsList;
    }

    private static LinkedList<GridCoord> GetTilesWithinSteps(GridCoord start, int steps, PathBlocker blocker)
    {
        int[,] stepCosts = SetupStepCosts(blocker);

        LinkedList<GridCoord> visitedTiles = new LinkedList<GridCoord>();
        LinkedList<GridCoord> neighbourTiles = new LinkedList<GridCoord>();

        visitedTiles.AddLast(start);
        stepCosts[start.x, start.y] = 0;

        foreach (GridCoord neighbour in GetTraversableNeighboursTiles(start, stepCosts))
        {
            neighbourTiles.AddLast(neighbour);
            stepCosts[neighbour.x, neighbour.y] = MoveCost;
        }

        while (neighbourTiles.Count > 0)
        {
            GridCoord tileToEvaluate = neighbourTiles.First.Value;
            int currentlyFoundMinDistance = stepCosts[tileToEvaluate.x, tileToEvaluate.y];

            foreach (GridCoord neighbour in neighbourTiles)
            {
                int neighbourDistance = stepCosts[neighbour.x, neighbour.y];

                if (neighbourDistance < currentlyFoundMinDistance)
                {
                    currentlyFoundMinDistance = neighbourDistance;
                    tileToEvaluate = neighbour;
                }
            }

            neighbourTiles.Remove(tileToEvaluate);
            visitedTiles.AddLast(tileToEvaluate);

            foreach (GridCoord neighbour in GetTraversableNeighboursTiles(tileToEvaluate, stepCosts))
            {
                if (!neighbourTiles.Contains(neighbour) && !visitedTiles.Contains(neighbour))
                {
                    neighbourTiles.AddLast(neighbour);
                }

                int prevTileMoveCost = stepCosts[tileToEvaluate.x, tileToEvaluate.y];
                int neighbourTileMoveCost = prevTileMoveCost + MoveCost;


                bool isBlockingTile = stepCosts[neighbour.x, neighbour.y] == TileIsBeingBlocked;
                bool isDistanceUninitialized = stepCosts[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isNewMoveCostCheaper = stepCosts[neighbour.x, neighbour.y] > neighbourTileMoveCost;

                if(isBlockingTile)
                    continue;

                if (isDistanceUninitialized || isNewMoveCostCheaper)
                {
                    stepCosts[neighbour.x, neighbour.y] = neighbourTileMoveCost;
                }
            }
        }

        LinkedList<GridCoord> tilesWithinSteps = new LinkedList<GridCoord>();

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (stepCosts[x, y] == UninitializedDistance)
                    continue;
                if(stepCosts[x, y] == TileIsBeingBlocked)
                    continue;
                if (stepCosts[x, y] > steps)
                    continue;
                
                tilesWithinSteps.AddLast(new GridCoord(x, y));
            }
        }

        return tilesWithinSteps;
    }



    public static List<GridCoord> GetStuffInArea(GridCoord start, int steps, PathfindingOptions options)
    {
        LinkedList<GridCoord> tilesWithinSteps = GetTilesWithinSteps(start, steps, options.pathBlockers);

        if (!options.canTargetSelf)
            tilesWithinSteps.Remove(start);

        LinkedList<GridCoord> filteredByType = FilterByType(tilesWithinSteps, options.targetType);

        List<GridCoord> resultAsList = new List<GridCoord>(filteredByType);

        return resultAsList;
    }


    private static LinkedList<GridCoord> GetTraversableNeighboursTiles(GridCoord coord, int[,] stepCosts)//PathBlocker blocker)
    {
        LinkedList<GridCoord> walkableNeighbours = new LinkedList<GridCoord>();

        GridCoord leftCoord = coord + GridCoord.Left;
        GridCoord rightCoord = coord + GridCoord.Right;
        GridCoord topCoord = coord + GridCoord.Up;
        GridCoord bottomCoord = coord + GridCoord.Down;

        if (IsTileInBounds(leftCoord) && stepCosts[leftCoord.x, leftCoord.y] != TileIsBeingBlocked)//IsTileTraversable(leftCoord, blocker))
        {
            walkableNeighbours.AddLast(leftCoord);
        }

        if (IsTileInBounds(bottomCoord) && stepCosts[bottomCoord.x, bottomCoord.y] != TileIsBeingBlocked)//IsTileTraversable(bottomCoord, blocker))
        {
            walkableNeighbours.AddLast(bottomCoord);
        }

        if (IsTileInBounds(rightCoord) && stepCosts[rightCoord.x, rightCoord.y] != TileIsBeingBlocked)//IsTileTraversable(rightCoord, blocker))
        {
            walkableNeighbours.AddLast(rightCoord);
        }

        if (IsTileInBounds(topCoord) && stepCosts[topCoord.x, topCoord.y] != TileIsBeingBlocked)//IsTileTraversable(topCoord, blocker))
        {
            walkableNeighbours.AddLast(topCoord);
        }

        UnityEngine.Debug.Log("walkableNeighbours == " + walkableNeighbours.Count);

        return walkableNeighbours;
    }


    private static bool IsTileInBounds(GridCoord coord)
    {
        bool inBoundsX = coord.x >= 0 && coord.x < gridSizeX;
        bool inBoundsY = coord.y >= 0 && coord.y < gridSizeY;
        bool isTileInBounds = inBoundsX && inBoundsY;

        return isTileInBounds;
    }

    private static bool IsTileBlocked(GridCoord coord, PathBlocker blocker)
    {
        //bool isTileInBounds;
        bool isTileWalkable;
        bool isTileEmpty;
        bool isTileBlockedByFoe;
        bool isTileBlockedByAlly;

        // bool inBoundsX = coord.x >= 0 && coord.x < gridSizeX;
        // bool inBoundsY = coord.y >= 0 && coord.y < gridSizeY;
        // isTileInBounds = inBoundsX && inBoundsY;

        // if (!isTileInBounds)
        //     return false;

        if ((blocker & PathBlocker.Terrain) != 0)
            isTileWalkable = battleGridTiles[coord.x, coord.y].isWalkable;
        else
            isTileWalkable = true;

        if (!isTileWalkable)
            return true;

        if ((blocker & (PathBlocker.Ally | PathBlocker.Foe)) == 0)
        {
            return false;
        }

        Hero hero = GetHeroAtCoord(coord);
        isTileEmpty = (hero == null);

        if (isTileEmpty)
            return false;

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
            return true;
        if (isTileBlockedByFoe)
            return true;

        return false;
    }

    public static int GetDistance(GridCoord start, GridCoord end)
    {
        GridCoord dif = end - start;
        return Math.Abs(dif.x) + Math.Abs(dif.y);
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


// public class PathfindingSOmethingSomething
// {
//     public LinkedList<GridCoord> visitedTiles = new LinkedList<GridCoord>();
//     public LinkedList<GridCoord> neighbourTiles = new LinkedList<GridCoord>();

//     public int[,] travelDistancesFromStart = new int[gridSizeX, gridSizeY];
// }

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
    None = 0,
    Ally = 1 << 0,
    Foe = 1 << 1,
    Terrain = 1 << 2
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
