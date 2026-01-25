using System.Text;
using DominoGame.Domain.Board;
using DominoGame.Domain.Entities;

namespace DominoGame.Presentation
{
    public static class BoardFormatter
    {
        public static string Format(IReadOnlyBoard board)
        {
            if (board.IsEmpty)
                return "[ Board is empty ]";

            StringBuilder sb = new StringBuilder();

            sb.Append("==============================");
            sb.AppendLine();

            foreach (Domino domino in board.GetDominoes())
            {
                sb.Append(domino);
                sb.Append(" ");
            }

            sb.AppendLine();
            sb.AppendLine();
            sb.Append($"LeftEnd: {board.LeftEnd} | RightEnd: {board.RightEnd}");

            return sb.ToString();
        }
    }
}