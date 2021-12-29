using System;
using System.Timers;

namespace AliceMafia.Application
{
    public class GameLobby
    {
        public string Id { get; set; }
        public bool GameStarted { get; set; }
        public int PlayersCount => game.Players.Count;
        private Game game = new Game();

        public void AddPlayer(string id, string name)
        {
            game.AddPlayer(id, name);
        }

        public void StartGame()
        {
            GameStarted = true;
            game.StartGame();
        }
        
        public AliceResponse HandleRequest(AliceRequest request)
        {
            throw new NotImplementedException();
        }

        public GameLobby()
        {
            Id = Math.Abs(DateTime.Now.GetHashCode()).ToString()[..6];
        }
    }
}