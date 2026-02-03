namespace Ludo.Game.Models.ValueObjects
{

    public readonly struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override bool Equals(object? obj)
        {
            return obj is Position other &&
                   X == other.X &&
                   Y == other.Y;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        public static bool operator ==(Position left, Position right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Position left, Position right)
        {
            return !left.Equals(right);
        }
        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}
