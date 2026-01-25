using DominoGame.Domain.Enums;

namespace DominoGame.Domain.Entities
{
    public sealed class Domino
    {
        public Dot LeftPip { get; init; }
        public Dot RightPip { get; init; }

        public Domino(Dot leftPip, Dot rightPip)
        {
            LeftPip = leftPip;
            RightPip = rightPip;
        }
        public bool IsDouble()
        {
            return LeftPip == RightPip;
        }
        public bool IsMatched(Dot pip)
        {
            return LeftPip == pip || RightPip == pip;
        }
        public bool CanConnect(Domino other)
        {
            return IsMatched(other.LeftPip) || IsMatched(other.RightPip);
        }
        public Dot GetOpposite(Dot pip)
        {
            if (LeftPip == pip) return RightPip;
            if (RightPip == pip) return LeftPip;

            throw new InvalidOperationException(
                $"Pip {pip} does not exist in this domino."
            );
        }
        public void Display()
        {
            Console.Write(this.ToString());
        }
        public override string ToString()
        {
            return $"[{(int)LeftPip}|{(int)RightPip}]";
        }
    }
}