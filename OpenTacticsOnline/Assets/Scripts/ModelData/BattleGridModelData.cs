using System.Collections.Generic;

public static partial class BattleGridModelData
{
    public const int gridSizeX = 20, gridSizeY = 10;
    private static BattleGridTile[,] battleGridTiles;
    private static LinkedList<Hero> heroes;
    
    private static LinkedList<Hero> allyHeroes;
    private static LinkedList<Hero> foeHeroes;

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

        battleGridTiles[0, 1].isWalkable = false;
        #endregion

        SetAllTilesToDefault();

        heroes = new LinkedList<Hero>();
        allyHeroes = new LinkedList<Hero>();
        foeHeroes = new LinkedList<Hero>();
        AIContoller aiContoller = new AIContoller();
        LocalPlayerController localPlayerController = new LocalPlayerController();

        Hero h = new Hero(2, 2, HeroRole.BlackMage, 6, 20, true);
        h.controller = localPlayerController;
        h.ModifyHealth(-20);
        heroes.AddLast(h);
        
        h = new Hero(3, 2, HeroRole.RedMage, 6, 20, true);
        h.controller = localPlayerController;
        heroes.AddLast(h);
        
        h = new Hero(3, 3, HeroRole.WhiteMage, 6, 20, true);
        h.controller = localPlayerController; 
        heroes.AddLast(h);

        h = new Hero(15, 7, HeroRole.Fighter, 8,20, false);
        h.controller = aiContoller;
        heroes.AddLast(h);
        
        h = new Hero(15, 6, HeroRole.Monk, 8, 20, false);
        h.controller = aiContoller;
        h.ModifyHealth(-20);
        heroes.AddLast(h);
        
        h = new Hero(15, 5, HeroRole.Thief, 8, 20, false);
        h.controller = aiContoller;
        heroes.AddLast(h);
        
        
        foreach (Hero hero in heroes)
        {
            if (hero.isAlly)
            {
                allyHeroes.AddLast(hero);
            }
            else
            {
                foeHeroes.AddLast(hero);
            }
        }
    }

    public static BattleGridTile[,] GetBattleGridTiles()
    {
        return battleGridTiles;
    }

    public static void ChangeTileID(GridCoord coord, int newID)
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
                battleGridTiles[x, y].coord.x = x;
                battleGridTiles[x, y].coord.y = y;

                if (battleGridTiles[x, y].isWalkable)
                    ChangeTileID(new GridCoord(x, y), 48);
                else
                    ChangeTileID(new GridCoord(x, y), 54);

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
    
    public static LinkedList<Hero> GetAllyHeroes()
    {
        return allyHeroes;
    }
    
    public static LinkedList<Hero> GetFoeHeroes()
    {
        return foeHeroes;
    }

    public static Hero GetHeroAtCoord(GridCoord coord)
    {
        foreach (Hero h in heroes)
        {
            if (h.coord == coord)
                return h;
        }

        return null;
    }
}

public struct BattleGridTile
{
    public GridCoord coord;
    public int id;
    public bool isWalkable;
}

