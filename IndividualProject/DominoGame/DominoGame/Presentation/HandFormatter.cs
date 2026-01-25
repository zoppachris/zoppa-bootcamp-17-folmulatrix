using System.Text;
using DominoGame.Domain.Entities;

namespace DominoGame.Presentation
{
    public static class HandFormatter
    {
        public static string Format(IReadOnlyList<Domino> hand)
        {
            if (hand.Count == 0)
                return "[ Empty Hand ]";

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hand.Count; i++)
            {
                sb.Append($"({i}) ");
                sb.Append(hand[i]);
                sb.Append("  ");
            }

            return sb.ToString();
        }
    }
}