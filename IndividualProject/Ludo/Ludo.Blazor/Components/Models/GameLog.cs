using Ludo.Game.Interfaces;

public class GameLog
{
    public string Message { get; }
    public GameLogType Type { get; }
    public DateTime Time { get; }
    public IPlayer? Player { get; }

    public GameLog(
        string message,
        GameLogType type,
        DateTime time,
        IPlayer? player)
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
