using System.Collections;
using System.Collections.Generic;

public static class BattleGridModelData
{
    const int gridSizeX = 20, gridSizeY = 10;

    static BattleGridTile[,] battleGridTiles;


    public static void Init()
    {
        battleGridTiles = new BattleGridTile[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                battleGridTiles[x, y].x = x;
                battleGridTiles[x, y].y = y;
                battleGridTiles[x, y].id = 54;
            }
        }

    }

    public static BattleGridTile[,] GetBattleGridTiles()
    {
        return battleGridTiles;
    }

    public static void ChangeTileID(int x, int y, int newID)
    {
        battleGridTiles[x,y].id = newID;
        ContentLoader.UpdateGridTileSprite(x, y, newID);
    }

}

public struct BattleGridTile
{
    public int x, y;
    public int id;
}

