using DominoGame.Domain.Collections;
using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;
using DominoGame.Domain.Events;

namespace DominoGame.Domain.Game
{
    public class GameController
    {
        public event EventHandler<TurnStartedEventArgs>? TurnStarted;
        public event EventHandler<ActionExecutedEventArgs>? ActionExecuted;
        public event EventHandler<RoundEndedEventArgs>? RoundEnded;
        public event EventHandler<GameEndedEventArgs>? GameEnded;

        private Board _board;
        private Boneyard _boneyard;
        private List<Player> _players;
        private int _currentPlayerIndex;
        private bool _roundEnded;
        private int _consecutivePasses;
        private int _maxScoreToWin = 50;
        public Board Board => _board;
        public bool IsRoundEnded => _roundEnded;
        public bool IsGameEnded { get; private set; }
        public Player? GameWinner { get; private set; }

        public GameController(List<Player> players)
        {
            if (players == null || players.Count == 0)
                throw new ArgumentException("Players cannot be empty.");

            _players = players;

            _board = new Board();
            _boneyard = new Boneyard(GenerateFullSet());

            _currentPlayerIndex = 0;
            _roundEnded = false;
            _consecutivePasses = 0;
            IsGameEnded = false;
        }
        public Player GetCurrentPlayer()
        {
            return _players[_currentPlayerIndex];
        }
        public void StartRound()
        {
            if (IsGameEnded)
                throw new InvalidOperationException("Game already ended.");

            foreach (Player player in _players)
            {
                player.Hand.Clear();
            }

            _board.Dominoes.Clear();
            ResetBoneyard();
            ShuffleBoneyard();

            DealInitialHands();

            _currentPlayerIndex = 0;
            _roundEnded = false;

            PlayerAction();
        }
        private List<Domino> GenerateFullSet()
        {
            List<Domino> freshBoneyard = new List<Domino>();

            for (int left = 0; left <= 6; left++)
            {
                for (int right = left; right <= 6; right++)
                {
                    freshBoneyard.Add(new Domino((Dot)left, (Dot)right));
                }
            }

            return freshBoneyard;
        }
        private bool CanPlay(Player player)
        {
            if (player.Hand.Count == 0)
                return false;

            if (Board.IsEmpty)
                return true;

            foreach (Domino domino in player.Hand)
            {
                if (_board.CanPlace(domino))
                    return true;
            }

            return false;
        }
        public void PlayerAction()
        {
            if (_roundEnded || IsGameEnded)
                return;

            Player player = GetCurrentPlayer();

            OnTurnStarted(player, _board);

            if (!CanPlay(player))
            {
                Console.WriteLine("No possible move. Passing turn...");
                PassTurn(player);
            }

            while (true)
            {
                Console.WriteLine("\nChoose action:");
                Console.WriteLine("1. Play Domino");
                Console.WriteLine("2. Pass");
                Console.Write("Select: ");

                string? input = Console.ReadLine();

                if (input == "2")
                {
                    PassTurn(player);
                }

                if (input == "1")
                {
                    PlayTurn(player);
                }

                Console.WriteLine("Invalid input.");
            }
        }
        private void PlayTurn(Player player)
        {
            while (true)
            {
                Console.Write("\nSelect domino index: ");
                if (!int.TryParse(Console.ReadLine(), out int index))
                {
                    Console.WriteLine("Invalid number.");
                    continue;
                }

                if (index < 0 || index >= player.Hand.Count)
                {
                    Console.WriteLine("Index out of range.");
                    continue;
                }

                Domino domino = player.Hand[index];

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

                OnActionExecuted(player, domino, side);
                PlaceDomino(player, domino, side);
            }
        }
        private void PlaceDomino(Player player, Domino domino, BoardSide side)
        {
            if (!_board.CanPlace(domino, side))
            {
                throw new InvalidOperationException($"\n{domino} cannot be placed on board from {side} side.");
            }

            if (side == BoardSide.Left)
            {
                _board.Dominoes.AddFirst(domino);
            }
            else
            {
                _board.Dominoes.AddLast(domino);
            }

            player.Hand.Remove(domino);
            _consecutivePasses = 0;

            CheckRoundEnd();
        }
        private void PassTurn(Player player)
        {
            _consecutivePasses++;
            Console.WriteLine("Enter to continue.");
            Console.ReadKey();

            CheckRoundEnd();
        }
        private void CheckRoundEnd()
        {
            Player? winner = _players.FirstOrDefault(p => p.Hand.Count == 0);
            if (winner != null)
            {
                NormalScoring(winner);
            }

            if (_consecutivePasses >= _players.Count)
            {
                BlockedScoring();
            }

            MoveToNextPlayer();
        }
        private int CountPips(IEnumerable<Domino> dominoes)
        {
            int total = 0;

            foreach (Domino domino in dominoes)
            {
                total += (int)domino.LeftPip;
                total += (int)domino.RightPip;
            }

            return total;
        }
        private void NormalScoring(Player winner)
        {
            int totalOpponentPip = 0;

            foreach (Player player in _players)
            {
                if (player == winner)
                    return;

                totalOpponentPip += CountPips(player.Hand);
            }

            winner.Score += totalOpponentPip;

            _roundEnded = true;

            OnRoundEnded(winner, false);

            CheckGameEnd();
        }
        private void BlockedScoring()
        {
            Dictionary<Player, int> scores = new Dictionary<Player, int>();
            Dictionary<Player, int> pipCounts = new Dictionary<Player, int>();

            foreach (Player player in _players)
            {
                Console.WriteLine($"{player.Name}'s remaining dominoes : ");
                int pips = CountPips(player.Hand);
                pipCounts[player] = pips;
                scores[player] = 0;

                Console.WriteLine($"{player.Name} total pips value : {pips}");
            }

            int minPips = pipCounts.Values.Min();

            List<Player> winners = pipCounts
                .Where(kv => kv.Value == minPips)
                .Select(kv => kv.Key)
                .ToList();

            if (winners.Count != 1)
            {
                Console.WriteLine($"Every player have same total pips, Draw.");
                _roundEnded = true;
                CheckGameEnd();
                return;
            }

            Player winner = winners[0];

            int totalOpponentPips = pipCounts
                .Where(kv => kv.Key != winner)
                .Sum(kv => kv.Value);

            winner.Score += totalOpponentPips - pipCounts[winner];
            _roundEnded = true;

            Console.WriteLine($"{winner.Name} have the least total pips");
            Console.WriteLine($"Score given to {winner.Name},\ntotal sum all pips minus the winner total pips :");
            Console.WriteLine($"{totalOpponentPips} - {pipCounts[winner]} = {scores[winner]}");


            OnRoundEnded(winner, true);
            CheckGameEnd();
        }
        private void CheckGameEnd()
        {
            Player? winner = _players.FirstOrDefault(player => player.Score >= _maxScoreToWin);
            if (winner != null)
            {
                IsGameEnded = true;
                GameWinner = winner;
                OnGameEnded(winner);
            }
        }
        private void ResetBoneyard()
        {
            _boneyard.Dominoes.Clear();
            _boneyard = new Boneyard(GenerateFullSet());
        }
        private void ShuffleBoneyard()
        {
            Random random = new();

            for (int i = _boneyard.Dominoes.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (_boneyard.Dominoes[i], _boneyard.Dominoes[j]) = (_boneyard.Dominoes[j], _boneyard.Dominoes[i]);
            }
        }
        private Domino Draw()
        {
            if (_boneyard.IsEmpty())
                throw new InvalidOperationException("Boneyard is empty.");

            Domino domino = _boneyard.Dominoes[0];
            _boneyard.Dominoes.RemoveAt(0);

            return domino;
        }
        private void DealInitialHands()
        {
            const int initialHandSize = 7;

            foreach (Player player in _players)
            {
                for (int i = 0; i < initialHandSize; i++)
                {
                    if (_boneyard.Dominoes.Count == 0)
                        return;

                    player.Hand.Add(Draw());
                }
            }
        }
        private void MoveToNextPlayer()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }


        // On Methods
        protected virtual void OnTurnStarted(Player player, Board board)
        {
            TurnStarted?.Invoke(
                this,
                new TurnStartedEventArgs(player, board)
            );
        }
        protected virtual void OnActionExecuted(Player player, Domino domino, BoardSide side)
        {
            ActionExecuted?.Invoke(
                this,
                new ActionExecutedEventArgs(player, domino, side)
            );
        }
        protected virtual void OnRoundEnded(Player? player, bool isBlocked)
        {
            RoundEnded?.Invoke(
                this,
                new RoundEndedEventArgs(player, isBlocked)
            );
        }
        protected virtual void OnGameEnded(Player winner)
        {
            GameEnded?.Invoke(
                this,
                new GameEndedEventArgs(winner)
            );
        }
    }
}