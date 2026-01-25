using DominoGame.Domain.Players.Interface;
using DominoGame.Game;

namespace DominoGame.Domain.Actions
{
    public sealed class PassAction : PlayerAction
    {
        public override void Execute(GameController controller, IPlayerMutable player)
        {
            controller.PassTurn(player);
        }
    }
}