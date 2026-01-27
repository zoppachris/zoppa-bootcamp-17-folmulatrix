using DominoGame.Domain.Enums;

namespace DominoGame.Domain.Entities
{
    public sealed class Board
    {
        public LinkedList<Domino> Dominoes = new();
        public bool IsEmpty => Dominoes.Count == 0;
        public Dot LeftEnd
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Board is empty.");

                return Dominoes.First!.Value.LeftPip;
            }
        }
        public Dot RightEnd
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Board is empty.");

                return Dominoes.Last!.Value.RightPip;
            }
        }
        public bool CanPlace(Domino domino)
        {
            if (IsEmpty)
                return true;

            return domino.IsMatched(LeftEnd) || domino.IsMatched(RightEnd);
        }
        public bool CanPlace(Domino domino, BoardSide side)
        {
            if (IsEmpty)
                return true;
            return side switch
            {
                BoardSide.Left => domino.IsMatched(LeftEnd),
                BoardSide.Right => domino.IsMatched(RightEnd),
                _ => false
            };
        }
    }
}