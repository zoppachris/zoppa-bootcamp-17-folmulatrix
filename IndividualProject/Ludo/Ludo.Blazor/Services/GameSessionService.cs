
using Ludo.Game.Controller;
using Ludo.Game.Interfaces;
using Ludo.Game.Models.Dice;
using Ludo.Game.Utils.BoardGenerate;

public sealed class GameSessionService
{
    public GameController? Game { get; private set; }

    public void CreateGame(List<IPlayer> players)
    {
        IBoard board = StandardBoard.GenerateBoard();
        IDice dice = new Dice(6);

        Game = new GameController(board, dice, players);
        Game.StartGame();
    }

    public bool HasGame => Game != null;

    public void Reset()
    {
        Game = null;
    }
}
