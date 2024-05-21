using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class BattleGridModelData
{
    private const int StepCost = 1;
    private const int UninitializedDistance = -1;
    private const int TileIsBeingBlocked = -2;
    private static readonly GridCoord EmptyGridCoord = new GridCoord(-99, -99);

    private static int[,] InitializeStepCosts()
    {
        int[,] stepCosts = new int[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                stepCosts[x, y] = UninitializedDistance;
            }
        }

        return stepCosts;
    }

    private static void SetupBlockedSteps(int[,] stepCosts, BitFlag<PathBlocker> pathBlockers)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (IsTileBlocked(new GridCoord(x, y), pathBlockers))
                {
                    stepCosts[x, y] = TileIsBeingBlocked;
                }
            }
        }
    }

    private static void PopulateStepCosts(int[,] stepCosts, GridCoord start, GridCoord end)
    {
        LinkedList<GridCoord> visitedTiles = new LinkedList<GridCoord>();
        LinkedList<GridCoord> neighbourTiles = new LinkedList<GridCoord>();

        visitedTiles.AddLast(start);
        stepCosts[start.x, start.y] = 0;

        foreach (GridCoord neighbour in GetTraversableNeighboursTiles(start, stepCosts))
        {
            neighbourTiles.AddLast(neighbour);
            stepCosts[neighbour.x, neighbour.y] = StepCost;
        }

        while (neighbourTiles.Count > 0)
        {
            GridCoord tileToEvaluate = neighbourTiles.First.Value;

            int currentMinDistance = stepCosts[tileToEvaluate.x, tileToEvaluate.y];
            if (end != EmptyGridCoord)
                currentMinDistance += GridCoord.CardinalDistance(tileToEvaluate, end);

            foreach (GridCoord neighbour in neighbourTiles)
            {
                int neighbourDistance = stepCosts[neighbour.x, neighbour.y];

                if (end != EmptyGridCoord)
                    neighbourDistance += GridCoord.CardinalDistance(neighbour, end);

                if (neighbourDistance < currentMinDistance)
                {
                    currentMinDistance = neighbourDistance;
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

                int previousTileStepCost = stepCosts[tileToEvaluate.x, tileToEvaluate.y];
                int neighbourTileStepCost = previousTileStepCost + StepCost;

                bool isBlockingTile = stepCosts[neighbour.x, neighbour.y] == TileIsBeingBlocked;
                bool isDistanceUninitialized = stepCosts[neighbour.x, neighbour.y] == UninitializedDistance;
                bool isNewStepCostCheaper = stepCosts[neighbour.x, neighbour.y] > neighbourTileStepCost;

                if (isBlockingTile)
                    continue;

                if (isDistanceUninitialized || isNewStepCostCheaper)
                {
                    stepCosts[neighbour.x, neighbour.y] = neighbourTileStepCost;
                }

                if (end == neighbour)
                {
                    return;
                }
            }
        }
    }

    public static List<GridCoord> FindShortestPath(GridCoord start, GridCoord end, bool isPlayerTeam)
    {
        int[,] stepCosts = InitializeStepCosts();

        #region Determine Path Blockers

        BitFlag<PathBlocker> pathBlockers;

        if (isPlayerTeam)
            pathBlockers = PathBlocker.Opponent;
        else
            pathBlockers = PathBlocker.Ally;

        pathBlockers += PathBlocker.Terrain;

        #endregion

        SetupBlockedSteps(stepCosts, pathBlockers);

        PopulateStepCosts(stepCosts, start, end);

        #region Find Shortest Path

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

        #endregion

        return new List<GridCoord>(path);
    }

    private static LinkedList<GridCoord> FindTilesWithinSteps(GridCoord start, int steps, BitFlag<PathBlocker> pathBlockers)
    {
        int[,] stepCosts = InitializeStepCosts();

        SetupBlockedSteps(stepCosts, pathBlockers);

        PopulateStepCosts(stepCosts, start, EmptyGridCoord);

        #region Collect Valid Tiles

        LinkedList<GridCoord> tilesWithinSteps = new LinkedList<GridCoord>();

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (stepCosts[x, y] == UninitializedDistance)
                    continue;
                if (stepCosts[x, y] == TileIsBeingBlocked)
                    continue;
                if (stepCosts[x, y] > steps)
                    continue;

                tilesWithinSteps.AddLast(new GridCoord(x, y));
            }
        }

        #endregion

        return tilesWithinSteps;
    }

    public static List<GridCoord> FindTargetsWithinSteps(GridCoord start, int steps, TargetingOptions targetingOptions)
    {
        LinkedList<GridCoord> tilesWithinSteps = FindTilesWithinSteps(start, steps, targetingOptions.pathBlockers);


        if (!targetingOptions.canTargetSelf)
            tilesWithinSteps.Remove(start);

        LinkedList<GridCoord> filteredByType = FilterByType(tilesWithinSteps, targetingOptions.targetType);

        if (targetingOptions.needLineOfSight)
            filteredByType = FilterLineOfSight(start, tilesWithinSteps);

        List<GridCoord> resultAsList = new List<GridCoord>(filteredByType);

        return resultAsList;
    }

    private static LinkedList<GridCoord> GetTraversableNeighboursTiles(GridCoord coord, int[,] stepCosts)
    {
        LinkedList<GridCoord> walkableNeighbours = new LinkedList<GridCoord>();

        GridCoord leftCoord = coord + GridCoord.Left;
        GridCoord rightCoord = coord + GridCoord.Right;
        GridCoord topCoord = coord + GridCoord.Up;
        GridCoord bottomCoord = coord + GridCoord.Down;

        if (IsTileInBounds(leftCoord) && stepCosts[leftCoord.x, leftCoord.y] != TileIsBeingBlocked)
        {
            walkableNeighbours.AddLast(leftCoord);
        }

        if (IsTileInBounds(bottomCoord) && stepCosts[bottomCoord.x, bottomCoord.y] != TileIsBeingBlocked)
        {
            walkableNeighbours.AddLast(bottomCoord);
        }

        if (IsTileInBounds(rightCoord) && stepCosts[rightCoord.x, rightCoord.y] != TileIsBeingBlocked)
        {
            walkableNeighbours.AddLast(rightCoord);
        }

        if (IsTileInBounds(topCoord) && stepCosts[topCoord.x, topCoord.y] != TileIsBeingBlocked)
        {
            walkableNeighbours.AddLast(topCoord);
        }

        return walkableNeighbours;
    }

    private static bool IsTileInBounds(GridCoord coord)
    {
        bool inBoundsX = coord.x >= 0 && coord.x < gridSizeX;
        bool inBoundsY = coord.y >= 0 && coord.y < gridSizeY;
        bool isTileInBounds = inBoundsX && inBoundsY;

        return isTileInBounds;
    }

    private static bool IsTileBlocked(GridCoord coord, BitFlag<PathBlocker> pathBlockers)
    {
        bool isTileWalkable;
        bool isTileEmpty;
        bool isTileBlockedByFoe;
        bool isTileBlockedByAlly;

        if (pathBlockers.Contains(PathBlocker.Terrain))
            isTileWalkable = battleGridTiles[coord.x, coord.y].isWalkable;
        else
            isTileWalkable = true;

        if (!isTileWalkable)
            return true;

        if (!pathBlockers.ContainsAny(PathBlocker.Ally | PathBlocker.Opponent))
        {
            return false;
        }

        Hero hero = GetHeroAtCoord(coord);
        isTileEmpty = (hero == null);

        if (isTileEmpty)
            return false;

        isTileBlockedByAlly = false;
        isTileBlockedByFoe = false;
        if (pathBlockers.Contains(PathBlocker.Ally))
        {
            if (hero.isAlly)
                isTileBlockedByAlly = true;
        }
        if (pathBlockers.Contains(PathBlocker.Opponent))
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

    private static LinkedList<GridCoord> FilterByType(LinkedList<GridCoord> initial, BitFlag<TargetType> type)
    {
        if (type == TargetType.AnyTile)
            return initial;
        else if (type.ContainsAny(TargetType.Ally | TargetType.Opponent | TargetType.KnockedOutAllies))
            return FilterHeroesInTiles(initial, type);
        else // if (type == TargetType.EmptyTile)
            return FilterEmptyTiles(initial);
    }

    private static LinkedList<GridCoord> FilterHeroesInTiles(LinkedList<GridCoord> tiles, BitFlag<TargetType> type)
    {
        LinkedList<GridCoord> result = new LinkedList<GridCoord>();
        foreach (Hero h in heroes)
        {
            if (type.Contains(TargetType.Opponent))
            {
                if (h.IsAlive() && !h.isAlly && tiles.Contains(h.coord))
                {
                    result.AddLast(h.coord);
                }
            }
            if (type.Contains(TargetType.Ally))
            {
                if (h.IsAlive() && h.isAlly && tiles.Contains(h.coord))
                {
                    result.AddLast(h.coord);
                }
            }
            else if (type.Contains(TargetType.KnockedOutAllies))
            {
                if (!h.IsAlive() && tiles.Contains(h.coord))
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

    private static LinkedList<GridCoord> FilterLineOfSight(GridCoord start, LinkedList<GridCoord> tiles)
    {
        int xLength = battleGridTiles.GetLength(0);
        int yLength = battleGridTiles.GetLength(1);
        float xOffSet = -(xLength - 1) / 2f;
        float yOffSet = -(yLength - 1) / 2f;

        LinkedList<GridCoord> toBeRemoved = new LinkedList<GridCoord>();

        foreach (GridCoord tile in tiles)
        {
            UnityEngine.Vector2 lineStart = new UnityEngine.Vector2(start.x, start.y);
            UnityEngine.Vector2 line = new UnityEngine.Vector2(tile.x - start.x, tile.y - start.y);

            LinkedList<GridCoord> collidingTiles = new LinkedList<GridCoord>();
            int checkDistance = (int)(line.magnitude * 2.9f);
            for (int i = 0; i < checkDistance; i++)
            {
                GridCoord tileUnder = new GridCoord((int)MathF.Round(lineStart.x + line.x / (checkDistance + 1) * i), (int)MathF.Round(lineStart.y + line.y / (checkDistance + 1) * i));
                if (!collidingTiles.Contains(tileUnder))
                {
                    collidingTiles.AddLast(tileUnder);
                }
            }

            bool blocked = false;
            foreach (GridCoord hit in collidingTiles)
            {
                if (!battleGridTiles[hit.x, hit.y].isWalkable)
                    blocked = true;
            }

            if (blocked)
                toBeRemoved.AddLast(tile);
            else
                UnityEngine.Debug.DrawRay(lineStart + new Vector2(xOffSet, yOffSet), line, Color.black, 15);
        }

        foreach (GridCoord tile in toBeRemoved)
        {
            tiles.Remove(tile);
        }
        return tiles;
    }
}


public class TargetingOptions
{
    public bool canTargetSelf;
    public BitFlag<TargetType> targetType;
    public BitFlag<PathBlocker> pathBlockers; 
    public bool needLineOfSight;

    public TargetingOptions(bool canTargetSelf, BitFlag<TargetType> targetType, BitFlag<PathBlocker> pathBlockers)
    {
        this.canTargetSelf = canTargetSelf;
        this.targetType = targetType;
        this.pathBlockers = pathBlockers;
    }

    public TargetingOptions()
    {
        canTargetSelf = true;
        targetType = TargetType.None;
        pathBlockers = PathBlocker.None;
    }
}