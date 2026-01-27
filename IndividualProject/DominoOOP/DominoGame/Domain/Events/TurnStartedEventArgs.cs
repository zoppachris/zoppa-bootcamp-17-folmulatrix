using DominoGame.Domain.Entities;

namespace DominoGame.Domain.Events
{
    public sealed class TurnStartedEventArgs : EventArgs
    {
        public Player Player { get; }
        public Board Board { get; }

        public TurnStartedEventArgs(Player player, Board board)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Board = board ?? throw new ArgumentNullException(nameof(board));
        }
    }
}