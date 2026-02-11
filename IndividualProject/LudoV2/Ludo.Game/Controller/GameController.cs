using Ludo.Game.Enums;
using Ludo.Game.Interfaces;
using Ludo.Game.Logging;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.Piece;

namespace Ludo.Game.Controller
{
    public class GameController
    {
        private readonly IGameLogger _logger;

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

        public GameController(IGameLogger logger, IBoard board, IDice dice, List<IPlayer> players)
        {
            _logger = logger;
            _board = board;
            _dice = dice;
            _players = players;
            _random = new Random();

            _pieceInHands = new Dictionary<IPlayer, List<Piece>>();
            _stepsMoved = new Dictionary<Piece, int>();

            InitializePlayers();
            InitializePieces();

            _logger.Info($"Game created with {_players.Count} players");
        }

        public void StartGame()
        {
            _currentPlayerIndex = 0;
            _currentDiceValue = 0;
            _hasBonusTurn = false;
            _winner = null;

            _logger.Info("Game started");
        }
        public void ResetGame()
        {
            _logger.Info("Game is being reset");
            foreach (IPlayer player in _players)
            {
                int i = 0;

                List<Tile> homeTiles = _board.Tiles
                    .Cast<Tile>()
                    .Where(t => t.Zone == Zone.Home && t.Color == player.Color)
                    .ToList();

                foreach (Piece piece in _pieceInHands[player])
                {
                    _piecePositions[piece] = homeTiles[i];
                    _stepsMoved[piece] = -1;
                    i++;
                }
            }

            StartGame();
        }
        public IPlayer GetCurrentPlayer()
        {
            IPlayer currentPlayer = _players[_currentPlayerIndex];
            _logger.Info($"Current Player: {currentPlayer.Name} ({currentPlayer.Color})");
            return currentPlayer;
        }
        public Tile? GetPieceTile(Piece piece)
        {
            Tile? pieceTile = _piecePositions.TryGetValue(piece, out Tile? tile) ? tile : null;

            _logger.Info($"Piece {piece.Color} is at tile {pieceTile?.Position}");

            return pieceTile;
        }
        public int RollDice()
        {
            _currentDiceValue = _random.Next(1, _dice.Sides + 1);
            GetBonusTurnIfNeeded();

            _logger.Info($"Dice rolled: {_currentDiceValue}");
            return _currentDiceValue;
        }
        public List<Piece> GetPieces(IPlayer player)
        {
            List<Piece> playerPieces = _pieceInHands[player];

            _logger.Info($"Retrieved pieces for player {player.Name} :");
            foreach (var piece in playerPieces)
            {
                GetPieceTile(piece);
            }

            return playerPieces;
        }
        public List<Piece> GetMoveablePieces(IPlayer player)
        {
            List<Piece> result = _pieceInHands[player].FindAll(p => CanMovePiece(player, p));

            _logger.Info($"Retrieved moveable pieces for player {player.Name} : {result.Count}");

            if (result.Count > 0)
            {
                foreach (var piece in result)
                {
                    GetPieceTile(piece);
                }
            }
            else
            {
                _logger.Info("No moveable pieces available.");
            }

            return result;
        }
        public void MovePiece(IPlayer player, Piece piece)
        {
            if (!CanMovePiece(player, piece))
            {
                _logger.Warning($"{player.Name} cannot move this {piece.Color} piece");
                throw new InvalidOperationException("Piece cannot be moved.");
            }

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
                _stepsMoved[piece] = targetIndex + _currentDiceValue;
            }

            _logger.Info($"Moving piece {piece.Color} to tile {targetTile.Position}");

            HandleCaptureIfAny(player, piece, targetTile);

            _piecePositions[piece] = targetTile;

            GetBonusTurnIfNeeded(targetTile);

            CheckGameEndInternal(player);
        }
        public bool IsTurnFinished()
        {
            _logger.Info($"Checking if turn is finished: {!_hasBonusTurn}");
            return !_hasBonusTurn;
        }
        public void NextTurn()
        {
            _hasBonusTurn = false;
            _currentDiceValue = 0;
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

            _logger.Info($"Next turn: {_players[_currentPlayerIndex].Name}");
        }
        public bool IsGameOver()
        {
            _logger.Info("Game over check: " + (_winner != null));
            return _winner != null;
        }
        public bool IsPlayerWin(IPlayer player)
        {
            int pathLength = _board.ColorPaths[player.Color].Count;

            bool allFinished = _pieceInHands[player]
                .All(p => _stepsMoved[p] == pathLength - 1);

            if (allFinished)
            {
                _logger.Info($"Player {player.Name} has won.");
                return true;
            }

            _logger.Info($"Player {player.Name} has not won yet.");

            return false;
        }
        public IPlayer? GetWinner()
        {
            if (_winner != null)
            {
                _logger.Info($"Returning winner: {_winner.Name}");
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

            _logger.Info(winner != null
                ? $"Winner found: {winner.Name}"
                : "No winner yet.");
            return winner;
        }
        private void InitializePlayers()
        {
            _logger.Info("Initializing players and their pieces.");
            foreach (IPlayer player in _players)
            {
                _pieceInHands[player] = new List<Piece>();
            }
        }
        private void InitializePieces()
        {
            _logger.Info("Initializing pieces on the board.");
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

                _logger.Info($"{attacker.Name} captured a {piece.Color} piece at tile {targetTile.Position}");

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

            _logger.Info($"Piece {piece.Color} sent back to Home.");

            GetBonusTurnIfNeeded(isKilling: true);
        }
        private void CheckGameEndInternal(IPlayer player)
        {
            if (IsPlayerWin(player))
            {
                _winner = player;
                OnGameEnded?.Invoke(player);
            }
        }
        private void GetBonusTurnIfNeeded(Tile? targetTile = null, bool isKilling = false)
        {
            _hasBonusTurn = _currentDiceValue == _dice.Sides;

            if (targetTile != null && targetTile.Zone == Zone.Goal)
            {
                _hasBonusTurn = true;
            }

            if (isKilling)
            {
                _hasBonusTurn = true;
            }

            if (_hasBonusTurn)
            {
                _logger.Info($"{_players[_currentPlayerIndex]} gets a bonus turn.");
            }
        }


        // Cheat Testing
        public void MoveAllPieceToGoal(IPlayer player)
        {
            Tile goalTile = _board.ColorPaths[player.Color].Last();

            foreach (Piece piece in _pieceInHands[player])
            {
                _piecePositions[piece] = goalTile;
                _stepsMoved[piece] = _board.ColorPaths[player.Color].Count - 1;
            }

            CheckGameEndInternal(player);
        }
    }
}