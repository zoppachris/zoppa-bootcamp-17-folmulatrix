using System.Text;
using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;

namespace DominoGame.UI
{
    public static class RenderUtil
    {
        public static string[] RenderDomino(Domino domino)
        {
            return domino.Orientation switch
            {
                DominoOrientation.Horizontal => RenderHorizontal(domino),
                DominoOrientation.Vertical => RenderVertical(domino),
                _ => Array.Empty<string>()
            };
        }

        public static string[] RenderHorizontal(Domino d)
        {
            return new[]
            {
                $"[{d.LeftPip}|{d.RightPip}]"
            };
        }

        public static string[] RenderVertical(Domino d)
        {
            return new[]
            {
                $"[{d.LeftPip}]",
                $"[|]",
                $"[{d.RightPip}]"
            };
        }

        public static void RenderBoard(IReadOnlyList<Domino> dominos)
        {
            if (!dominos.Any())
            {
                Console.WriteLine("(Board Empty)");
                return;
            }

            var rendered = dominos.Select(RenderDomino).ToList();

            int maxLines = rendered.Max(r => r.Length);

            for (int i = 0; i < rendered.Count; i++)
            {
                var lines = rendered[i];

                if (lines.Length < maxLines)
                {
                    var padded = new string[maxLines];

                    for (int j = 0; j < maxLines; j++)
                    {
                        padded[j] = j < lines.Length ? lines[j] : "   ";
                    }

                    rendered[i] = padded;
                }
            }

            for (int line = 0; line < maxLines; line++)
            {
                foreach (var dominoLines in rendered)
                {
                    Console.Write(dominoLines[line].PadRight(6));
                }
                Console.WriteLine();
            }
        }
        public static void RenderHand(IReadOnlyList<Domino> dominos)
        {
            if (dominos.Count == 0)
            {
                Console.WriteLine("(Hand Empty)");
                return;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dominos.Count; i++)
            {
                sb.Append($"({i}) ");
                sb.Append(dominos[i]);
                sb.Append("  ");
            }

            Console.WriteLine(sb.ToString());
        }
    }
}