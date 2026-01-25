using DominoGame.Domain.Actions;
using DominoGame.Domain.Board;
using DominoGame.Domain.Collections;
using DominoGame.Domain.Entities;
using DominoGame.Domain.Enums;
using DominoGame.Domain.Events;
using DominoGame.Domain.Players;
using DominoGame.Domain.Players.Interface;
using DominoGame.Domain.Rounds;
using DominoGame.Domain.Scoring;
using DominoGame.Domain.Turns;

namespace DominoGame.Game
{
    public class GameController
    {
        public event EventHandler<TurnStartedEventArgs>? TurnStarted;
        public event EventHandler<ActionExecutedEventArgs>? ActionExecuted;
        public event EventHandler<RoundEndedEventArgs>? RoundEnded;
        public event EventHandler<GameEndedEventArgs>? GameEnded;

        private readonly Board _board;
        private readonly DominoSet _dominoSet;
        private readonly List<IPlayerMutable> _players;
        private readonly IScoringStrategy _normalWinScoring;
        private readonly IScoringStrategy _blockedGameScoring;
        private int _currentPlayerIndex;
        private bool _roundEnded;
        private int _consecutivePasses;
        private const int MaxScoreToWin = 20;

        public IReadOnlyBoard Board => _board;
        public bool IsRoundEnded => _roundEnded;
        public bool IsGameEnded { get; private set; }
        public IPlayer? GameWinner { get; private set; }

        public GameController(List<IPlayerMutable> players, IScoringStrategy normalWinScoring, IScoringStrategy blockedGameScoring)
        {
            if (players == null || players.Count == 0)
                throw new ArgumentException("Players cannot be empty.");

            _players = players;
            _normalWinScoring = normalWinScoring;
            _blockedGameScoring = blockedGameScoring;

            _board = new Board();
            _dominoSet = new DominoSet();

            _currentPlayerIndex = 0;
            _roundEnded = false;
            _consecutivePasses = 0;
            IsGameEnded = false;
        }
        public void StartRound()
        {
            if (IsGameEnded)
                throw new InvalidOperationException("Game already ended.");

            foreach (PlayerBase player in _players)
                player.ClearHand();

            _board.Clear();
            _dominoSet.Reset();
            _dominoSet.Shuffle();

            DealInitialHands();

            _roundEnded = false;
        }
        public void PlayTurn()
        {
            if (_roundEnded || IsGameEnded)
                return;

            IPlayerMutable player = GetCurrentPlayer();

            OnTurnStarted(player);

            TurnContext context = new TurnContext(
                _board,
                player.Hand
            );

            PlayerAction action = player.PlayTurn(context);

            ExecuteAction(player, action);

            RoundResult? roundResult = CheckRoundEnd();

            if (roundResult != null)
            {
                ApplyScoring(roundResult);
                _roundEnded = true;

                OnRoundEnded(roundResult);
                return;
            }

            MoveToNextPlayer();
        }
        private void ExecuteAction(IPlayerMutable player, PlayerAction action)
        {
            action.Execute(this, player);
            OnActionExecuted(player, action);
        }
        public void PlaceDomino(IPlayerMutable player, Domino domino, BoardSide side)
        {
            if (!player.Hand.Contains(domino))
            {
                throw new InvalidOperationException("\nPlayer does not own this domino.");
            }

            if (!_board.CanPlace(domino, side))
            {
                throw new InvalidOperationException($"\n{domino} cannot be placed on board from {side} side.");
            }

            _board.Place(domino, side);

            player.RemoveDomino(domino);
            _consecutivePasses = 0;
        }
        public void PassTurn(IPlayer player)
        {
            _consecutivePasses++;
            Console.WriteLine("Enter to continue.");
            Console.ReadKey();
        }
        private RoundResult? CheckRoundEnd()
        {
            // 1. Normal Win
            IPlayer? winner = _players.FirstOrDefault(p => p.Hand.Count == 0);
            if (winner != null)
            {
                return new RoundResult(
                    isBlocked: false,
                    winner: winner,
                    players: _players
                );
            }

            // 2. Blocked Win
            if (_consecutivePasses >= _players.Count)
            {
                return new RoundResult(
                    isBlocked: true,
                    winner: null,
                    players: _players
                );
            }

            return null;
        }
        private void ApplyScoring(RoundResult result)
        {
            IScoringStrategy strategy = result.IsBlocked ? _blockedGameScoring : _normalWinScoring;

            IReadOnlyDictionary<IPlayer, int> scores = strategy.CalculateScores(result);

            foreach (var entry in scores)
            {
                IPlayerMutable player = (IPlayerMutable)entry.Key;
                player.AddScore(entry.Value);
            }

            CheckGameEnd();
        }
        private void CheckGameEnd()
        {
            IPlayer? winner = _players.FirstOrDefault(player => player.Score >= MaxScoreToWin);
            if (winner != null)
            {
                IsGameEnded = true;
                GameWinner = winner;

                OnGameEnded(winner);
            }
        }
        private void DealInitialHands()
        {
            const int initialHandSize = 7;

            foreach (IPlayerMutable player in _players)
            {
                for (int i = 0; i < initialHandSize; i++)
                {
                    if (_dominoSet.IsEmpty())
                        return;

                    player.AddDomino(_dominoSet.Draw());
                }
            }
        }
        private void MoveToNextPlayer()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }
        public IPlayerMutable GetCurrentPlayer()
        {
            return _players[_currentPlayerIndex];
        }

        // On Methods
        protected virtual void OnTurnStarted(IPlayer player)
        {
            TurnStarted?.Invoke(
                this,
                new TurnStartedEventArgs(player)
            );
        }
        protected virtual void OnActionExecuted(IPlayer player, PlayerAction action)
        {
            ActionExecuted?.Invoke(
                this,
                new ActionExecutedEventArgs(player, action)
            );
        }
        protected virtual void OnRoundEnded(RoundResult result)
        {
            RoundEnded?.Invoke(
                this,
                new RoundEndedEventArgs(result)
            );
        }
        protected virtual void OnGameEnded(IPlayer winner)
        {
            GameEnded?.Invoke(
                this,
                new GameEndedEventArgs(winner)
            );
        }
    }
}