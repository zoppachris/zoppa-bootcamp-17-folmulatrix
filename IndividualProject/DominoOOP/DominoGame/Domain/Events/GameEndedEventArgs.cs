

using DominoGame.Domain.Entities;

namespace DominoGame.Domain.Events
{
    public sealed class GameEndedEventArgs : EventArgs
    {
        public Player Winner { get; }

        public GameEndedEventArgs(Player winner)
        {
            Winner = winner ?? throw new ArgumentNullException(nameof(winner));
        }
    }
}