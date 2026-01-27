using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;

namespace DominoGame.Domain.Board
{
    public interface IReadOnlyBoard
    {
        Dot LeftEnd { get; }
        Dot RightEnd { get; }
        bool IsEmpty { get; }

        bool CanPlace(Domino domino);
        bool CanPlace(Domino domino, BoardSide side);
        IReadOnlyList<Domino> GetDominoes();
    }
}