using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;
using DominoGame.Domain.Players.Interface;
using DominoGame.Game;

namespace DominoGame.Domain.Actions
{
    public sealed class PlayDominoAction : PlayerAction
    {
        public Domino Domino { get; }
        public BoardSide Side { get; }

        public PlayDominoAction(Domino domino, BoardSide side)
        {
            Domino = domino ?? throw new ArgumentNullException(nameof(domino));
            Side = side;
        }
        public override void Execute(GameController controller, IPlayerMutable player)
        {
            controller.PlaceDomino(player, Domino, Side);
        }
    }
}