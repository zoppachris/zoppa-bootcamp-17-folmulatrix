using Ludo.Game.Controller;
using Ludo.Game.Interfaces;
using Ludo.Game.Logging;

namespace Ludo.Game.Controller
{
    public class GameControllerFactory
    {
        private readonly IGameLogger _logger;

        public GameControllerFactory(IGameLogger logger)
        {
            _logger = logger;
        }

        public GameController Create(
            IBoard board,
            IDice dice,
            List<IPlayer> players)
        {
            return new GameController(
                _logger,
                board,
                dice,
                players);
        }
    }
}