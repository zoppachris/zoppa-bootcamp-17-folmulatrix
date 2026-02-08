using Ludo.Game.Enums;
using Ludo.Game.Interfaces;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.Piece;

namespace Ludo.Game.Controller
{
    public class GameController
    {
        private readonly IBoard _board;
        private readonly IDice _dice;
        private readonly List<IPlayer> _players;
        private readonly Random _random;
        private readonly Dictionary<IPlayer, List<Piece>> _pieceInHands;
        private readonly Dictionary<Piece, int> _stepsMoved;
        private readonly Dictionary<Piece, Tile?> _piecePositions = new();
        private int _currentDiceValue;
        private int _currentPlayerIndex;
        private bool _hasBonusTurn;
        private IPlayer? _winner;
        public IDice Dice => _dice;
        public List<IPlayer> Players => _players;
        public IBoard Board => _board;
        public Dictionary<Piece, Tile?> PiecePositions => _piecePositions;
        public Action<IPlayer, Piece>? OnCaptured;
        public Action<IPlayer>? OnGameEnded;

        public GameController(IBoard board, IDice dice, List<IPlayer> players)
        {
            _board = board;
            _dice = dice;
            _players = players;
            _random = new Random();

            _pieceInHands = new Dictionary<IPlayer, List<Piece>>();
            _stepsMoved = new Dictionary<Piece, int>();

            InitializePlayers();
            InitializePieces();
        }
        public IPlayer GetCurrentPlayer()
        {
            return _players[_currentPlayerIndex];
        }
        public Tile? GetPieceTile(Piece piece)
        {
            return _piecePositions.TryGetValue(piece, out Tile? tile) ? tile : null;
        }
        public void StartGame()
        {
            _currentPlayerIndex = 0;
            _currentDiceValue = 0;
            _hasBonusTurn = false;
            _winner = null;
        }
        public int RollDice()
        {
            _currentDiceValue = _random.Next(1, _dice.Sides + 1);
            _hasBonusTurn = _currentDiceValue == _dice.Sides;
            return _currentDiceValue;
        }
        public List<Piece> GetPieces(IPlayer player)
        {
            return _pieceInHands[player];
        }
        public List<Piece> GetMoveablePieces(IPlayer player)
        {
            List<Piece> result = _pieceInHands[player].FindAll(p => CanMovePiece(player, p));
            return result;
        }
        public void MovePiece(IPlayer player, Piece piece)
        {
            if (!CanMovePiece(player, piece))
                throw new InvalidOperationException("Piece cannot be moved.");

            List<Tile> path = _board.ColorPaths[piece.Color];
            Tile? currentTile = _piecePositions[piece];
            Tile targetTile;

            if (currentTile == null || currentTile.Zone == Zone.Home)
            {
                // Piece dari Home
                targetTile = path[0];
                _stepsMoved[piece] = 0;
            }
            else
            {
                // Piece di Main
                int targetIndex = path.IndexOf(currentTile);
                targetTile = path[targetIndex + _currentDiceValue];
                _stepsMoved[piece] = targetIndex;
            }

            HandleCaptureIfAny(player, piece, targetTile);

            _piecePositions[piece] = targetTile;

            if (targetTile.Zone == Zone.Goal)
            {
                _hasBonusTurn = true;
            }

            CheckGameEndInternal(player);
        }
        public bool IsTurnFinished()
        {
            return !_hasBonusTurn;
        }
        public void NextTurn()
        {
            _hasBonusTurn = false;
            _currentDiceValue = 0;
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }
        public bool IsGameOver()
        {
            return _winner != null;
        }
        public bool IsPlayerWin(IPlayer player)
        {
            int pathLength = _board.ColorPaths[player.Color].Count;

            bool allFinished = _pieceInHands[player]
                .All(p => _stepsMoved[p] == pathLength - 1);

            if (allFinished)
            {
                return true;
            }

            return false;
        }
        public IPlayer? GetWinner()
        {
            if (_winner != null)
            {
                return _winner;
            }

            IPlayer? winner = null;

            foreach (IPlayer player in _players)
            {
                if (IsPlayerWin(player))
                {
                    _winner = player;
                    winner = player;
                    break;
                }
            }

            return winner;
        }
        private void InitializePlayers()
        {
            foreach (IPlayer player in _players)
            {
                _pieceInHands[player] = new List<Piece>();
            }
        }
        private void InitializePieces()
        {
            foreach (IPlayer player in _players)
            {
                List<Tile> homeTiles = _board.Tiles
                    .Cast<Tile>()
                    .Where(t => t.Zone == Zone.Home && t.Color == player.Color)
                    .ToList();

                for (int i = 0; i < 4; i++)
                {
                    Piece piece = new Piece(player.Color);
                    _piecePositions[piece] = homeTiles[i];
                    _pieceInHands[player].Add(piece);
                    _stepsMoved[piece] = -1;
                }
            }
        }
        private bool CanMovePiece(IPlayer player, Piece piece)
        {
            if (player.Color != piece.Color)
                return false;

            // Piece masih di Home
            if (_stepsMoved[piece] == -1)
            {
                return _currentDiceValue == _dice.Sides;
            }

            int targetIndex = _stepsMoved[piece] + _currentDiceValue;
            return targetIndex < _board.ColorPaths[piece.Color].Count;
        }
        private void HandleCaptureIfAny(IPlayer attacker, Piece movingPiece, Tile targetTile)
        {
            if (targetTile.IsSafe)
                return;

            foreach (Piece piece in _stepsMoved.Keys)
            {
                if (piece.Color == movingPiece.Color)
                    continue;

                if (_piecePositions[piece] != targetTile)
                    continue;

                KillPiece(piece);
                OnCaptured?.Invoke(attacker, piece);
            }
        }
        private void KillPiece(Piece piece)
        {
            Tile homeTile = _board.Tiles
            .Cast<Tile>()
            .First(t => t.Zone == Zone.Home && t.Color == piece.Color);

            _stepsMoved[piece] = -1;
            _piecePositions[piece] = homeTile;

            _hasBonusTurn = true; ;
        }
        private void CheckGameEndInternal(IPlayer player)
        {
            if (IsPlayerWin(player))
            {
                _winner = player;
                OnGameEnded?.Invoke(player);
            }
        }
    }
}