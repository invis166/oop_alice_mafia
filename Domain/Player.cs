namespace AliceMafia
{
    public class Player
    {
        public string Id { get;  }
        public string Name { get; }
        public RoleBase Role { get; set; }
        public PlayerState State { get; set; }
        public bool HasVoted { get; set; }

        public Player(string id, string name)
        {
            Id = id;
            Name = name;
            State = PlayerState.DayVoting;
        }
    }
}