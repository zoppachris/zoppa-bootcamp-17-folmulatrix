using LudoGame.Controller;
using LudoGame.Enums;
using LudoGame.Models.Board;
using LudoGame.Models.Dice;
using LudoGame.Models.Piece;
using LudoGame.Models.Player;
using LudoGame.Utils;

namespace LudoGame
{
    class Program
    {
        static void Main()
        {
            // ===== INIT =====
            Board board = StandardBoard.GenerateBoard();
            Dice dice = new Dice(6);

            List<Player> players = new List<Player>
            {
                new Player("Player Red", Color.Red),
                new Player("Player Blue", Color.Blue),
                new Player("Player Yellow", Color.Yellow),
                new Player("Player Green", Color.Green),
            };

            GameController game = new GameController(board, dice, players);

            game.OnCaptured += (player, piece) =>
            {
                Console.WriteLine($"💥 {player.Name} captured a {piece.Color} piece!");
            };

            game.OnGameEnded += winner =>
            {
                Console.WriteLine($"\n🏆 GAME OVER! Winner: {winner.Name}");
            };

            game.StartGame();

            // ===== GAME LOOP =====
            while (!game.IsGameOver())
            {
                Console.Clear();

                Player currentPlayer = game.GetCurrentPlayer();
                Console.WriteLine($"🎮 Turn: {currentPlayer.Name} ({currentPlayer.Color})");

                Console.WriteLine("Press ENTER to roll dice...");
                Console.ReadLine();

                int diceValue = game.RollDice();
                Console.WriteLine($"🎲 Dice rolled: {diceValue}\n");

                List<Piece> moveablePieces = game.GetMoveablePieces(currentPlayer);
                ConsoleRenderer.RenderBoard(board, game, moveablePieces.ToHashSet());

                if (moveablePieces.Count == 0)
                {
                    Console.WriteLine("❌ No moveable pieces.");
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                    game.NextTurn();
                    continue;
                }

                Console.WriteLine("\nChoose piece to move:");
                for (int i = 0; i < moveablePieces.Count; i++)
                {
                    Piece piece = moveablePieces[i];
                    Console.WriteLine($"{i + 1}. Piece at {piece.CurrentTile?.Position}");
                }

                int choice = ReadChoice(1, moveablePieces.Count);
                Piece selectedPiece = moveablePieces[choice - 1];

                game.MovePiece(currentPlayer, selectedPiece);

                Console.WriteLine($"➡️ Moved piece to {selectedPiece.CurrentTile?.Position}");

                if (game.IsTurnFinished())
                {
                    Console.WriteLine("\nTurn finished. Press ENTER...");
                    Console.ReadLine();
                    game.NextTurn();
                }
                else
                {
                    Console.WriteLine("\n🎲 You get another turn!");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("\nPress ENTER to exit...");
            Console.ReadLine();
        }

        private static int ReadChoice(int min, int max)
        {
            while (true)
            {
                Console.Write("Input number: ");
                if (int.TryParse(Console.ReadLine(), out int value) &&
                    value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine("❌ Invalid input.");
            }
        }
    }
}