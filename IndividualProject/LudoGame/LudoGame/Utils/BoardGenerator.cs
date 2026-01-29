using LudoGame.Enums;
using LudoGame.Models.Board;
using LudoGame.Models.ValueObjects;

namespace LudoGame.Utils
{
    public static class BoardGenerator
    {
        public static Board GenerateStandard15x15()
        {
            const int size = 15;
            var tiles = new Tile[size, size];

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    tiles[row, col] = new Tile(
                        new Position(row, col),
                        Zone.None,
                        color: null,
                        isSafe: false
                    );
                }
            }

            MarkStandardMainPath(tiles);
            MarkStandardStartTiles(tiles);
            MarkStandardFinishTiles(tiles);
            MarkStandardGoalTiles(tiles);
            MarkStandardHomeTiles(tiles);
            MarkStandardSafeTiles(tiles);

            return new Board(tiles);
        }
        public static Dictionary<Color, List<Tile>> GetStandardPath(Board board)
        {
            Dictionary<Color, List<Tile>> paths = new();

            List<Tile> redPath = new()
            {
                board.Tiles[13, 6],
                board.Tiles[12, 6],
                board.Tiles[11, 6],
                board.Tiles[10, 6],
                board.Tiles[9, 6],
                board.Tiles[8, 6],
                board.Tiles[8, 5],
                board.Tiles[8, 4],
                board.Tiles[8, 3],
                board.Tiles[8, 2],
                board.Tiles[8, 1],
                board.Tiles[8, 0],
                board.Tiles[7, 0],
                board.Tiles[6, 0],
                board.Tiles[6, 1],
                board.Tiles[6, 2],
                board.Tiles[6, 3],
                board.Tiles[6, 4],
                board.Tiles[6, 5],
                board.Tiles[6, 6],
            };

            paths.Add(Color.Red, redPath);
            return paths;
        }
        private static void MarkStandardMainPath(Tile[,] tiles)
        {
            // TOP
            for (int col = 6; col <= 8; col++)
                tiles[0, col].Zone = Zone.Main;

            // BOTTOM
            for (int col = 6; col <= 8; col++)
                tiles[14, col].Zone = Zone.Main;

            // LEFT
            for (int row = 6; row <= 8; row++)
            {
                tiles[row, 0].Zone = Zone.Main;
            }

            // RIGHT
            for (int row = 6; row <= 8; row++)
            {
                tiles[row, 14].Zone = Zone.Main;
            }

            // VERTICAL LEFT
            for (int row = 0; row <= 14; row++)
                tiles[row, 6].Zone = Zone.Main;

            // VERTICAL RIGHT
            for (int row = 0; row <= 14; row++)
                tiles[row, 8].Zone = Zone.Main;

            // MIDDLE TOP
            for (int col = 0; col <= 14; col++)
                tiles[6, col].Zone = Zone.Main;

            // MIDDLE BOTTOM
            for (int col = 0; col <= 14; col++)
                tiles[8, col].Zone = Zone.Main;
        }
        private static void MarkStandardStartTiles(Tile[,] tiles)
        {
            // Bottom : Red
            tiles[13, 6].Zone = Zone.Start;
            tiles[13, 6].Color = Color.Red;
            tiles[13, 6].IsSafe = true;

            // Left : Blue
            tiles[6, 1].Zone = Zone.Start;
            tiles[6, 1].Color = Color.Blue;
            tiles[6, 1].IsSafe = true;

            // Right : Yellow
            tiles[1, 8].Zone = Zone.Start;
            tiles[1, 8].Color = Color.Yellow;
            tiles[1, 8].IsSafe = true;

            // Bottom : Green
            tiles[8, 13].Zone = Zone.Start;
            tiles[8, 13].Color = Color.Green;
            tiles[8, 13].IsSafe = true;

        }
        private static void MarkStandardFinishTiles(Tile[,] tiles)
        {
            // Bottom : Red
            for (int row = 9; row <= 13; row++)
            {
                tiles[row, 7].Zone = Zone.Finish;
                tiles[row, 7].Color = Color.Red;
                tiles[row, 7].IsSafe = true;
            }

            // Left : Blue
            for (int col = 1; col <= 5; col++)
            {
                tiles[7, col].Zone = Zone.Finish;
                tiles[7, col].Color = Color.Blue;
                tiles[7, col].IsSafe = true;
            }

            // Top : Yellow
            for (int row = 1; row <= 5; row++)
            {
                tiles[row, 7].Zone = Zone.Finish;
                tiles[row, 7].Color = Color.Yellow;
                tiles[row, 7].IsSafe = true;
            }

            // Right : Green
            for (int col = 9; col <= 13; col++)
            {
                tiles[7, col].Zone = Zone.Finish;
                tiles[7, col].Color = Color.Green;
                tiles[7, col].IsSafe = true;
            }
        }
        private static void MarkStandardGoalTiles(Tile[,] tiles)
        {
            // Bottom : Red
            tiles[8, 7].Zone = Zone.Goal;
            tiles[8, 7].Color = Color.Red;
            tiles[8, 7].IsSafe = true;

            // Left : Blue
            tiles[7, 6].Zone = Zone.Goal;
            tiles[7, 6].Color = Color.Blue;
            tiles[7, 6].IsSafe = true;

            // Top : Yellow
            tiles[6, 7].Zone = Zone.Goal;
            tiles[6, 7].Color = Color.Yellow;
            tiles[6, 7].IsSafe = true;

            // Right : Green
            tiles[7, 8].Zone = Zone.Goal;
            tiles[7, 8].Color = Color.Green;
            tiles[7, 8].IsSafe = true;
        }
        private static void MarkStandardHomeTiles(Tile[,] tiles)
        {
            // Red Home (bottom-left)
            for (int row = 10; row <= 13; row++)
            {
                for (int col = 1; col <= 4; col++)
                {
                    tiles[row, col].Zone = Zone.Home;
                    tiles[row, col].Color = Color.Red;
                    tiles[row, col].IsSafe = true;
                }
            }

            // Blue Home (top-left)
            for (int row = 1; row <= 4; row++)
            {
                for (int col = 1; col <= 4; col++)
                {
                    tiles[row, col].Zone = Zone.Home;
                    tiles[row, col].Color = Color.Blue;
                    tiles[row, col].IsSafe = true;
                }
            }

            // Yellow Home (top-right)
            for (int row = 1; row <= 4; row++)
            {
                for (int col = 10; col <= 13; col++)
                {
                    tiles[row, col].Zone = Zone.Home;
                    tiles[row, col].Color = Color.Yellow;
                    tiles[row, col].IsSafe = true;
                }
            }

            // Green Home (bottom-right)
            for (int row = 10; row <= 13; row++)
            {
                for (int col = 10; col <= 13; col++)
                {
                    tiles[row, col].Zone = Zone.Home;
                    tiles[row, col].Color = Color.Green;
                    tiles[row, col].IsSafe = true;
                }
            }
        }
        private static void MarkStandardSafeTiles(Tile[,] tiles)
        {
            // Bottom Left
            tiles[8, 2].IsSafe = true;
            // Top Left
            tiles[2, 6].IsSafe = true;
            // Top Right
            tiles[6, 12].IsSafe = true;
            // Bottom Right
            tiles[12, 8].IsSafe = true;
        }
    }
}