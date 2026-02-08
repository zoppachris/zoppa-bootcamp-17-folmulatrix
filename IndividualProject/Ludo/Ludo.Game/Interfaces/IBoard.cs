using Ludo.Game.Enums;
using Ludo.Game.Models.Board;

namespace Ludo.Game.Interfaces
{
    public interface IBoard
    {
        public Tile[,] Tiles { get; }
        public Dictionary<Color, List<Tile>> ColorPaths { get; }
    }
}