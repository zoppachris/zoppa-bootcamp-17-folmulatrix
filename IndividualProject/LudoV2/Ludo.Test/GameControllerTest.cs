using Ludo.Game.Controller;
using Ludo.Game.Enums;
using Ludo.Game.Interfaces;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.Dice;
using Ludo.Game.Models.Piece;
using Ludo.Game.Models.Player;
using Ludo.Game.Utils.BoardGenerate;

namespace Ludo.Test;

[TestFixture]
public class GameControllerTest
{
    private GameController _game;

    [SetUp]
    public void Setup()
    {
        _game = new GameController(
            StandardBoard.GenerateBoard(),
            new Dice(6),
            new List<IPlayer> {
                new Player("Player1", Color.Red),
                new Player("Player2", Color.Yellow),
            }
        );
    }

    [Test]
    public void ReturnPlayer_InCurrentTurn()
    {
        IPlayer result = _game.GetCurrentPlayer();
        Assert.Pass($"Get player {result.Name}");
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    public void GetPieces_Player_IsAvailable(int playerIndex)
    {
        try
        {
            IPlayer player = _game.Players[playerIndex];
            List<Piece> pieces = _game.GetPieces(_game.Players[playerIndex]);
            Assert.That(pieces.Count, Is.EqualTo(4));
        }
        catch (Exception)
        {
            Assert.Fail("Player not found");
        }
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void GetTile_Piece_IsAvailable(bool availablePiece)
    {
        try
        {
            IPlayer player = _game.Players[0];
            List<Piece> pieces = _game.GetPieces(player);
            Piece piece = pieces[0];

            Tile? tile;

            if (availablePiece)
            {
                tile = _game.GetPieceTile(piece);
                Assert.Pass($"Piece found in tile at position {tile?.Position}");
            }
            else
            {
                tile = _game.GetPieceTile(new Piece(Color.Green));
                Assert.That(tile, Is.Null);
            }
        }
        catch (Exception)
        {
            Assert.Fail("Piece not found");
        }
    }
}
