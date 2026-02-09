using Ludo.Game.Enums;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.ValueObjects;

namespace Ludo.Game.Utils.BoardGenerate
{
    public static class BigBoard
    {
        private const int Size = 25;

        public static Board GenerateBoard()
        {
            var tiles = CreateTiles(Size);

            MarkStandardMainPath(tiles);
            MarkStandardStartTiles(tiles);
            MarkStandardFinishTiles(tiles);
            MarkStandardGoalTiles(tiles);
            MarkStandardHomeTiles(tiles);
            MarkStandardSafeTiles(tiles);

            var paths = CreateStandardPaths(tiles);

            return new Board(tiles, paths);
        }
        private static Tile[,] CreateTiles(int size)
        {
            var tiles = new Tile[size, size];

            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    tiles[r, c] = new Tile(
                        new Position(r, c),
                        Zone.None,
                        color: null,
                        isSafe: false
                    );

            return tiles;
        }
        private static void MarkStandardMainPath(Tile[,] tiles)
        {
            MarkLine(tiles, LineHorizontal(0, 9, 15), Zone.Main);
            MarkLine(tiles, LineHorizontal(24, 9, 15), Zone.Main);

            MarkLine(tiles, LineVertical(0, 9, 15), Zone.Main);
            MarkLine(tiles, LineVertical(24, 9, 15), Zone.Main);

            MarkLine(tiles, LineVertical(9, 0, 9), Zone.Main);
            MarkLine(tiles, LineVertical(9, 15, 24), Zone.Main);

            MarkLine(tiles, LineVertical(15, 0, 9), Zone.Main);
            MarkLine(tiles, LineVertical(15, 15, 24), Zone.Main);

            MarkLine(tiles, LineHorizontal(9, 0, 9), Zone.Main);
            MarkLine(tiles, LineHorizontal(9, 15, 24), Zone.Main);

            MarkLine(tiles, LineHorizontal(15, 0, 9), Zone.Main);
            MarkLine(tiles, LineHorizontal(15, 15, 24), Zone.Main);
        }
        private static void MarkStandardStartTiles(Tile[,] tiles)
        {
            MarkLine(tiles, new[] { new Position(23, 9) }, Zone.Start, Color.Red, true);
            MarkLine(tiles, new[] { new Position(9, 1) }, Zone.Start, Color.Blue, true);
            MarkLine(tiles, new[] { new Position(1, 15) }, Zone.Start, Color.Yellow, true);
            MarkLine(tiles, new[] { new Position(15, 23) }, Zone.Start, Color.Green, true);
        }
        private static void MarkStandardGoalTiles(Tile[,] tiles)
        {
            MarkLine(tiles, new[] { new Position(13, 12) }, Zone.Goal, Color.Red, true);
            MarkLine(tiles, new[] { new Position(12, 11) }, Zone.Goal, Color.Blue, true);
            MarkLine(tiles, new[] { new Position(11, 12) }, Zone.Goal, Color.Yellow, true);
            MarkLine(tiles, new[] { new Position(12, 13) }, Zone.Goal, Color.Green, true);
        }
        private static void MarkStandardSafeTiles(Tile[,] tiles)
        {
            foreach (var p in new[]
                {
                    new Position(15, 2),
                    new Position(2, 9),
                    new Position(9, 22),
                    new Position(22, 15)
                }
            )
            {
                tiles[p.X, p.Y].IsSafe = true;
            }
        }
        private static void MarkStandardHomeTiles(Tile[,] tiles)
        {
            MarkHome(tiles, 17, 23, 1, 7, Color.Red);
            MarkHome(tiles, 1, 7, 1, 7, Color.Blue);
            MarkHome(tiles, 1, 7, 17, 23, Color.Yellow);
            MarkHome(tiles, 17, 23, 17, 23, Color.Green);
        }
        private static void MarkHome(
            Tile[,] tiles,
            int rowFrom, int rowTo,
            int colFrom, int colTo,
            Color color)
        {
            for (int r = rowFrom; r <= rowTo; r++)
                for (int c = colFrom; c <= colTo; c++)
                {
                    tiles[r, c].Zone = Zone.Home;
                    tiles[r, c].Color = color;
                    tiles[r, c].IsSafe = true;
                }
        }
        private static void MarkStandardFinishTiles(Tile[,] tiles)
        {
            MarkFinish(tiles, Color.Red,
                positions: Enumerable.Range(14, 10).Select(r => new Position(r, 12)));
            MarkFinish(tiles, Color.Blue,
                positions: Enumerable.Range(1, 10).Select(c => new Position(12, c)));
            MarkFinish(tiles, Color.Yellow,
                positions: Enumerable.Range(1, 10).Select(r => new Position(r, 12)));
            MarkFinish(tiles, Color.Green,
                positions: Enumerable.Range(14, 10).Select(c => new Position(12, c)));
        }
        private static void MarkFinish(
            Tile[,] tiles,
            Color color,
            IEnumerable<Position> positions)
        {
            foreach (var p in positions)
            {
                var tile = tiles[p.X, p.Y];
                tile.Zone = Zone.Finish;
                tile.Color = color;
                tile.IsSafe = true;
            }
        }
        private static Dictionary<Color, List<Tile>> CreateStandardPaths(Tile[,] tiles)
        {
            var redPositions = CreateRedPathPositions();
            var bluePositions = RotatePath90(redPositions);
            var yellowPositions = RotatePath90(bluePositions);
            var greenPositions = RotatePath90(yellowPositions);

            return new Dictionary<Color, List<Tile>>
            {
                [Color.Red] = ToTiles(tiles, redPositions),
                [Color.Blue] = ToTiles(tiles, bluePositions),
                [Color.Yellow] = ToTiles(tiles, yellowPositions),
                [Color.Green] = ToTiles(tiles, greenPositions),
            };
        }
        private static Position Rotate90(Position p)
        {
            const int center = 12;

            int newX = center + (p.Y - center);
            int newY = center - (p.X - center);

            return new Position(newX, newY);
        }
        private static List<Position> RotatePath90(IEnumerable<Position> path)
        {
            return path.Select(Rotate90).ToList();
        }
        private static List<Position> CreateRedPathPositions()
        {
            return new List<Position>
            {
                // START
                new(23, 9),
                // UP
                new(22, 9), new(21, 9), new(20, 9), new(19, 9), new(18, 9), new(17, 9), new(16, 9), new(15, 9),
                // LEFT
                new(15, 8), new(15, 7), new(15, 6), new(15, 5), new(15, 4), new(15, 3), new(15, 2), new(15, 1), new(15, 0), 
                // UP
                new(14, 0), new(13, 0), new(12, 0), new(11, 0), new(10, 0), new(9, 0), 
                // RIGHT
                new(9, 1), new(9, 2), new(9, 3), new(9, 4), new(9, 5), new(9, 6), new(9, 7), new(9, 8), new(9, 9), 
                // UP
                new(9, 9), new(8, 9), new(7, 9), new(6, 9), new(5, 9), new(4, 9), new(3, 9), new(2, 9), new(1, 9), new(0, 9), 
                // TOP CROSS
                new(0, 10), new(0, 11), new(0, 12), new(0, 13), new(0, 14), new(0, 15), 
                // DOWN
                new(1, 15), new(2, 15), new(3, 15), new(4, 15), new(5, 15), new(6, 15), new(7, 15), new(8, 15), new(9, 15), 
                // RIGHT
                new(9, 16), new(9, 17), new(9, 18), new(9, 19), new(9, 20), new(9, 21), new(9, 22), new(9, 23), new(9, 24), 
                // DOWN
                new(10, 24), new(11, 24), new(12, 24), new(13, 24), new(14, 24), new(15, 24), 
                // LEFT
                new(15, 23), new(15, 22), new(15, 21), new(15, 20), new(15, 19), new(15, 18), new(15, 17), new(15, 16), new(15, 15), 
                // DOWN
                new(16, 15), new(17, 15), new(18, 15), new(19, 15), new(20, 15), new(21, 15), new(22, 15), new(23, 15), new(24, 15), 
                // LEFT
                new(24, 14), new(24, 13), new(24, 12), 
                // FINISH
                new(23, 12), new(22, 12), new(21, 12), new(20, 12), new(19, 12), new(18, 12), new(17, 12), new(16, 12), new(15, 12), new(14, 12), 
                // GOAL
                new(13, 12),
            };
        }

        // Helper
        private static void MarkLine(Tile[,] tiles, IEnumerable<Position> positions, Zone zone, Color? color = null, bool safe = false)
        {
            foreach (var p in positions)
            {
                var tile = tiles[p.X, p.Y];
                tile.Zone = zone;
                tile.Color = color;
                tile.IsSafe |= safe;
            }
        }
        private static IEnumerable<Position> LineHorizontal(int row, int from, int to)
        {
            for (int col = from; col <= to; col++)
                yield return new Position(row, col);
        }
        private static IEnumerable<Position> LineVertical(int col, int from, int to)
        {
            for (int row = from; row <= to; row++)
                yield return new Position(row, col);
        }
        private static List<Tile> ToTiles(Tile[,] tiles, IEnumerable<Position> positions)
        {
            return positions.Select(p => tiles[p.X, p.Y]).ToList();
        }
    }
}