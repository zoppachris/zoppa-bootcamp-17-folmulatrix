using DominoGame.Domain.Actions;
using DominoGame.Domain.Players.Interface;

namespace DominoGame.Domain.Events
{
    public sealed class ActionExecutedEventArgs : EventArgs
    {
        public IPlayer Player { get; }
        public PlayerAction Action { get; }

        public ActionExecutedEventArgs(IPlayer player, PlayerAction action)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }
    }
}