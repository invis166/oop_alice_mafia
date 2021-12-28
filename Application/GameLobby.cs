using System;
using System.Timers;

namespace AliceMafia.Application
{
    public class GameLobby
    {
        public string Id { get; set; }

        public void AddPlayer(string player)
        {
            
        }

        public void StartGame()
        {
            
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