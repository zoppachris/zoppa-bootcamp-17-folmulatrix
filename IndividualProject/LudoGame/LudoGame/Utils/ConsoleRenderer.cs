using LudoGame.Controller;
using LudoGame.Enums;
using LudoGame.Models.Board;
using LudoGame.Models.Piece;
using LudoGame.Models.Player;

namespace LudoGame.Utils
{
    public static class ConsoleRenderer
    {
        public static void RenderBoard(Board board, GameController game, HashSet<Piece> moveablePieces)
        {

            Dictionary<Tile, List<Piece>> tileToPieces = new Dictionary<Tile, List<Piece>>();

            foreach (Player player in game.Players)
            {
                foreach (Piece piece in game.GetPieces(player))
                {
                    Tile? tile = game.GetPieceTile(piece);
                    if (tile == null)
                        continue;

                    if (!tileToPieces.TryGetValue(tile, out List<Piece>? list))
                    {
                        list = new List<Piece>();
                        tileToPieces[tile] = list;
                    }

                    list.Add(piece);
                }
            }

            int rowCount = board.Tiles.GetLength(0);    // Returns 4
            int columnCount = board.Tiles.GetLength(1); // Returns 5

            Console.Write("    ");
            for (int x = 0; x < rowCount; x++)
            {
                Console.Write($"{x,3}");
            }
            Console.WriteLine();

            for (int y = 0; y < rowCount; y++)
            {
                Console.Write($"{y,3} |");

                for (int x = 0; x < columnCount; x++)
                {
                    Tile tile = board.Tiles[y, x];

                    if (tileToPieces.TryGetValue(tile, out List<Piece>? pieces))
                    {
                        RenderPiecesInTile(pieces, moveablePieces);
                    }
                    else
                    {
                        if (tile.Zone == Zone.Finish || tile.Zone == Zone.Start)
                        {
                            Console.BackgroundColor = GetColor(tile.Color);
                        }
                        Console.Write($" {GetTileChar(tile)} ");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                }

                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static void RenderPiecesInTile(List<Piece> pieces, HashSet<Piece> moveablePieces)
        {
            bool hasMoveable = pieces.Any(p => moveablePieces.Contains(p));

            if (hasMoveable)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            }

            if (pieces.Count == 1)
            {
                Piece? p = pieces[0];
                Console.ForegroundColor = GetColor(p.Color);
                Console.Write($" {GetPieceChar(p.Color)} ");
            }
            else
            {
                Color firstColor = pieces[0].Color;
                bool sameColor = pieces.All(p => p.Color == firstColor);

                if (sameColor)
                {
                    Console.ForegroundColor = GetColor(firstColor);
                    Console.Write($" {GetPieceChar(firstColor)}{pieces.Count}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(" * ");
                }
            }

            Console.ResetColor();
        }
        public static char GetTileChar(Tile tile)
        {
            return tile.Zone switch
            {
                Zone.Home => 'H',
                Zone.Start => 'S',
                Zone.Main => tile.IsSafe ? '*' : '#',
                Zone.Finish => 'F',
                Zone.Goal => 'G',
                _ => '.'
            };
        }
        public static char GetPieceChar(Color color)
        {
            return color switch
            {
                Color.Red => 'R',
                Color.Blue => 'B',
                Color.Yellow => 'Y',
                Color.Green => 'G',
                _ => '?'
            };
        }
        public static ConsoleColor GetColor(Color? color)
        {
            return color switch
            {
                Color.Red => ConsoleColor.Red,
                Color.Blue => ConsoleColor.Cyan,
                Color.Yellow => ConsoleColor.Yellow,
                Color.Green => ConsoleColor.Green,
                _ => ConsoleColor.White
            };
        }
    }
}