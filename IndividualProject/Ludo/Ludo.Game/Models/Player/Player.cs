using Ludo.Game.Enums;
using Ludo.Game.Interfaces;

namespace Ludo.Game.Models.Player
{
    public class Player : IPlayer
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