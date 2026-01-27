using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;

namespace DominoGame.Domain.Events
{
    public sealed class ActionExecutedEventArgs : EventArgs
    {
        public Player Player { get; }
        public Domino? Domino { get; }
        public BoardSide? Side { get; }

        public ActionExecutedEventArgs(Player player, Domino? domino, BoardSide side)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
            Domino = domino;
            Side = side;
        }
    }
}