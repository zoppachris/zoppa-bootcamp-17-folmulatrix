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

            Console.WriteLine("Welcome to Ludo Game!!!");

            Console.WriteLine();

            List<Player> players = PlayerChoose();

            Console.WriteLine("\nList of player :");
            foreach (var player in players)
            {
                Console.WriteLine($"- {player.Name} - {player.Color}");
            }

            Console.WriteLine("\nEnter any key to start the game...");
            Console.ReadKey();

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
                    Console.WriteLine($"{i + 1}. Piece at {game.GetPieceTile(piece)?.Position}");
                }

                int choice = ReadChoice(1, moveablePieces.Count);
                Piece selectedPiece = moveablePieces[choice - 1];

                game.MovePiece(currentPlayer, selectedPiece);

                Console.WriteLine($"➡️ Moved piece to {game.GetPieceTile(selectedPiece)?.Position}");

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

        private static List<Player> PlayerChoose()
        {
            Console.WriteLine("Choose how many player will play?");
            Console.WriteLine("1. 2 Players");
            Console.WriteLine("2. 3 Players");
            Console.WriteLine("3. 4 Players");

            int choice = ReadChoice(1, 3);
            List<Color> colors = new() { Color.Blue, Color.Green, Color.Red, Color.Yellow };
            List<Player> players = new();

            for (int i = 1; i <= choice + 1; i++)
            {
                Console.WriteLine($"\nPlayer {i} you can pick your name.");
                string playerName = InputString("Input your name : ");
                Console.WriteLine("\nChoose your color :");
                for (int n = 0; n < colors.Count; n++)
                {
                    Console.WriteLine($"{n + 1}. {colors[n]}");
                }
                int choiseColor = ReadChoice(1, colors.Count);
                Color choicedColor = colors[choiseColor - 1];

                players.Add(new Player(playerName, choicedColor));
                colors.Remove(choicedColor);
            }

            return players;
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

        private static string InputString(string inputMessage = "Input : ")
        {
            while (true)
            {
                Console.Write(inputMessage);
                string? input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }

                Console.WriteLine("❌ Input cannot be empty.");
            }
        }
    }
}