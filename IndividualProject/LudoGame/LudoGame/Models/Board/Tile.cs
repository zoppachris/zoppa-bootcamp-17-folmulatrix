using LudoGame.Enums;
using LudoGame.Models.ValueObjects;

namespace LudoGame.Models.Board
{
    public sealed class Tile
    {
        public Position Position { get; }
        public Zone Zone { get; set; }
        public Color? Color { get; set; }
        public bool IsSafe { get; set; }
        public Tile(Position position, Zone zone = Zone.None, Color? color = null, bool isSafe = false)
        {
            Position = position;
            Zone = zone;
            Color = color;
            IsSafe = isSafe;
        }
    }
}