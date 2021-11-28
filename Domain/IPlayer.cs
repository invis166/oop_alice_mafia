namespace AliceMafia
{
    public enum PlayerState
    {
        Alive,
        Dead
    }
    
    public interface IPlayer
    {
        public int PlayerId { get; }
        public IRole Role { get; }
        public PlayerState State { get; set; }
        public IGame Game { get; }
    }
}