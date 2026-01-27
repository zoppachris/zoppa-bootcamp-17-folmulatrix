using DominoGame.Domain.Events;
using DominoGame.Game;

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
            _controller.TurnStarted += TurnInformation;
            _controller.ActionExecuted += PlayPassAction;
            _controller.RoundEnded += RoundEndedNotification;
            _controller.GameEnded += GameEndedNotification;
        }

        private void TurnInformation(object? sender, TurnStartedEventArgs e)
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine($"Turn: {e.Player.Name}");
            Console.WriteLine($"Score: {e.Player.Score}");
            Console.WriteLine("==============================");

            RenderUtil.RenderBoard(e.Board.Dominoes.ToList().AsReadOnly());
            Console.WriteLine();
            RenderUtil.RenderHand(e.Player.Hand);
        }
        private void PlayPassAction(object? sender, ActionExecutedEventArgs e)
        {
            Console.Clear();
            if (e.Domino != null)
            {
                Console.WriteLine($"{e.Player.Name} placed {e.Domino} on {e.Side}");
            }
            else
            {
                Console.WriteLine($"{e.Player.Name} passes");
            }
        }
        private void RoundEndedNotification(object? sender, RoundEndedEventArgs e)
        {
            Console.WriteLine("\n=== ROUND ENDED ===");

            if (!e.IsBlocked && e.Winner != null)
            {
                Console.WriteLine($"Congratulation {e.Winner.Name} Win!!!");
                Console.WriteLine($"{e.Winner.Name} score : {e.Winner.Score}");
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
        private void GameEndedNotification(object? sender, GameEndedEventArgs e)
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine($"GAME OVER");
            Console.WriteLine($"WINNER: {e.Winner.Name}");
            Console.WriteLine("==============================");
        }
    }
}