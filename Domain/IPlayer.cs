namespace AliceMafia
{
    public enum PlayerState
    {
        DayVoting,
        NightVoting,
        DayWaiting,
        NightWaiting
    }
    
    public interface IPlayer
    {
        public string Id { get; }
        public string Name { get; }
        public RoleBase Role { get; }
        public PlayerState State { get; set; }
        public bool HasVoted { get; set; }
    }
}