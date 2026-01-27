using DominoGame.Domain.Players.Human;
using DominoGame.Domain.Players.Interface;
using DominoGame.Domain.Scoring;
using DominoGame.Game;
using DominoGame.Presentation;
using DominoGame.Presentation.Interface;

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

            IPlayerInputHandler inputHandler = new ConsoleInputHandler();

            List<IPlayerMutable> players = new List<IPlayerMutable>
            {
                new HumanPlayer(1, "Player 1", inputHandler),
                new HumanPlayer(2, "Player 2", inputHandler)
            };

            IScoringStrategy normalWinScoring = new NormalWinScoring();
            IScoringStrategy blockedGameScoring = new BlockedGameScoring();

            GameController controller = new GameController(players, normalWinScoring, blockedGameScoring);

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
                        controller.PlayTurn();
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
