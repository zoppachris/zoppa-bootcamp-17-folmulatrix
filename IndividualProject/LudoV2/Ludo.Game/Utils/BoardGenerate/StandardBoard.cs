using Ludo.Game.Enums;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.ValueObjects;

namespace Ludo.Game.Utils.BoardGenerate
{
    public static class StandardBoard
    {
        private const int Size = 15;

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
            MarkLine(tiles, LineHorizontal(0, 6, 8), Zone.Main);
            MarkLine(tiles, LineHorizontal(14, 6, 8), Zone.Main);

            MarkLine(tiles, LineVertical(0, 6, 8), Zone.Main);
            MarkLine(tiles, LineVertical(14, 6, 8), Zone.Main);

            MarkLine(tiles, LineVertical(6, 0, 14), Zone.Main);
            MarkLine(tiles, LineVertical(8, 0, 14), Zone.Main);

            MarkLine(tiles, LineHorizontal(6, 0, 14), Zone.Main);
            MarkLine(tiles, LineHorizontal(8, 0, 14), Zone.Main);
        }
        private static void MarkStandardStartTiles(Tile[,] tiles)
        {
            MarkLine(tiles, new[] { new Position(13, 6) }, Zone.Start, Color.Red, true);
            MarkLine(tiles, new[] { new Position(6, 1) }, Zone.Start, Color.Blue, true);
            MarkLine(tiles, new[] { new Position(1, 8) }, Zone.Start, Color.Yellow, true);
            MarkLine(tiles, new[] { new Position(8, 13) }, Zone.Start, Color.Green, true);
        }
        private static void MarkStandardGoalTiles(Tile[,] tiles)
        {
            MarkLine(tiles, new[] { new Position(8, 7) }, Zone.Goal, Color.Red, true);
            MarkLine(tiles, new[] { new Position(7, 6) }, Zone.Goal, Color.Blue, true);
            MarkLine(tiles, new[] { new Position(6, 7) }, Zone.Goal, Color.Yellow, true);
            MarkLine(tiles, new[] { new Position(7, 8) }, Zone.Goal, Color.Green, true);
        }
        private static void MarkStandardSafeTiles(Tile[,] tiles)
        {
            foreach (var p in new[]
                {
                    new Position(8, 2),
                    new Position(2, 6),
                    new Position(6, 12),
                    new Position(12, 8)
                }
            )
            {
                tiles[p.X, p.Y].IsSafe = true;
            }
        }
        private static void MarkStandardHomeTiles(Tile[,] tiles)
        {
            MarkHome(tiles, 10, 13, 1, 4, Color.Red);
            MarkHome(tiles, 1, 4, 1, 4, Color.Blue);
            MarkHome(tiles, 1, 4, 10, 13, Color.Yellow);
            MarkHome(tiles, 10, 13, 10, 13, Color.Green);
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
                positions: Enumerable.Range(9, 5).Select(r => new Position(r, 7)));
            MarkFinish(tiles, Color.Blue,
                positions: Enumerable.Range(1, 5).Select(c => new Position(7, c)));
            MarkFinish(tiles, Color.Yellow,
                positions: Enumerable.Range(1, 5).Select(r => new Position(r, 7)));
            MarkFinish(tiles, Color.Green,
                positions: Enumerable.Range(9, 5).Select(c => new Position(7, c)));
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
            const int center = 7;

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
                new(13, 6),
                // UP
                new(12,6), new(11,6), new(10,6), new(9,6), new(8,6),
                // LEFT
                new(8,5), new(8,4), new(8,3), new(8,2), new(8,1), new(8,0),
                // UP
                new(7,0), new(6,0),
                // RIGHT
                new(6,1), new(6,2), new(6,3), new(6,4), new(6,5), new(6,6),
                // UP
                new(5,6), new(4,6), new(3,6), new(2,6), new(1,6), new(0,6),
                // TOP CROSS
                new(0,7), new(0,8),
                // DOWN
                new(1,8), new(2,8), new(3,8), new(4,8), new(5,8), new(6,8),
                // RIGHT
                new(6,9), new(6,10), new(6,11), new(6,12), new(6,13), new(6,14),
                // DOWN
                new(7,14), new(8,14),
                // LEFT
                new(8,13), new(8,12), new(8,11), new(8,10), new(8,9), new(8,8),
                // DOWN
                new(9,8), new(10,8), new(11,8), new(12,8), new(13,8), new(14,8),
                // FINISH
                new(14,7), new(13,7), new(12,7), new(11,7), new(10,7), new(9,7), 
                // GOAL
                new(8,7)
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