using System.Collections.Concurrent;
using AliceMafia.Application;

namespace AliceMafia.Controllers
{
    public class ControllerData
    {
        public ConcurrentDictionary<string, GameLobby> lobbies { get; set; }

        public ControllerData()
        {
            lobbies = new ConcurrentDictionary<string, GameLobby>();
        }
    }
}