using System.Collections.Generic;
using System.Linq;
using AliceMafia.Application;

namespace AliceMafia.Infrastructure
{
    public class DatabaseController : IDatabaseController
    {
        private IDictionary<string, GameLobbyBase> database;
        
        public DatabaseController()
        {
            database = new Dictionary<string, GameLobbyBase>();
        }
        
        public void UpdateDatabase(string gameId, GameLobbyBase lobby)
        {
            database.Add(gameId, lobby);
        }

        public void Remove(string gameId)
        {
            database.Remove(gameId);
        }

        public GameLobbyBase GetLobby(string gameId)
        {
            return database[gameId];
        }
    }
}