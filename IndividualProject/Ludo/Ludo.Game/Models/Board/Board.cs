using Ludo.Game.Enums;
using Ludo.Game.Interfaces;

namespace Ludo.Game.Models.Board
{
    public sealed class Board : IBoard
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