using Ludo.Game.Enums;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.ValueObjects;

namespace Ludo.Game.Utils
{
    public static class BigBoard
    {
        private const int Size = 25;
        private const int Center = 12;

        public static Board GenerateBoard()
        {
            var tiles = CreateTiles(Size);

            MarkMainPath(tiles);
            MarkStartTiles(tiles);
            MarkFinishTiles(tiles);
            MarkGoalTiles(tiles);
            MarkHomeTiles(tiles);
            MarkSafeTiles(tiles);

            var paths = CreatePaths(tiles);

            return new Board(tiles, paths);
        }

        #region Tiles
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
        #endregion

        #region Mark Zones
        private static void MarkMainPath(Tile[,] tiles)
        {
            MarkLine(tiles, LineHorizontal(0, Center - 1, Center + 1), Zone.Main);
            MarkLine(tiles, LineHorizontal(Size - 1, Center - 1, Center + 1), Zone.Main);

            MarkLine(tiles, LineVertical(0, Center - 1, Center + 1), Zone.Main);
            MarkLine(tiles, LineVertical(Size - 1, Center - 1, Center + 1), Zone.Main);

            MarkLine(tiles, LineVertical(Center - 1, 0, Size - 1), Zone.Main);
            MarkLine(tiles, LineVertical(Center + 1, 0, Size - 1), Zone.Main);

            MarkLine(tiles, LineHorizontal(Center - 1, 0, Size - 1), Zone.Main);
            MarkLine(tiles, LineHorizontal(Center + 1, 0, Size - 1), Zone.Main);
        }

        private static void MarkStartTiles(Tile[,] tiles)
        {
            MarkLine(tiles, new[] { new Position(Size - 2, Center - 1) }, Zone.Start, Color.Red, true);
            MarkLine(tiles, new[] { new Position(Center - 1, 1) }, Zone.Start, Color.Blue, true);
            MarkLine(tiles, new[] { new Position(1, Center + 1) }, Zone.Start, Color.Yellow, true);
            MarkLine(tiles, new[] { new Position(Center + 1, Size - 2) }, Zone.Start, Color.Green, true);
        }

        private static void MarkGoalTiles(Tile[,] tiles)
        {
            MarkLine(tiles, new[] { new Position(Center, Center) }, Zone.Goal, null, true);
        }

        private static void MarkSafeTiles(Tile[,] tiles)
        {
            foreach (var p in new[]
            {
                new Position(Center + 1, 3),
                new Position(3, Center - 1),
                new Position(Center - 1, Size - 4),
                new Position(Size - 4, Center + 1)
            })
            {
                tiles[p.X, p.Y].IsSafe = true;
            }
        }

        private static void MarkHomeTiles(Tile[,] tiles)
        {
            MarkHome(tiles, Size - 7, Size - 2, 2, 7, Color.Red);
            MarkHome(tiles, 2, 7, 2, 7, Color.Blue);
            MarkHome(tiles, 2, 7, Size - 7, Size - 2, Color.Yellow);
            MarkHome(tiles, Size - 7, Size - 2, Size - 7, Size - 2, Color.Green);
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

        private static void MarkFinishTiles(Tile[,] tiles)
        {
            MarkFinish(tiles, Color.Red,
                Enumerable.Range(Center + 2, 10).Select(r => new Position(r, Center)));

            MarkFinish(tiles, Color.Blue,
                Enumerable.Range(1, 10).Select(c => new Position(Center, c)));

            MarkFinish(tiles, Color.Yellow,
                Enumerable.Range(1, 10).Select(r => new Position(r, Center)));

            MarkFinish(tiles, Color.Green,
                Enumerable.Range(Center + 2, 10).Select(c => new Position(Center, c)));
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
        #endregion

        #region Paths
        private static Dictionary<Color, List<Tile>> CreatePaths(Tile[,] tiles)
        {
            var red = CreateRedPathPositions();
            var blue = RotatePath90(red);
            var yellow = RotatePath90(blue);
            var green = RotatePath90(yellow);

            return new Dictionary<Color, List<Tile>>
            {
                [Color.Red] = ToTiles(tiles, red),
                [Color.Blue] = ToTiles(tiles, blue),
                [Color.Yellow] = ToTiles(tiles, yellow),
                [Color.Green] = ToTiles(tiles, green),
            };
        }

        private static Position Rotate90(Position p)
        {
            int newX = Center + (p.Y - Center);
            int newY = Center - (p.X - Center);
            return new Position(newX, newY);
        }

        private static List<Position> RotatePath90(IEnumerable<Position> path)
            => path.Select(Rotate90).ToList();

        private static List<Position> CreateRedPathPositions()
        {
            var path = new List<Position>();

            // START
            path.Add(new Position(Size - 2, Center - 1));

            // UP
            for (int r = Size - 3; r >= Center + 1; r--)
                path.Add(new Position(r, Center - 1));

            // LEFT
            for (int c = Center - 2; c >= 0; c--)
                path.Add(new Position(Center + 1, c));

            // UP
            for (int r = Center; r >= 0; r--)
                path.Add(new Position(r, 0));

            // RIGHT
            for (int c = 1; c <= Center - 1; c++)
                path.Add(new Position(0, c));

            // DOWN
            for (int r = 1; r <= Center - 1; r++)
                path.Add(new Position(r, Center + 1));

            // RIGHT
            for (int c = Center + 2; c < Size; c++)
                path.Add(new Position(Center - 1, c));

            // DOWN
            for (int r = Center; r < Size; r++)
                path.Add(new Position(r, Size - 1));

            // LEFT
            for (int c = Size - 2; c >= Center + 1; c--)
                path.Add(new Position(Size - 1, c));

            // FINISH
            for (int r = Size - 2; r > Center; r--)
                path.Add(new Position(r, Center));

            // GOAL
            path.Add(new Position(Center, Center));

            return path;
        }
        #endregion

        #region Helpers
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
            => positions.Select(p => tiles[p.X, p.Y]).ToList();
        #endregion
    }
}
