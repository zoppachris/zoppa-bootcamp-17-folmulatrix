using DominoGame.Domain.Players.Interface;
using DominoGame.Domain.Rounds;
using DominoGame.Presentation;

namespace DominoGame.Domain.Scoring
{
    public sealed class BlockedGameScoring : ScoringStrategyBase
    {
        protected override IReadOnlyDictionary<IPlayer, int> Calculating(RoundResult result)
        {
            if (!result.IsBlocked)
                throw new InvalidOperationException("Round is not blocked.");

            Dictionary<IPlayer, int> scores = new Dictionary<IPlayer, int>();
            Dictionary<IPlayer, int> pipCounts = new Dictionary<IPlayer, int>();

            foreach (IPlayer player in result.Players)
            {
                Console.WriteLine($"{player.Name}'s remaining dominoes : {HandFormatter.Format(player.Hand)}");
                int pips = CountPips(player.Hand);
                pipCounts[player] = pips;
                scores[player] = 0;

                Console.WriteLine($"{player.Name} total pips value : {pips}");
            }

            int minPips = pipCounts.Values.Min();

            List<IPlayer> winners = pipCounts
                .Where(kv => kv.Value == minPips)
                .Select(kv => kv.Key)
                .ToList();

            // Draw
            if (winners.Count != 1)
            {
                Console.WriteLine($"Every player have same total pips, Draw.");
                return scores;
            }

            IPlayer winner = winners[0];

            int totalOpponentPips = pipCounts
                .Where(kv => kv.Key != winner)
                .Sum(kv => kv.Value);

            scores[winner] = totalOpponentPips - pipCounts[winner];

            Console.WriteLine($"{winner.Name} have the least total pips");
            Console.WriteLine($"Score given to {winner.Name},\ntotal sum all pips minus the winner total pips :");
            Console.WriteLine($"{totalOpponentPips} - {pipCounts[winner]} = {scores[winner]}");

            return scores;
        }
    }
}