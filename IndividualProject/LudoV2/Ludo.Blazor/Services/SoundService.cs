using Microsoft.JSInterop;

public class SoundService
{
    private readonly IJSRuntime _js;

    public SoundService(IJSRuntime js)
    {
        _js = js;
    }

    private readonly string[] _allSounds =
    {
        "/sounds/home-bgm.mp3",
        "/sounds/button-start.mp3",
        "/sounds/button-click.mp3",
        "/sounds/button-add.mp3",
        "/sounds/button-delete.mp3",
        "/sounds/game-start.mp3",
        "/sounds/player-turn.mp3",
        "/sounds/dice-roll.mp3",
        "/sounds/piece-can-move.mp3",
        "/sounds/piece-step.mp3",
        "/sounds/piece-kill.mp3",
        "/sounds/piece-goal.mp3",
        "/sounds/winning-song.mp3"
    };

    public async Task InitializeAsync()
    {
        foreach (var sound in _allSounds)
        {
            await _js.InvokeVoidAsync("soundPlayer.register", sound);
        }

        await _js.InvokeVoidAsync("soundPlayer.preloadAll");
    }

    public ValueTask Unlock() =>
        _js.InvokeVoidAsync("soundPlayer.unlock");

    public ValueTask Play(string src) =>
        _js.InvokeVoidAsync("soundPlayer.play", src);

    public ValueTask PlayLoop(string src) =>
        _js.InvokeVoidAsync("soundPlayer.playLoop", src);

    public ValueTask Stop(string src) =>
        _js.InvokeVoidAsync("soundPlayer.stop", src);

    public ValueTask PlayHomeBgm() =>
        _js.InvokeVoidAsync("soundPlayer.playLoop", "/sounds/home-bgm.mp3");

    public ValueTask StopHomeBgm() =>
     _js.InvokeVoidAsync("soundPlayer.stop", "/sounds/home-bgm.mp3");

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
