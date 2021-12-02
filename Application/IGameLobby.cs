namespace AliceMafia.Application
{
    public abstract class GameLobbyBase
    {
        public string Id { get; set; }
        public void AddPlayer(IPlayer player);
        public void StartGame();
    }
}