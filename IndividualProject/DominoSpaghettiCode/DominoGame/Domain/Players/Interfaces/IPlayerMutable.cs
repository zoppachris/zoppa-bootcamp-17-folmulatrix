using DominoGame.Domain.Entities;

namespace DominoGame.Domain.Players.Interface
{
    public interface IPlayerMutable : IPlayer
    {
        void AddDomino(Domino domino);
        void RemoveDomino(Domino domino);
        void AddScore(int points);
    }
}