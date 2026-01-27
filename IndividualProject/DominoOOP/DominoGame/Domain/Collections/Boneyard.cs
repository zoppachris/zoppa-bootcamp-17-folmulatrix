using DominoGame.Domain.Entities;

namespace DominoGame.Domain.Collections
{
    public class Boneyard
    {
        public List<Domino> Dominoes;

        public Boneyard(List<Domino> dominoes)
        {
            Dominoes = dominoes;
        }
        public bool IsEmpty()
        {
            return Dominoes.Count == 0;
        }
    }
}