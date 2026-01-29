using LudoGame.Enums;
using LudoGame.Models.Board;
using LudoGame.Utils;

namespace LudoGame
{
    class Program
    {
        static void Main()
        {
            Board standardBoard = BoardGenerator.GenerateStandard15x15();

            PrintBoard(standardBoard);
        }

        static void PrintBoard(Board board)
        {
            Tile[,] tiles = board.Tiles;
            int size = tiles.GetLength(0);

            Console.WriteLine("=== LUDO BOARD 15x15 ===\n");

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Console.BackgroundColor = ChangeConsoleColor(tiles[row, col]);
                    Console.Write(GetTileChar(tiles[row, col]) + " ");
                }
                Console.WriteLine();
            }
        }

        static ConsoleColor ChangeConsoleColor(Tile tile)
        {
            return tile.Color switch
            {
                Color.Red => ConsoleColor.Red,
                Color.Blue => ConsoleColor.Blue,
                Color.Green => ConsoleColor.Green,
                Color.Yellow => ConsoleColor.Yellow,
                _ => ConsoleColor.Black
            };
        }
        static char GetTileChar(Tile tile)
        {
            return tile.Zone switch
            {
                Zone.None => '.',
                Zone.Main => tile.IsSafe ? '*' : '#',
                Zone.Start => 'S',
                Zone.Finish => 'F',
                Zone.Goal => 'G',
                Zone.Home => 'H',
                _ => '?'
            };
        }
    }
}