record GameLog(
    string Message,
    GameLogType Type,
    DateTime Time
);

enum GameLogType
{
    Turn,
    Dice,
    Info,
    Action,
    Kill,
    Goal,
    Win
}
