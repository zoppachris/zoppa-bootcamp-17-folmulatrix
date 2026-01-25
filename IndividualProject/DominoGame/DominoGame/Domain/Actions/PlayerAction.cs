using DominoGame.Domain.Players.Interface;
using DominoGame.Game;

namespace DominoGame.Domain.Actions
{
    public abstract class PlayerAction
    {
        protected PlayerAction() { }

        public abstract void Execute(GameController controller, IPlayerMutable player);
    }
}