using System.Drawing;
using LudoGame.Models.Board;

namespace LudoGame.Models.Piece
{
    public sealed class Piece
    {
        public Color Color { get; }
        public Tile? CurrentTile { get; }
        public Piece(Color color, Tile? currentTile)
        {
            Color = color;
            CurrentTile = currentTile;
        }
    }
}