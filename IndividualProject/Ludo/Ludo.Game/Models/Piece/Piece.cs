using Ludo.Game.Enums;
using Ludo.Game.Models.Board;

namespace Ludo.Game.Models.Piece
{
    public sealed class Piece
    {
        public Color Color { get; }
        public Tile? CurrentTile { get; set; }
        public Piece(Color color, Tile? currentTile)
        {
            Color = color;
            CurrentTile = currentTile;
        }
    }
}