
using DominoGame.Domain.Players.Interface;

namespace DominoGame.Domain.Rounds
{
    public class RoundResult
    {
        public bool IsBlocked { get; }
        public IPlayer? Winner { get; }
        public IReadOnlyList<IPlayer> Players { get; }

        public RoundResult(bool isBlocked, IPlayer? winner, IReadOnlyList<IPlayer> players)
        {
            if (players == null || players.Count == 0)
                throw new ArgumentException("Players cannot be empty.", nameof(players));

            if (!isBlocked && winner == null)
                throw new ArgumentException("Winner must be provided for non-blocked round.");

            IsBlocked = isBlocked;
            Winner = winner;
            Players = players;
        }
    }
}