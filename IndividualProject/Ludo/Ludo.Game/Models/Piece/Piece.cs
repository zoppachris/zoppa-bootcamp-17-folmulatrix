using Ludo.Game.Enums;

namespace Ludo.Game.Models.Piece
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