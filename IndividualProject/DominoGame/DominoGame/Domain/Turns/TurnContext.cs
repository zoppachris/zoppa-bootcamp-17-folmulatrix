using DominoGame.Domain.Board;
using DominoGame.Domain.Entities;

namespace DominoGame.Domain.Turns
{
    public sealed class TurnContext
    {
        public IReadOnlyBoard Board { get; }
        public IReadOnlyList<Domino> Hand { get; }
        public bool CanPlay { get; }

        public TurnContext(IReadOnlyBoard board, IReadOnlyList<Domino> hand)
        {
            Board = board;
            Hand = hand;

            CanPlay = CalculateCanPlay();
        }
        private bool CalculateCanPlay()
        {
            if (Hand.Count == 0)
                return false;

            if (Board.IsEmpty)
                return true;

            foreach (Domino domino in Hand)
            {
                if (Board.CanPlace(domino))
                    return true;
            }

            return false;
        }
    }
}