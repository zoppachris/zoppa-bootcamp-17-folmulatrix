using Ludo.Game.Controller;
using Ludo.Game.Enums;
using Ludo.Game.Models.Board;
using Ludo.Game.Models.Piece;

public class GameViewState
{
    public bool CanRollDice { get; private set; } = true;
    public bool IsRolling { get; private set; }
    public int DisplayDiceValue { get; private set; }

    public bool IsAnimating { get; set; }
    private readonly Dictionary<Piece, Tile?> _animatingPieces = new();

    public void StartRolling()
    {
        IsRolling = true;
        DisplayDiceValue = 0;
        CanRollDice = false;
    }

    public void StopRolling(int finalValue)
    {
        DisplayDiceValue = finalValue;
        IsRolling = false;
    }

    public void ResetDice()
    {
        CanRollDice = true;
        DisplayDiceValue = 0;
    }
    public void DisableDice()
    {
        CanRollDice = false;
        IsRolling = false;
    }
    public void SetDisplayDice(int value)
    {
        DisplayDiceValue = value;
    }
    public void AnimatePiece(Piece piece, Tile tile)
    {
        _animatingPieces[piece] = tile;
    }

    public void ClearAnimation(Piece piece)
    {
        _animatingPieces.Remove(piece);
    }

    public Dictionary<Tile, List<Piece>> PiecesOnTiles(GameController game)
    {
        return game.PiecePositions
            .Select(kvp =>
            {
                var piece = kvp.Key;

                var tile =
                    _animatingPieces.TryGetValue(piece, out var animatedTile)
                        ? animatedTile
                        : kvp.Value;

                return (piece, tile);
            })
            .Where(x => x.tile != null)
            .GroupBy(x => x.tile!)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.piece).ToList()
            );
    }

    public List<Tile> GetMovePath(
        GameController game,
        Board board,
        Piece piece)
    {
        if (DisplayDiceValue <= 0)
            return new();

        var currentTile = game.GetPieceTile(piece);
        var path = board.ColorPaths[piece.Color];

        // keluar dari home (visual only)
        if (currentTile == null || currentTile.Zone == Zone.Home)
        {
            return DisplayDiceValue == game.Dice.Sides
                ? new List<Tile> { path[0] }
                : new();
        }

        int start = path.IndexOf(currentTile);
        int end = start + DisplayDiceValue;

        if (end >= path.Count)
            return new();

        return path
            .Skip(start + 1)
            .Take(DisplayDiceValue)
            .ToList();
    }

    public Tile? GetDestinationTile(
        GameController game,
        Board board,
        Piece piece)
    {
        if (DisplayDiceValue <= 0)
            return null;

        var currentTile = game.GetPieceTile(piece);
        var path = board.ColorPaths[piece.Color];

        if (currentTile == null || currentTile.Zone == Zone.Home)
            return DisplayDiceValue == game.Dice.Sides
                ? path[0]
                : null;

        int index = path.IndexOf(currentTile);
        int target = index + DisplayDiceValue;

        return target < path.Count ? path[target] : null;
    }
}