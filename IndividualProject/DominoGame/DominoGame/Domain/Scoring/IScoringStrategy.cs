using DominoGame.Domain.Players.Interface;
using DominoGame.Domain.Rounds;

namespace DominoGame.Domain.Scoring
{
    public interface IScoringStrategy
    {
        IReadOnlyDictionary<IPlayer, int> CalculateScores(RoundResult result);
    }
}