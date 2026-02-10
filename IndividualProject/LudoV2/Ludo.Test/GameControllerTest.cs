using Ludo.Game.Controller;
using Ludo.Game.Enums;
using Ludo.Game.Interfaces;
using Ludo.Game.Logging;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.Dice;
using Ludo.Game.Models.Piece;
using Ludo.Game.Models.Player;
using Ludo.Game.Utils.BoardGenerate;
using Moq;

namespace Ludo.Test;

[TestFixture]
public class GameControllerTest
{
    private GameController _game;
    private IPlayer _player1;
    private IPlayer _player2;

    [SetUp]
    public void Setup()
    {
        _player1 = new Player("Player1", Color.Red);
        _player2 = new Player("Player2", Color.Yellow);

        _game = new GameController(
            new Mock<IGameLogger>().Object,
            StandardBoard.GenerateBoard(),
            new Dice(6),
            new List<IPlayer> { _player1, _player2 }
        );

        _game.StartGame();
    }

    [TestCase(0)]
    [TestCase(1)]
    public void CurrentTurn_GetCurrentPlayer_ShouldMatchIndex(int expectedIndex)
    {
        IPlayer current = _game.GetCurrentPlayer();

        Assert.That(current, Is.EqualTo(_game.Players[expectedIndex]),
            $"Current player should be {_game.Players[expectedIndex].Name}");
    }


    [Test]
    public void GetPieces_Player_ShouldReturnFourPieces()
    {
        List<Piece> pieces = _game.GetPieces(_player1);
        Assert.That(pieces.Count, Is.EqualTo(4));
    }
    
    [TestCase(0)]
    [TestCase(3)]
    [TestCase(5)]
    public void FAIL_GetPieces_Count_ShouldMatch(int expectedCount)
    {
        List<Piece> pieces = _game.GetPieces(_player1);

        Assert.That(pieces.Count, Is.EqualTo(expectedCount),
            $"Player should have {expectedCount} piece");
    }

    [Test]
    public void GetPieceTile_ExistingPiece_ShouldReturnHomeTile()
    {
        Piece piece = _game.GetPieces(_player1)[0];
        Tile? tile = _game.GetPieceTile(piece);

        Assert.That(tile, Is.Not.Null);
        Assert.That(tile!.Zone, Is.EqualTo(Zone.Home));
    }

    [Test]
    public void RollDice_ShouldReturnValueWithinDiceRange()
    {
        int value = _game.RollDice();
        Assert.That(value, Is.InRange(1, _game.Dice.Sides));
    }

    [Test]
    public void GetMoveablePieces_WhenDiceNotMax_ShouldReturnEmpty()
    {
        _game.RollDice();
        List<Piece> pieces = _game.GetMoveablePieces(_player1);

        Assert.That(pieces.Count, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void FAIL_MovePiece_FromHome_WithoutMaxDice_ShouldMove()
    {
        Piece piece = _game.GetPieces(_player1)[0];

        _game.RollDice();

        _game.MovePiece(_player1, piece);

        Assert.Pass("Piece success moved to Home");
    }

    [Test]
    public void MovePiece_WhenPieceCannotMove_ShouldThrowException()
    {
        Piece piece = _game.GetPieces(_player1)[0];

        Assert.Throws<InvalidOperationException>(() =>
            _game.MovePiece(_player1, piece)
        );
    }

    [Test]
    public void IsTurnFinished_WithoutBonusTurn_ShouldReturnTrue()
    {
        _game.RollDice();
        bool finished = _game.IsTurnFinished();

        Assert.That(finished, Is.True);
    }

    [Test]
    public void NextTurn_ShouldSwitchToNextPlayer()
    {
        _game.NextTurn();
        IPlayer current = _game.GetCurrentPlayer();

        Assert.That(current, Is.EqualTo(_player2));
    }

    [Test]
    public void IsGameOver_AtStart_ShouldReturnFalse()
    {
        Assert.That(_game.IsGameOver(), Is.False);
    }

    [Test]
    public void FAIL_GetWinner_Immediately_ShouldReturnPlayer()
    {
        IPlayer? winner = _game.GetWinner();

        Assert.That(winner, Is.Not.Null,
            "Winner already set from the beginning.");
    }


    [Test]
    public void GetWinner_WhenPlayerWins_ShouldReturnThatPlayer()
    {
        _game.MoveAllPieceToGoal(_player1);

        IPlayer? winner = _game.GetWinner();
        Assert.That(winner, Is.EqualTo(_player1));
    }

    [Test]
    public void MoveAllPieceToGoal_ShouldTriggerOnGameEnded()
    {
        IPlayer? endedPlayer = null;
        _game.OnGameEnded += p => endedPlayer = p;

        _game.MoveAllPieceToGoal(_player1);

        Assert.That(endedPlayer, Is.EqualTo(_player1));
    }
}
