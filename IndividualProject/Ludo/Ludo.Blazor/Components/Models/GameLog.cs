using Ludo.Game.Models.Player;

public class GameLog
{
    public string Message { get; }
    public GameLogType Type { get; }
    public DateTime Time { get; }
    public Player? Player { get; }

    public GameLog(
        string message,
        GameLogType type,
        DateTime time,
        Player? player)
    {
        Message = message;
        Type = type;
        Time = time;
        Player = player;
    }
}

public enum GameLogType
{
    Turn,
    Dice,
    Info,
    Action,
    Kill,
    Goal,
    Win
}
