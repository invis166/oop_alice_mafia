using System;
using System.Timers;

namespace AliceMafia.Application
{
    public class GameLobby
    {
        public string Id { get; set; }
        public bool GameStarted { get; set; }
        public int PlayersCount { get; set; }

        public void AddPlayer(string id, string name)
        {
            PlayersCount++;
        }

        public void StartGame()
        {
            GameStarted = true;
        }
        
        public AliceResponse HandleRequest(AliceRequest request)
        {
            throw new NotImplementedException();
        }

        public GameLobby()
        {
            Id = DateTime.Now.GetHashCode().ToString().Substring(0, 6);
        }
    }
}