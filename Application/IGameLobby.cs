using System;

namespace AliceMafia.Application
{
    public abstract class GameLobbyBase
    {
        public string Id { get; set; }

        public void AddPlayer(IPlayer player)
        {
            
        }

        public void StartGame()
        {
            
        }
        
        public AliceResponse HandleRequest(AliceRequest request)
        {
            throw new NotImplementedException();
        }
    }
}