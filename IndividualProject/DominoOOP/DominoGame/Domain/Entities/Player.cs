namespace DominoGame.Domain.Entities
{
    public class Player
    {
        public List<Domino> Hand = new();
        public int Id { get; init; }
        public string Name { get; set; }
        public int Score { get; set; }

        public Player(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}