using DominoGame.Domain.Enums;

namespace DominoGame.Domain.Entities
{
    public sealed class Domino
    {
        public Dot LeftPip { get; init; }
        public Dot RightPip { get; init; }
        public DominoOrientation Orientation => LeftPip == RightPip ? DominoOrientation.Vertical : DominoOrientation.Horizontal;

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
        public override string ToString()
        {
            return $"[{(int)LeftPip}|{(int)RightPip}]";
        }
    }
}