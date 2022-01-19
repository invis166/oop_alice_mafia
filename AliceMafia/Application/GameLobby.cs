using System;
using System.Collections.Generic;
using System.Linq;

namespace AliceMafia.Application
{
    public class GameLobby
    {
        public string Id { get; set; }
        public bool GameStarted { get; set; }
        public int PlayersCount => game.Players.Count;
        private IGame game;

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
            var userRequest = new UserRequest {UserId = request.Session.SessionId, Data = request.Request.Command, Payload = request.Request.Payload?.UserId}; 
            var userResponse = game.HandleUserRequest(userRequest);

            if (userResponse.IsGameOver)
            {
            }

            List<ButtonModel> buttons;
            if (userResponse.Buttons is null)
                buttons = new List<ButtonModel> {new ButtonModel {Title = "Далее"}};
            else
                buttons = userResponse.Buttons
                    .Select(btn => new ButtonModel {Title = btn.Value, Payload = new PayloadModel {UserId = btn.Key}, Hide = true})
                    .ToList();
            
            return new AliceResponse
            {
                Response = new ResponseModel
                {
                    Text = userResponse.Title,
                    Buttons = buttons
                },
            };
        }

        public GameLobby(IGame game)
        {
            Id = Math.Abs(DateTime.Now.GetHashCode()).ToString()[..6];
            this.game = game;
        }
    }
}