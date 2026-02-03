namespace Ludo.Game.Models.Dice
{
    public sealed class Dice
    {
        public int Sides { get; }
        public Dice(int sides)
        {
            Sides = sides;
        }
    }
}