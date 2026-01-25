using DominoGame.Domain.Players.Interface;
using DominoGame.Domain.Rounds;

namespace DominoGame.Domain.Scoring
{
    public sealed class NormalWinScoring : ScoringStrategyBase
    {
        protected override IReadOnlyDictionary<IPlayer, int> Calculating(RoundResult result)
        {
            if (result.IsBlocked)
                throw new InvalidOperationException("Round is blocked.");

            Dictionary<IPlayer, int> scores = new Dictionary<IPlayer, int>();

            IPlayer winner = result.Winner!;

            int totalOpponentPips = 0;

            foreach (IPlayer player in result.Players)
            {
                if (player == winner)
                    continue;

                totalOpponentPips += CountPips(player.Hand);
                scores[player] = 0;
            }

            scores[winner] = totalOpponentPips;

            return scores;
        }

    }
}