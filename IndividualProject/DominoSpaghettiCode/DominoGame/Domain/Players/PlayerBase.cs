using DominoGame.Domain.Actions;
using DominoGame.Domain.Entities;
using DominoGame.Domain.Players.Interface;
using DominoGame.Domain.Turns;

namespace DominoGame.Domain.Players
{
    public abstract class PlayerBase : IPlayerMutable
    {
        private List<Domino> _hand = new();
        public int Id { get; }
        public string Name { get; }
        public int Score { get; private set; }
        public IReadOnlyList<Domino> Hand => _hand.AsReadOnly();

        protected PlayerBase(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public abstract PlayerAction PlayTurn(TurnContext context);
        public void AddDomino(Domino domino)
        {
            _hand.Add(domino);
        }
        public void RemoveDomino(Domino domino)
        {
            _hand.Remove(domino);
        }
        public void ClearHand()
        {
            _hand.Clear();
        }
        public void AddScore(int points)
        {
            Score += points;
        }
    }
}