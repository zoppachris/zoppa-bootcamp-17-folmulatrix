window.soundPlayer = {
    play: function (src) {
        const audio = new Audio(src);
        audio.volume = 0.8;
        audio.play();
    }
};