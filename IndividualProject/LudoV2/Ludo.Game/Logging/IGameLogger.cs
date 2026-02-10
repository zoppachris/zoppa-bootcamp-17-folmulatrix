namespace Ludo.Game.Logging
{
    public interface IGameLogger
    {
        void Info(string message, params object[] args);
        void Warning(string message, params object[] args);
        void Error(Exception ex, string message, params object[] args);
    }
}