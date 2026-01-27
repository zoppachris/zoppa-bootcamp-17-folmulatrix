using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;

namespace DominoGame.Domain.Board
{
    public sealed class Board : IReadOnlyBoard
    {
        private LinkedList<Domino> _dominoes = new();
        public Dot LeftEnd
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Board is empty.");

                return _dominoes.First!.Value.LeftPip;
            }
        }
        public Dot RightEnd
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException("Board is empty.");

                return _dominoes.Last!.Value.RightPip;
            }
        }
        public bool IsEmpty => _dominoes.Count == 0;
        private static Domino FlipDomino(Domino domino)
        {
            return new Domino(domino.RightPip, domino.LeftPip);
        }
        public void Clear()
        {
            _dominoes.Clear();
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
        public void Place(Domino domino, BoardSide side)
        {
            if (!CanPlace(domino, side))
                throw new InvalidOperationException($"Domino cannot be placed on the {side} side.");

            if (IsEmpty)
            {
                _dominoes.AddFirst(domino);
                return;
            }

            if (side == BoardSide.Left)
            {
                Domino orientedDomino = domino.RightPip == LeftEnd ? domino : FlipDomino(domino);
                _dominoes.AddFirst(orientedDomino);
            }
            else
            {
                Domino orientedDomino = domino.LeftPip == RightEnd ? domino : FlipDomino(domino);
                _dominoes.AddLast(orientedDomino);
            }
        }
        public IReadOnlyList<Domino> GetDominoes()
        {
            return _dominoes.ToList().AsReadOnly();
        }
    }
}