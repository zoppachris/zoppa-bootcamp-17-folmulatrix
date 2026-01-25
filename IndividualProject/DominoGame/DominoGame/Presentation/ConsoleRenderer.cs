using DominoGame.Domain.Actions;
using DominoGame.Domain.Events;
using DominoGame.Domain.Players.Interface;
using DominoGame.Game;

namespace DominoGame.Presentation
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

        private void RenderBoard()
        {
            Console.WriteLine("\nBoard:");
            Console.WriteLine(
                BoardFormatter.Format(_controller.Board)
            );
        }

        private void RenderHand(IPlayer player)
        {
            Console.WriteLine("\nYour Hand:");
            Console.WriteLine(
                HandFormatter.Format(player.Hand)
            );
        }

        private void OnTurnStarted(object? sender, TurnStartedEventArgs e)
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine($"Turn: {e.Player.Name}");
            Console.WriteLine($"Score: {e.Player.Score}");
            Console.WriteLine("==============================");

            RenderBoard();
            RenderHand(e.Player);
        }

        private void OnActionExecuted(object? sender, ActionExecutedEventArgs e)
        {
            Console.Clear();
            switch (e.Action)
            {
                case PlayDominoAction play:
                    Console.WriteLine($"{e.Player.Name} plays {play.Domino} on {play.Side}");
                    break;

                case PassAction:
                    Console.WriteLine($"{e.Player.Name} passes");
                    break;

                default:
                    Console.WriteLine($"{e.Player.Name} performed an action");
                    break;
            }
        }

        private void OnRoundEnded(object? sender, RoundEndedEventArgs e)
        {
            Console.WriteLine("\n=== ROUND ENDED ===");

            if (!e.Result.IsBlocked && e.Result.Winner != null)
            {
                Console.WriteLine($"Winner: {e.Result.Winner.Name}");
            }
            else
            {
                Console.WriteLine("Blocked game!");
            }

            Console.WriteLine("\nScores:");
            foreach (var player in e.Result.Players)
            {
                Console.WriteLine($"- {player.Name}: {player.Score}");
            }
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