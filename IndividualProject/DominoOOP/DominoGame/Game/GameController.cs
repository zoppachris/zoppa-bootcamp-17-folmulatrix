using System.Runtime.Intrinsics.Arm;
using DominoGame.Domain.Collections;
using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;
using DominoGame.Domain.Events;

namespace DominoGame.Game
{
    public class GameController
    {
        #region Events
        public event EventHandler<TurnStartedEventArgs>? TurnStarted;
        public event EventHandler<ActionExecutedEventArgs>? ActionExecuted;
        public event EventHandler<RoundEndedEventArgs>? RoundEnded;
        public event EventHandler<GameEndedEventArgs>? GameEnded;
        #endregion

        #region Fields
        private readonly List<Player> _players = new();
        private Board _board;
        private Boneyard _boneyard;
        private int _currentPlayerIndex;
        private int _consecutivePasses;
        private bool _roundEnded;
        private readonly int _maxScoreToWin = 50;
        #endregion

        #region Properties
        public bool IsRoundEnded => _roundEnded;
        public bool IsGameEnded { get; private set; }
        public Player? GameWinner { get; private set; }
        #endregion

        #region Constructor
        public GameController(List<Player> players)
        {
            if (players == null || players.Count == 0)
                throw new ArgumentException("Players cannot be empty.");

            _players.AddRange(players);

            _board = new Board();
            _boneyard = new Boneyard(GenerateFullSet());
        }
        #endregion

        #region Public API
        public Player GetCurrentPlayer()
            => _players[_currentPlayerIndex];
        public void StartRound()
        {
            if (IsGameEnded)
                throw new InvalidOperationException("Game already ended.");

            ResetPlayers();
            ResetBoard();
            ResetBoneyard();

            DealInitialHands();

            _currentPlayerIndex = 0;
            _consecutivePasses = 0;
            _roundEnded = false;

            PlayerAction();
        }
        public void PlayerAction()
        {
            if (_roundEnded || IsGameEnded)
                return;

            Player player = GetCurrentPlayer();
            OnTurnStarted(player, _board);

            HandleTurn(player);
        }
        #endregion

        #region Turn Handling
        private void HandleTurn(Player player)
        {
            if (!CanPlay(player))
            {
                HandleForcedPass(player);
                return;
            }

            while (true)
            {
                ShowTurnMenu();

                string? input = Console.ReadLine();

                if (input == "1")
                {
                    HandlePlayAction(player);
                    break;
                }

                if (input == "2")
                {
                    HandlePassAction(player);
                    break;
                }

                Console.WriteLine("Invalid input.");
            }
        }
        private void HandlePlayAction(Player player)
        {
            Domino domino = SelectDomino(player);
            BoardSide side = SelectBoardSide();

            if (!_board.CanPlace(domino, side))
            {
                Console.WriteLine($"{domino} cannot be placed on {side}");
                return;
            }

            OnActionExecuted(player, domino, side);
            PlaceDomino(player, domino, side);

            CheckRoundEnd();
        }
        private void HandlePassAction(Player player)
        {
            OnActionExecuted(player, null, BoardSide.Left);

            Console.WriteLine("Passing turn...");
            WaitForContinue();

            CheckRoundEnd();
        }
        private void HandleForcedPass(Player player)
        {
            Console.WriteLine($"{player.Name} has no valid move. Forced pass.");
            _consecutivePasses++;
            WaitForContinue();

            CheckRoundEnd();
        }
        #endregion

        #region Placement Logic
        private void PlaceDomino(Player player, Domino domino, BoardSide side)
        {
            Domino placed = GetOrientedDomino(domino, side);

            if (_board.IsEmpty)
                _board.Dominoes.AddFirst(placed);
            else if (side == BoardSide.Left)
                _board.Dominoes.AddFirst(placed);
            else
                _board.Dominoes.AddLast(placed);

            player.Hand.Remove(domino);
            _consecutivePasses = 0;
        }
        private Domino GetOrientedDomino(Domino domino, BoardSide side)
        {
            if (_board.IsEmpty || domino.IsDouble())
                return domino;

            return side switch
            {
                BoardSide.Left when domino.RightPip == _board.LeftEnd => domino,
                BoardSide.Right when domino.LeftPip == _board.RightEnd => domino,
                _ => FlipDomino(domino)
            };
        }
        private Domino FlipDomino(Domino domino)
            => new Domino(domino.RightPip, domino.LeftPip);
        #endregion

        #region Round & Game End
        private void CheckRoundEnd()
        {
            Player? emptyHandWinner = _players.FirstOrDefault(p => p.Hand.Count == 0);

            if (emptyHandWinner != null)
            {
                HandleNormalWin(emptyHandWinner);
                return;
            }

            if (_consecutivePasses >= _players.Count)
            {
                HandleBlockedGame();
                return;
            }

            MoveToNextPlayer();
            PlayerAction();
        }
        private void HandleNormalWin(Player winner)
        {
            int score = _players
                .Where(p => p != winner)
                .Sum(p => CountPips(p.Hand));

            winner.Score += score;
            _roundEnded = true;

            OnRoundEnded(winner, false);
            CheckGameEnd();
        }
        private void HandleBlockedGame()
        {
            var pipMap = _players.ToDictionary(p => p, p => CountPips(p.Hand));
            int minPips = pipMap.Values.Min();

            var winners = pipMap.Where(kv => kv.Value == minPips).Select(kv => kv.Key).ToList();

            if (winners.Count != 1)
            {
                _roundEnded = true;
                CheckGameEnd();
                return;
            }

            Player winner = winners[0];
            int score = pipMap.Where(kv => kv.Key != winner).Sum(kv => kv.Value) - pipMap[winner];

            winner.Score += score;
            _roundEnded = true;

            OnRoundEnded(winner, true);
            CheckGameEnd();
        }
        private void CheckGameEnd()
        {
            Player? winner = _players.FirstOrDefault(p => p.Score >= _maxScoreToWin);
            if (winner == null)
                return;

            IsGameEnded = true;
            GameWinner = winner;
            OnGameEnded(winner);
        }
        #endregion

        #region Setup Helpers
        private void ResetPlayers()
        {
            foreach (Player player in _players)
                player.Hand.Clear();
        }
        private void ResetBoard()
            => _board.Dominoes.Clear();
        private void ResetBoneyard()
        {
            _boneyard = new Boneyard(GenerateFullSet());
            ShuffleBoneyard();
        }
        private void DealInitialHands()
        {
            const int handSize = 7;

            foreach (Player player in _players)
                for (int i = 0; i < handSize && !_boneyard.IsEmpty(); i++)
                    player.Hand.Add(Draw());
        }
        #endregion

        #region Utilities
        private List<Domino> GenerateFullSet()
        {
            var set = new List<Domino>();

            for (int i = 0; i <= 6; i++)
                for (int j = i; j <= 6; j++)
                    set.Add(new Domino((Dot)i, (Dot)j));

            return set;
        }
        private bool CanPlay(Player player)
            => _board.IsEmpty || player.Hand.Any(d => _board.CanPlace(d));
        private Domino Draw()
        {
            Domino domino = _boneyard.Dominoes[0];
            _boneyard.Dominoes.RemoveAt(0);
            return domino;
        }
        private void ShuffleBoneyard()
        {
            Random rnd = new();
            _boneyard.Dominoes = _boneyard.Dominoes.OrderBy(_ => rnd.Next()).ToList();
        }
        private int CountPips(IEnumerable<Domino> dominoes)
            => dominoes.Sum(d => (int)d.LeftPip + (int)d.RightPip);
        private void MoveToNextPlayer()
            => _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        private void ShowTurnMenu()
        {
            Console.WriteLine("\n1. Play Domino");
            Console.WriteLine("2. Pass");
            Console.Write("Select: ");
        }
        private Domino SelectDomino(Player player)
        {
            while (true)
            {
                Console.Write("Select domino index: ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int index))
                {
                    Console.WriteLine("Input must be a number.");
                    continue;
                }

                if (index < 0 || index >= player.Hand.Count)
                {
                    Console.WriteLine("Index out of range.");
                    continue;
                }

                return player.Hand[index];
            }
        }
        private BoardSide SelectBoardSide()
        {
            while (true)
            {
                Console.Write("Select side (L/R): ");
                string? input = Console.ReadLine();

                if (string.Equals(input, "L", StringComparison.OrdinalIgnoreCase))
                    return BoardSide.Left;

                if (string.Equals(input, "R", StringComparison.OrdinalIgnoreCase))
                    return BoardSide.Right;

                Console.WriteLine("Invalid side. Please enter L or R.");
            }
        }
        private void WaitForContinue()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
        #endregion

        #region On Methods
        protected virtual void OnTurnStarted(Player player, Board board)
            => TurnStarted?.Invoke(this, new TurnStartedEventArgs(player, board));
        protected virtual void OnActionExecuted(Player player, Domino? domino, BoardSide side)
            => ActionExecuted?.Invoke(this, new ActionExecutedEventArgs(player, domino, side));
        protected virtual void OnRoundEnded(Player? player, bool isBlocked)
            => RoundEnded?.Invoke(this, new RoundEndedEventArgs(player, isBlocked));
        protected virtual void OnGameEnded(Player winner)
            => GameEnded?.Invoke(this, new GameEndedEventArgs(winner));
        #endregion
    }
}