using LudoGame.Enums;

namespace LudoGame.Models.Piece
{
    public sealed class Piece
    {
        public Color Color { get; }
        public Piece(Color color)
        {
            Color = color;
        }
    }
}