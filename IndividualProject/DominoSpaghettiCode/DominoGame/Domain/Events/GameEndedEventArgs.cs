using DominoGame.Domain.Players.Interface;

namespace DominoGame.Domain.Events
{
    public sealed class GameEndedEventArgs : EventArgs
    {
        public IPlayer Winner { get; }

        public GameEndedEventArgs(IPlayer winner)
        {
            Winner = winner ?? throw new ArgumentNullException(nameof(winner));
        }
    }
}