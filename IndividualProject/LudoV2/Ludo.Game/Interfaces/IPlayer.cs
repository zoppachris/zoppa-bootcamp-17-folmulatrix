using Ludo.Game.Enums;

namespace Ludo.Game.Interfaces
{
    public interface IPlayer
    {
        string Name { get; }
        Color Color { get; }
    }
}