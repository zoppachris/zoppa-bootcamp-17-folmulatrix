using DominoGame.Domain.Entities;

namespace DominoGame.Domain.Events
{
    public sealed class RoundEndedEventArgs : EventArgs
    {
        public Player? Winner { get; }
        public bool IsBlocked { get; }

        public RoundEndedEventArgs(Player? winner, bool isBlocked)
        {
            Winner = winner ?? throw new ArgumentNullException(nameof(winner));
            IsBlocked = isBlocked;
        }
    }
}