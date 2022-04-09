namespace AlbLib.Mapping
{
    public interface ITiled
    {
        byte Width { get; }
        byte Height { get; }
        Tile[,] TileData { get; }
    }
}
