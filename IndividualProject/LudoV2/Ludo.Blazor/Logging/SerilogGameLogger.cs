using Ludo.Game.Logging;

public class SerilogGameLogger : IGameLogger
{
    private readonly ILogger<SerilogGameLogger> _logger;

    public SerilogGameLogger(ILogger<SerilogGameLogger> logger)
    {
        _logger = logger;
    }

    public void Info(string message, params object[] args)
        => _logger.LogInformation(message, args);

    public void Warning(string message, params object[] args)
        => _logger.LogWarning(message, args);

    public void Error(Exception ex, string message, params object[] args)
        => _logger.LogError(ex, message, args);
}
