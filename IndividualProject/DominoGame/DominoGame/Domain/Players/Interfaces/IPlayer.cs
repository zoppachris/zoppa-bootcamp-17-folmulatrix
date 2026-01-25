using DominoGame.Domain.Actions;
using DominoGame.Domain.Entities;
using DominoGame.Domain.Turns;

namespace DominoGame.Domain.Players.Interface
{
    public interface IPlayer
    {
        int Id { get; }
        string Name { get; }
        int Score { get; }
        IReadOnlyList<Domino> Hand { get; }

        PlayerAction PlayTurn(TurnContext context);
    }
}