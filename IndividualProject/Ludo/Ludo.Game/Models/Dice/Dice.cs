using Ludo.Game.Interfaces;

namespace Ludo.Game.Models.Dice
{
    public sealed class Dice : IDice
    {
        public int Sides { get; }
        public Dice(int sides)
        {
            Sides = sides;
        }
    }
}