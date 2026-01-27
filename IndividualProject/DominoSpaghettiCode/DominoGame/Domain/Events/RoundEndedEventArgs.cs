using DominoGame.Domain.Rounds;

namespace DominoGame.Domain.Events
{
    public sealed class RoundEndedEventArgs : EventArgs
    {
        public RoundResult Result { get; }

        public RoundEndedEventArgs(RoundResult result)
        {
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }
    }
}