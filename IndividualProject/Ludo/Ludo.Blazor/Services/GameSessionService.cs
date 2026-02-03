
using Ludo.Game.Controller;
using Ludo.Game.Models.Dice;
using Ludo.Game.Models.Player;
using Ludo.Game.Utils;

public sealed class GameSessionService
{
    public GameController? Game { get; private set; }

    public void CreateGame(List<Player> players)
    {
        var board = StandardBoard.GenerateBoard();
        var dice = new Dice(6);

        Game = new GameController(board, dice, players);
        Game.StartGame();
    }

    public bool HasGame => Game != null;

    public void Reset()
    {
        Game = null;
    }
}
