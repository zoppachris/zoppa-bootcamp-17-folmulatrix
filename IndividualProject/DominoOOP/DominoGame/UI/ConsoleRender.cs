using DominoGame.Domain.Events;
using DominoGame.Domain.Game;

namespace DominoGame.UI
{
    public sealed class ConsoleRenderer
    {
        private readonly GameController _controller;

        public ConsoleRenderer(GameController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _controller.TurnStarted += OnTurnStarted;
            _controller.ActionExecuted += OnActionExecuted;
            _controller.RoundEnded += OnRoundEnded;
            _controller.GameEnded += OnGameEnded;
        }

        private void OnTurnStarted(object? sender, TurnStartedEventArgs e)
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine($"Turn: {e.Player.Name}");
            Console.WriteLine($"Score: {e.Player.Score}");
            Console.WriteLine("==============================");

            RenderUtil.RenderBoard(e.Board.Dominoes.ToList().AsReadOnly());
            RenderUtil.RenderHand(e.Player.Hand);
        }
        private void OnActionExecuted(object? sender, ActionExecutedEventArgs e)
        {
            Console.Clear();
            if (e.Domino != null)
            {
                Console.WriteLine($"{e.Player.Name} plays {e.Domino} on {e.Side}");
            }
            else
            {
                Console.WriteLine($"{e.Player.Name} passes");
            }
        }

        private void OnRoundEnded(object? sender, RoundEndedEventArgs e)
        {
            Console.WriteLine("\n=== ROUND ENDED ===");

            if (!e.IsBlocked && e.Winner != null)
            {
                Console.WriteLine($"Winner: {e.Winner.Name}");
            }
            else
            {
                Console.WriteLine("Blocked game!");
            }

            // Console.WriteLine("\nScores:");
            // foreach (var player in e.Result.Players)
            // {
            //     Console.WriteLine($"- {player.Name}: {player.Score}");
            // }
        }

        private void OnGameEnded(object? sender, GameEndedEventArgs e)
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine($"GAME OVER");
            Console.WriteLine($"WINNER: {e.Winner.Name}");
            Console.WriteLine("==============================");
        }
    }
}