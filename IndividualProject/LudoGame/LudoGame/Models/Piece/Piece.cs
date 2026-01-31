using LudoGame.Enums;
using LudoGame.Models.Board;

namespace LudoGame.Models.Piece
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