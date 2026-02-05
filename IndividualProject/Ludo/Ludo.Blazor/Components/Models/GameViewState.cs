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

    public async Task FakeRolling(int sides)
    {
        var random = new Random();

        for (int i = 0; i < 10; i++)
        {
            DisplayDiceValue = random.Next(1, sides + 1);
            await Task.Delay(80);
        }
    }

    /* =========================
     * Piece Animation
     * ========================= */
    public void AnimatePiece(Piece piece, Tile tile)
    {
        _animatingPieces[piece] = tile;
    }

    public void ClearAnimation(Piece piece)
    {
        _animatingPieces.Remove(piece);
    }

    /* =========================
     * Board Projection
     * ========================= */
    public Dictionary<Tile, List<Piece>> PiecesOnTiles(GameController game)
    {
        return game.PlayersPieces
            .SelectMany(kvp => kvp.Value)
            .Select(piece =>
            {
                var tile = _animatingPieces.TryGetValue(piece, out var animatedTile)
                    ? animatedTile
                    : piece.CurrentTile;

                return (piece, tile);
            })
            .Where(x => x.tile != null)
            .GroupBy(x => x.tile!)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.piece).ToList()
            );
    }

    /* =========================
     * Movement Helpers (UI only)
     * ========================= */
    public List<Tile> GetMovePath(Board board, Piece piece, int diceSides)
    {
        if (DisplayDiceValue <= 0)
            return new();

        var path = board.ColorPaths[piece.Color];

        // keluar dari home
        if (piece.CurrentTile == null || piece.CurrentTile.Zone == Zone.Home)
        {
            return DisplayDiceValue == diceSides
                ? new List<Tile> { path[0] }
                : new();
        }

        int start = path.IndexOf(piece.CurrentTile);
        int end = start + DisplayDiceValue;

        if (end >= path.Count)
            return new();

        return path
            .Skip(start + 1)
            .Take(DisplayDiceValue)
            .ToList();
    }

    public Tile? GetDestinationTile(Board board, Piece piece)
    {
        if (DisplayDiceValue <= 0)
            return null;

        var path = board.ColorPaths[piece.Color];

        if (piece.CurrentTile == null || piece.CurrentTile.Zone == Zone.Home)
            return DisplayDiceValue == 6 ? path[0] : null;

        int index = path.IndexOf(piece.CurrentTile);
        int target = index + DisplayDiceValue;

        return target < path.Count ? path[target] : null;
    }
}