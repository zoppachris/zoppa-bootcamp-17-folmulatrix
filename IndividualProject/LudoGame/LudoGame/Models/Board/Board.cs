using System.Drawing;
using LudoGame.Models.ValueObjects;

namespace LudoGame.Models.Board
{
    public sealed class Board
    {
        public Tile[,] Tiles { get; }
        public Board(Tile[,] tiles)
        {
            Tiles = tiles;
        }
    }
}