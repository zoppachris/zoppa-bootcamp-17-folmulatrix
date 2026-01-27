using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;

namespace DominoGame.Domain.Collections
{
    public class DominoSet
    {
        private List<Domino> _dominoes;
        private readonly Random _random = new();
        public int Count => _dominoes.Count;

        private static List<Domino> GenerateFullSet()
        {
            List<Domino> result = new List<Domino>();

            for (int left = 0; left <= 6; left++)
            {
                for (int right = left; right <= 6; right++)
                {
                    result.Add(new Domino((Dot)left, (Dot)right));
                }
            }

            return result;
        }
        public DominoSet()
        {
            _dominoes = GenerateFullSet();
        }
        public void Reset()
        {
            _dominoes.Clear();
            _dominoes = GenerateFullSet();
        }
        public bool IsEmpty()
        {
            return _dominoes.Count == 0;
        }
        public void Shuffle()
        {
            for (int i = _dominoes.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (_dominoes[i], _dominoes[j]) = (_dominoes[j], _dominoes[i]);
            }
        }
        public Domino Draw()
        {
            if (IsEmpty())
                throw new InvalidOperationException("DominoSet is empty.");

            var domino = _dominoes[0];
            _dominoes.RemoveAt(0);
            return domino;
        }
    }
}