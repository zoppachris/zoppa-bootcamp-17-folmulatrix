using System.Drawing;

namespace LudoGame.Models.Player
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