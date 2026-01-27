

using DominoGame.Domain.Entities;
using DominoGame.Game;
using DominoGame.UI;

namespace DominoGame
{
    public class Program
    {
        public static void Main()
        {
            PrintBanner();

            var players = CreatePlayers();

            var controller = new GameController(players);

            // Renderer subscribe ke event di constructor
            _ = new ConsoleRenderer(controller);

            Console.WriteLine("\nPress any key to start the game...");
            Console.ReadKey();

            RunGame(controller);

            Console.WriteLine("\nGame finished.");
            Console.WriteLine($"Winner: {controller.GameWinner?.Name}");
            Console.ReadKey();
        }

        private static void RunGame(GameController controller)
        {
            while (!controller.IsGameEnded)
            {
                controller.StartRound();

                while (!controller.IsRoundEnded)
                {
                    controller.PlayerAction();
                }

                if (!controller.IsGameEnded)
                {
                    Console.WriteLine("\nPress any key to start next round...");
                    Console.ReadKey();
                }
            }
        }

        private static List<Player> CreatePlayers()
        {
            return new List<Player>
        {
            new Player(1, "Player 1"),
            new Player(2, "Player 2")
        };
        }

        private static void PrintBanner()
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("|                              |");
            Console.WriteLine("| Welcome to Block Domino Game |");
            Console.WriteLine("|                              |");
            Console.WriteLine("================================");
        }
    }

}
