using Ludo.Game.Enums;

namespace Ludo.Game.Models.Player
{
    public class Player
    {
        public string Name { get; }
        public Color Color { get; }
        public Player(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }
}