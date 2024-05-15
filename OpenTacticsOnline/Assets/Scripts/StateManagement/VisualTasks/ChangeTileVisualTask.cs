public class ChangeTileVisualTask : VisualTask
{
    int id;
    GridCoord coord;

    public ChangeTileVisualTask(GridCoord coord, int id)
    {
        this.id = id;
        this.coord = coord;
    }

    public override void Update()
    {
        BattleGridModelData.ChangeTileID(coord, id);
        IsDone = true;
    }
}