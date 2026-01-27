using DominoGame.Domain.Entities;
using DominoGame.Domain.Players.Interface;
using DominoGame.Domain.Rounds;

namespace DominoGame.Domain.Scoring
{
    public abstract class ScoringStrategyBase : IScoringStrategy
    {
        public IReadOnlyDictionary<IPlayer, int> CalculateScores(RoundResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));

            return Calculating(result);
        }

        protected abstract IReadOnlyDictionary<IPlayer, int> Calculating(RoundResult result);

        protected int CountPips(IEnumerable<Domino> dominoes)
        {
            int total = 0;

            foreach (Domino domino in dominoes)
            {
                total += (int)domino.LeftPip;
                total += (int)domino.RightPip;
            }

            return total;
        }
    }
}