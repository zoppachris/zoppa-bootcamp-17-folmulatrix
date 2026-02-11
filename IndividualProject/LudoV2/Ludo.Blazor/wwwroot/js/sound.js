window.soundPlayer = (function () {

  const sounds = {};
  let unlocked = false;

  function create(src, options = {}) {
    const audio = new Audio(src);
    audio.preload = "auto";
    audio.loop = options.loop ?? false;
    audio.volume = options.volume ?? 0.7;
    return audio;
  }

  function register(src, options = {}) {
    if (!sounds[src]) {
      sounds[src] = create(src, options);
    }
  }

  function preloadAll() {
    Object.values(sounds).forEach(audio => {
      audio.load();
    });
  }

  function unlock() {
    unlocked = true;

    Object.values(sounds).forEach(audio => {
      audio.muted = true;
      audio.play().then(() => {
        audio.pause();
        audio.currentTime = 0;
        audio.muted = false;
      }).catch(() => { });
    });
  }

  function play(src) {
    if (!unlocked) return;

    const audio = sounds[src];
    if (!audio) return;

    audio.currentTime = 0;
    audio.play();
  }

  function playLoop(src) {
    if (!unlocked) return;

    const audio = sounds[src];
    if (!audio) return;

    audio.loop = true;

    if (audio.paused) {
      audio.play();
    }
  }

  function stop(src) {
    const audio = sounds[src];
    if (!audio) return;

    audio.pause();
    audio.currentTime = 0;
  }

  return {
    register,
    preloadAll,
    unlock,
    play,
    playLoop,
    stop
  };
})();
