using Microsoft.JSInterop;

public class SoundService
{
    private readonly IJSRuntime _js;

    public SoundService(IJSRuntime js)
    {
        _js = js;
    }

    public ValueTask PlayHomeBgm() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/home-bgm.mp3");

    public ValueTask PlayButtonStart() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/button-start.mp3");

    public ValueTask PlayButton() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/button-click.mp3");

    public ValueTask PlayAdd() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/button-add.mp3");

    public ValueTask PlayDelete() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/button-delete.mp3");

    public ValueTask PlayStart() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/game-start.mp3");

    public ValueTask PlayTurn() =>
            _js.InvokeVoidAsync("soundPlayer.play", "/sounds/player-turn.mp3");

    public ValueTask PlayDice() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/dice-roll.mp3");

    public ValueTask PlayCanMove() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/piece-can-move.mp3");

    public ValueTask PlayMove() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/piece-step.mp3");

    public ValueTask PlayKill() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/piece-kill.mp3");

    public ValueTask PlayGoal() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/piece-goal.mp3");

    public ValueTask PlayWinning() =>
        _js.InvokeVoidAsync("soundPlayer.play", "/sounds/winning-song.mp3");
}
