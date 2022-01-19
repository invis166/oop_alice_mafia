using System;
using System.Collections.Generic;
using System.Linq;
using AliceMafia.PlayerState;
using AliceMafia.Setting;
using AliceMafia.Setting.DefaultSetting;
using AliceMafia.Voting;

namespace AliceMafia
{
    public class Game : IGame
    {
        private GameContext context;
        private RoleFactoryBase roleFactory;
        public List<Player> Players { get; }

        public Game()
        {
            var gameSetting = new DefaultGameSetting();
            var gameState = new GameState();
            
            context = new GameContext(gameSetting, gameState);
            roleFactory = new RoleFactory(gameState);
            Players = new List<Player>();
        }

        public void AddPlayer(string id, string name)
        {
            Players.Add(new Player(id, name));
        }

        public void StartGame()
        {
            SetRoles();
            foreach (var player in Players)
                context.State.AlivePlayers.Add(player);
        }

        public void SetSetting(IGameSetting setting)
        {
            context.Setting = setting;
        }

        private void SetRoles()
        {
            var playersCopyArray = new Player[Players.Count];
            Players.CopyTo(playersCopyArray);
            var playersCopyList = playersCopyArray.ToList();
            var random = new Random();

            var roles = new List<RoleBase>
            {
                roleFactory.CreateRole<Mafia>(), roleFactory.CreateRole<Courtesan>(), roleFactory.CreateRole<Sheriff>(),
                roleFactory.CreateRole<Doctor>(), roleFactory.CreateRole<Mafia>()
            };

            for (var j = 0; j < 2 + (playersCopyList.Count + 1) % 2; j++)
            {
                var civilian = playersCopyList[random.Next(playersCopyList.Count)];
                civilian.Role = roleFactory.CreateRole<Civilian>();
                playersCopyList.Remove(civilian);
            }

            while (playersCopyList.Count > 0)
            {
                var player = playersCopyList[random.Next(playersCopyList.Count)];
                player.Role = roles[^1];
                roles.RemoveAt(roles.Count - 1);
                playersCopyList.Remove(player);
            }
        }
        
        public UserResponse HandleUserRequest(UserRequest request)
        {
            var currentPlayer = context.State.GetAlivePlayerById(request.UserId);
            currentPlayer.State ??= new FirstDayState(currentPlayer, context);
            
            return currentPlayer.State.HandleUserRequest(request);
        }
    }
}