using System.Collections.ObjectModel;
using LudoGame.Enums;
using LudoGame.Models.Board;
using LudoGame.Models.Dice;
using LudoGame.Models.Piece;
using LudoGame.Models.Player;

namespace LudoGame.Controller
{
    public class GameController
    {
        private readonly Board _board;
        private readonly Dice _dice;
        private readonly List<Player> _players;
        private readonly Random _random;
        private readonly Dictionary<Player, List<Piece>> _pieceInHands;
        private readonly Dictionary<Piece, int> _stepsMoved;
        private int _currentDiceValue;
        private int _currentPlayerIndex;
        public List<Player> Players => _players;
        public Action<Player, Piece>? OnCaptured;
        public Action<Player>? OnGameEnded;

        public GameController(Board board, Dice dice, List<Player> players)
        {
            _board = board;
            _dice = dice;
            _players = players;
            _random = new Random();

            _pieceInHands = new Dictionary<Player, List<Piece>>();
            _stepsMoved = new Dictionary<Piece, int>();

            InitializePlayers();
            InitializePieces();
        }
        public Player GetCurrentPlayer()
        {
            return _players[_currentPlayerIndex];
        }
        public void StartGame()
        {
            _currentPlayerIndex = 0;
            _currentDiceValue = 0;
        }
        public int RollDice()
        {
            _currentDiceValue = _random.Next(1, _dice.Sides + 1);
            return _currentDiceValue;
        }
        public List<Piece> GetPieces(Player player)
        {
            return _pieceInHands[player];
        }
        public List<Piece> GetMoveablePieces(Player player)
        {
            return _pieceInHands[player]
                .Where(p => CanMovePiece(player, p))
                .ToList();
        }
        public void MovePiece(Player player, Piece piece)
        {
            if (!CanMovePiece(player, piece))
                throw new InvalidOperationException("Piece cannot be moved.");

            List<Tile> path = _board.ColorPaths[piece.Color];

            // Piece dari Home
            if (_stepsMoved[piece] == -1)
            {
                _stepsMoved[piece] = 0;
                piece.CurrentTile = path[0];
                return;
            }

            // Piece di Main path
            int targetIndex = _stepsMoved[piece] + _currentDiceValue;
            Tile targetTile = path[targetIndex];

            HandleCaptureIfAny(player, piece, targetTile);

            _stepsMoved[piece] = targetIndex;
            piece.CurrentTile = targetTile;

            CheckGameEndInternal();
        }
        public bool IsTurnFinished()
        {
            return _currentDiceValue != _dice.Sides;
        }
        public void NextTurn()
        {
            _currentDiceValue = 0;
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }
        public bool IsGameOver()
        {
            return GetWinner() != null;
        }
        public Player? GetWinner()
        {
            foreach (Player player in _players)
            {
                int pathLength = _board.ColorPaths[player.Color].Count;

                bool allFinished = _pieceInHands[player]
                    .All(p => _stepsMoved[p] == pathLength - 1);

                if (allFinished)
                    return player;
            }

            return null;
        }
        public void DisplayPlayerPiece()
        {
            Player player = GetCurrentPlayer();
            List<Piece> pieces = GetPieces(player);

            Console.WriteLine($"{player.Name} pieces information :");
            for (int i = 1; i < pieces.Count; i++)
            {
                Console.WriteLine($"Piece {i} position {pieces[i].CurrentTile?.Position}");
            }
        }
        public void DisplayPieces(List<Piece> pieces)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                Console.WriteLine($"Piece {i + 1} position {pieces[i].CurrentTile?.Position}");
            }
        }
        private void InitializePlayers()
        {
            foreach (Player player in _players)
            {
                _pieceInHands[player] = new List<Piece>();
            }
        }
        private void InitializePieces()
        {
            foreach (Player player in _players)
            {
                List<Tile> homeTiles = _board.Tiles
                    .Cast<Tile>()
                    .Where(t => t.Zone == Zone.Home && t.Color == player.Color)
                    .ToList();

                for (int i = 0; i < 4; i++)
                {
                    Piece piece = new Piece(player.Color, homeTiles[i]);
                    _pieceInHands[player].Add(piece);
                    _stepsMoved[piece] = -1;
                }
            }
        }
        private bool CanMovePiece(Player player, Piece piece)
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
        private void HandleCaptureIfAny(Player attacker, Piece movingPiece, Tile targetTile)
        {
            if (targetTile.IsSafe)
                return;

            foreach (Piece enemy in _stepsMoved.Keys)
            {
                if (enemy.Color == movingPiece.Color)
                    continue;

                if (enemy.CurrentTile != targetTile)
                    continue;

                KillPiece(enemy);
                OnCaptured?.Invoke(attacker, enemy);
            }
        }
        private void KillPiece(Piece piece)
        {
            Tile homeTile = _board.Tiles
            .Cast<Tile>()
            .First(t => t.Zone == Zone.Home && t.Color == piece.Color);

            _stepsMoved[piece] = -1;
            piece.CurrentTile = homeTile;
        }
        private void CheckGameEndInternal()
        {
            Player? winner = GetWinner();
            if (winner != null)
                OnGameEnded?.Invoke(winner);
        }
    }
}