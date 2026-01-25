using DominoGame.Domain.Players.Interface;

namespace DominoGame.Domain.Events
{
    public sealed class TurnStartedEventArgs : EventArgs
    {
        public IPlayer Player { get; }

        public TurnStartedEventArgs(IPlayer player)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
        }
    }
}