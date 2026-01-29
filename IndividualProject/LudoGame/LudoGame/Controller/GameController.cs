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
        private int _currentPlayerIndex;
        private readonly Dictionary<Player, List<Piece>> _pieceInHands;
        private readonly Dictionary<Piece, int> _stepsMoved;
        private readonly Random _random;
        public Action<Player, Piece>? OnCaptured;
        public Action<Player>? OnGameEnded;

        public GameController(Board board, Dice dice, List<Player> players)
        {
            _board = board;
            _dice = dice;
            _players = players;
            _currentPlayerIndex = 0;
            _random = new Random();
            _pieceInHands = new Dictionary<Player, List<Piece>>();
            _stepsMoved = new Dictionary<Piece, int>();

            InitializePieces();
        }
        public Player GetCurrentPlayer()
        {
            return _players[_currentPlayerIndex];
        }
        public void StartGame()
        {

        }
        public int RollDice()
        {
            return _random.Next(1, _dice.Sides);
        }
        public List<Piece> GetPieces(Player player)
        {
            return _pieceInHands[player];
        }
        // public List<Piece> GetMoveablePieces(Player player, int diceValue) { }
        public void MovePiece(Piece piece, int diceValue)
        {

        }
        public bool CanMovePiece(Piece piece, int diceValue)
        {
            return false;
        }
        public bool IsKill(Piece movePiece, Piece targetPiece)
        {
            return false;
        }
        public void KillPiece(Piece piece) { }
        public bool IsTurnFinished(int diceValue)
        {
            return diceValue != _dice.Sides;
        }
        public void NextTurn()
        {
            _currentPlayerIndex = _currentPlayerIndex + 1 % _players.Count;
        }
        public bool IsGameOver()
        {
            return false;
        }
        // public Player GetWinner() {}
        private void InitializePieces()
        {
            foreach (Player player in _players)
            {
                List<Piece> pieces = new List<Piece>();

                // Standard Ludo: 4 pieces per player
                for (int i = 0; i < 4; i++)
                {
                    Piece piece = new Piece(player.Color, currentTile: null);
                    pieces.Add(piece);
                    _stepsMoved[piece] = 0;
                }

                _pieceInHands[player] = pieces;
            }
        }
        private void PieceToHome(Piece piece)
        {

        }
    }
}