using DominoGame.Domain.Actions;
using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;
using DominoGame.Domain.Turns;
using DominoGame.Presentation.Interface;

namespace DominoGame.Presentation
{
    public sealed class ConsoleInputHandler : IPlayerInputHandler
    {
        public PlayerAction GetPlayerAction(TurnContext context)
        {
            if (!context.CanPlay)
            {
                Console.WriteLine("No possible move. Passing turn...");
                return new PassAction();
            }

            while (true)
            {
                Console.WriteLine("\nChoose action:");
                Console.WriteLine("1. Play Domino");
                Console.WriteLine("2. Pass");
                Console.Write("Select: ");

                string? input = Console.ReadLine();

                if (input == "2")
                    return new PassAction();

                if (input == "1")
                    return ReadPlayDomino(context);

                Console.WriteLine("Invalid input.");
            }
        }
        private PlayerAction ReadPlayDomino(TurnContext context)
        {
            while (true)
            {
                Console.Write("\nSelect domino index: ");
                if (!int.TryParse(Console.ReadLine(), out int index))
                {
                    Console.WriteLine("Invalid number.");
                    continue;
                }

                if (index < 0 || index >= context.Hand.Count)
                {
                    Console.WriteLine("Index out of range.");
                    continue;
                }

                Domino domino = context.Hand[index];

                Console.Write("Select side (L/R): ");
                string? sideInput = Console.ReadLine();

                BoardSide side;
                if (sideInput?.Equals("L", StringComparison.OrdinalIgnoreCase) == true)
                    side = BoardSide.Left;
                else if (sideInput?.Equals("R", StringComparison.OrdinalIgnoreCase) == true)
                    side = BoardSide.Right;
                else
                {
                    Console.WriteLine("Invalid side.");
                    continue;
                }

                return new PlayDominoAction(domino, side);
            }
        }
    }
}