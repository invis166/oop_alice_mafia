using AliceMafia.Application;

namespace AliceMafia.Infrastructure
{
    public interface IDatabaseController
    {
        public void UpdateDatabase(string gameId, GameLobbyBase lobby);
        public void Remove(string gameId);
        public GameLobbyBase GetLobby(string gameId);
    }
}