using DominoGame.Domain.Actions;
using DominoGame.Domain.Turns;
using DominoGame.Presentation.Interface;

namespace DominoGame.Domain.Players.Human
{
    public class HumanPlayer : PlayerBase
    {
        private readonly IPlayerInputHandler _inputHandler;
        public HumanPlayer(int id, string name, IPlayerInputHandler inputHandler) : base(id, name)
        {
            _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));
        }
        public override PlayerAction PlayTurn(TurnContext context)
        {
            return _inputHandler.GetPlayerAction(context);
        }
    }
}