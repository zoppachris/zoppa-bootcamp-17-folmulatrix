using LudoGame.Enums;

namespace LudoGame.Models.Board
{
    public sealed class Board
    {
        public Tile[,] Tiles { get; }
        public Dictionary<Color, List<Tile>> ColorPaths { get; }
        public Board(Tile[,] tiles, Dictionary<Color, List<Tile>> colorPaths)
        {
            Tiles = tiles;
            ColorPaths = colorPaths;
        }
    }
}