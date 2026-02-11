using Ludo.Game.Controller;
using Ludo.Game.Enums;
using Ludo.Game.Interfaces;
using Ludo.Game.Logging;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.Dice;
using Ludo.Game.Models.Piece;
using Ludo.Game.Models.Player;
using Ludo.Game.Models.ValueObjects;
using Ludo.Game.Utils.BoardGenerate;
using Moq;

namespace Ludo.Test;

[TestFixture]
public class GameControllerTest
{
    private GameController _game;
    private IPlayer _player1;
    private IPlayer _player2;
    private Piece _piece1;

    [SetUp]
    public void Setup()
    {
        _player1 = new Player("Player1", Color.Red);
        _player2 = new Player("Player2", Color.Yellow);
        _piece1 = new Piece(Color.Red);

        _game = new GameController(
            new Mock<IGameLogger>().Object,
            StandardBoard.GenerateBoard(),
            new Dice(6),
            new List<IPlayer> { _player1, _player2 }
        );

        _game.StartGame();
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    public void Board_ShouldHave_ColorPaths_ForEachPlayer(int playerIndex)
    {
        Color playerColor = _game.Players[playerIndex].Color;

        Assert.That(_game.Board.ColorPaths[playerColor], Is.Not.Null);
    }

    [Test]
    public void StartGame_TurnIsFinish()
    {
        _game.StartGame();
        bool isTurnFinished = _game.IsTurnFinished();

        Assert.That(isTurnFinished, Is.True, "Player turn is not finish yet");
    }

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    public void CurrentTurn_GetCurrentPlayer_ShouldMatchIndex(int expectedIndex)
    {
        if (expectedIndex == 1)
        {
            _game.NextTurn();
        }

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

    [Test]
    [TestCase(0)]
    [TestCase(3)]
    [TestCase(5)]
    public void GetPieces_Count_ShouldNotMatch(int expectedCount)
    {
        List<Piece> pieces = _game.GetPieces(_player1);

        Assert.That(pieces.Count != expectedCount, Is.True,
            $"Player should not have {expectedCount} piece");
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
    public void GetPieceTile_NonExistingPiece()
    {
        Tile? tile = _game.GetPieceTile(_piece1);

        Assert.That(tile, Is.Null,
            "Tile should be null for non-existing piece.");
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
    public void GetMoveablePieces_WhenDiceMax()
    {
        int diceValue = 0;

        while (diceValue != _game.Dice.Sides)
        {
            diceValue = _game.RollDice();
        }
        List<Piece> pieces = _game.GetMoveablePieces(_player1);

        Assert.That(pieces.Count, Is.GreaterThan(0));
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
    public void MovePiece_FromHome_ToStart_WhenDiceMax()
    {
        int diceValue = 0;

        while (diceValue != _game.Dice.Sides)
        {
            diceValue = _game.RollDice();
        }
        List<Piece> pieces = _game.GetMoveablePieces(_player1);

        _game.MovePiece(_player1, pieces[0]);

        Tile? pieceTile = _game.PiecePositions[pieces[0]];

        Assert.That(pieceTile?.Zone == Zone.Start, Is.True);
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
    public void IsPlayerWinner_ShouldReturnFalseAtStart()
    {
        bool isWinner = _game.IsPlayerWin(_player1);

        Assert.That(isWinner, Is.False);
    }

    [Test]
    public void GetWinner_Immediately_ShouldNotReturnPlayer()
    {
        IPlayer? winner = _game.GetWinner();

        Assert.That(winner, Is.Null,
            "Winner not set from the beginning.");
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

    [Test]
    public void PieceInGoal_ShouldReturnTrue()
    {
        _game.MoveAllPieceToGoal(_player1);
        Piece playerPiece = _game.GetPieces(_player1)[0];
        Tile? pieceTile = _game.PiecePositions[playerPiece];

        Assert.That(pieceTile?.Zone == Zone.Goal, Is.True, "Piece position is not in Goal Tile");
    }

    [Test]
    public void ResetGame_Winner_ShouldReturnNull()
    {
        _game.ResetGame();

        IPlayer? winner = _game.GetWinner();
        Assert.That(winner, Is.Null);
    }

    [Test]
    public void IsPositionEqual_WithDifferentInstance_ButSameValue()
    {
        Position position1 = new(1, 1);
        Position position2 = new(1, 1);

        Assert.That(position1 == position2, Is.True, $"{position1} is equal to {position2}");
    }

    [Test]
    public void IsPositionNotEqual_WithDifferentInstance_DifferentValue()
    {
        Position position1 = new(1, 1);
        Position position2 = new(1, 2);

        Assert.That(position1 != position2, Is.True, $"{position1} is not equal to {position2}");
    }

    [Test]
    public void IsPositionHashcodeEqual_WithDifferentInstance_ButSameValue()
    {
        Position position1 = new(1, 1);
        Position position2 = new(1, 1);

        Assert.That(position1.GetHashCode() == position2.GetHashCode(), Is.True, $"{position1} hashcode is equal to {position2}");
    }
}
