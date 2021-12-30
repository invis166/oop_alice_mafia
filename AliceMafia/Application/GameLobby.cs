using System;
using System.Collections.Generic;
using System.Linq;
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
            var userRequest = new UserRequest {UserId = request.Session.SessionId, Data = request.Request.Command, Payload = request.Request.Payload?.UserId}; 
            var userResponse = game.ProcessUserRequest(userRequest);

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
                State = new StateModel
                {
                    GameId = request.State.Session.GameId,
                    DgState = request.State.Session.DgState
                }
            };
        }

        public GameLobby()
        {
            Id = Math.Abs(DateTime.Now.GetHashCode()).ToString()[..6];
        }
    }
}