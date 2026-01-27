

using DominoGame.Domain.Entities;
using DominoGame.Domain.Game;
using DominoGame.UI;

namespace DominoGame
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("================================");
            Console.WriteLine("|                              |");
            Console.WriteLine("| Welcome to Block Domino Game |");
            Console.WriteLine("|                              |");
            Console.WriteLine("================================");


            List<Player> players = new List<Player>
            {
                new Player(1, "Player 1"),
                new Player(2, "Player 2")
            };


            GameController controller = new GameController(players);

            ConsoleRenderer renderer = new ConsoleRenderer(controller);

            Console.WriteLine("Press any key to start the game...");
            Console.ReadKey();

            while (!controller.IsGameEnded)
            {
                controller.StartRound();

                while (!controller.IsRoundEnded)
                {
                    try
                    {
                        controller.PlayerAction();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        Console.WriteLine("Please try again.\n");
                    }
                }

                Console.WriteLine("\nPress any key to start next round...");
                Console.ReadKey();
            }

            Console.WriteLine("\nGame finished.");
            Console.WriteLine($"Winner: {controller.GameWinner?.Name}");
            Console.ReadKey();
        }
    }
}
