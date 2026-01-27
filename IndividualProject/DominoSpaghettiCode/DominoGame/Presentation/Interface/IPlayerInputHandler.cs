using DominoGame.Domain.Actions;
using DominoGame.Domain.Turns;

namespace DominoGame.Presentation.Interface
{
    public interface IPlayerInputHandler
    {
        PlayerAction GetPlayerAction(TurnContext context);
    }
}