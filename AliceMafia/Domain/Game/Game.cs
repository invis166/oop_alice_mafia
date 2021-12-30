﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AliceMafia.Setting;
using AliceMafia.Voting;
using System.Security.Policy;
using AliceMafia.Setting.DefaultSetting;


namespace AliceMafia
{
    public class Game : IGame
    {
        private IGameSetting gameSetting;
        private RoleFactoryBase roleFactory;
        public GameState gameState;
        public List<Player> Players { get; }

        public Game(IGameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
            gameState = new GameState();
            roleFactory = new RoleFactory(gameState);
            Players = new List<Player>();
        }

        public Game()
        {
            gameSetting = new DefaultGameSetting();
            gameState = new GameState();
            roleFactory = new RoleFactory(gameState);
            Players = new List<Player>();
        }
        
        public void AddPlayer(string id, string name)
        {
            Players.Add(new Player(id, name));
        }

        //todo
        public void StartGame()
        {
            SetRoles();
            foreach (var player in Players)
                gameState.AlivePlayers.Add(player);
        }

        private void SetRoles()
        {
            var playersCopyArray = new Player[Players.Count];
            Players.CopyTo(playersCopyArray);
            var playersCopyList = playersCopyArray.ToList();
            var random = new Random();

            var roles = new List<RoleBase>
            {
                roleFactory.CreateMafia(), roleFactory.CreateCourtesan(), roleFactory.CreateSheriff(),
                roleFactory.CreateDoctor(),
                roleFactory.CreateMafia()
            };

            for (var j = 0; j < 2 + (playersCopyList.Count + 1) % 2; j++)
            {
                var civilian = playersCopyList[random.Next(playersCopyList.Count)];
                civilian.Role = roleFactory.CreateCivilian();
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

        private UserResponse ProcessRequestWhileDay(UserRequest userRequest)
        {
            var currentPlayer = GetPlayerById(userRequest.UserId);
            if (!gameState.AlivePlayers.Contains(currentPlayer))
                return new UserResponse {Title = gameSetting.GeneralMessages.DeathMessage};
            
            if (currentPlayer.State == PlayerState.DayWaiting)
                return new UserResponse {Title = gameSetting.GeneralMessages.DayWaitingMessage};

            if (gameState.IsFirstDay)
                return HandleFirstDay(currentPlayer);
            
            if (currentPlayer.State == PlayerState.DayVoting)
                return HandleVote(userRequest, currentPlayer);
            
            if (currentPlayer.State == PlayerState.NightWaiting || currentPlayer.State == PlayerState.NightAction)
                return HandleDayStart(currentPlayer);
            
            return new UserResponse();
        }

        private UserResponse HandleDayStart(Player currentPlayer)
        {
            currentPlayer.State = PlayerState.DayVoting;

            return new UserResponse
            {
                Title = gameSetting
                            .GeneralMessages
                            .GetKillMessage(gameState
                                .AboutToKillPlayers
                                .Where(x => x.Id != gameState.HealedPlayer?.Id)
                                .Select(x => x.Name).ToList())
                        + gameSetting.GeneralMessages.DayVotingMessage,
                Buttons = gameState.AlivePlayers
                    .Where(x => x.Id != currentPlayer.Id && x.Id != gameState.PlayerWithAlibi?.Id)
                    .ToDictionary(keySelector: player => player.Id, elementSelector: player => player.Name)
            };
        }

        private UserResponse HandleVote(UserRequest userRequest, Player currentPlayer)
        {
            gameState.Voting.AddVote(GetPlayerById(userRequest.Payload));
            currentPlayer.State = PlayerState.DayWaiting;

            if (gameState.Voting.totalVoteCounter == gameState.AlivePlayers.Count)
            {
                gameState.TimeOfDay = TimeOfDay.Night;
                gameState.Clear();
                return new UserResponse {Title = "завершить день"};
            }

            return new UserResponse {Title = gameSetting.GeneralMessages.DayWaitingMessage};
        }

        private UserResponse HandleFirstDay(Player currentPlayer)
        {
            currentPlayer.State = PlayerState.DayWaiting;
            if (gameState.AlivePlayers.Count(player => player.State == PlayerState.DayWaiting) == gameState.AlivePlayers.Count)
            {
                gameState.TimeOfDay = TimeOfDay.Night;
                gameState.IsFirstDay = false;
                return new UserResponse {Title = gameSetting.GeneralMessages.DayEndMessage};
            }

            var roleName = gameSetting.roles[currentPlayer.Role.GetType().Name].Name;
            return new UserResponse {Title = $"Ваша роль {roleName}"};
        }

        private UserResponse ProcessRequestWhileNight(UserRequest userRequest)
        {
            var currentPlayer = GetPlayerById(userRequest.UserId);
            
            if (!gameState.AlivePlayers.Contains(currentPlayer))
                return new UserResponse {Title = gameSetting.GeneralMessages.DeathMessage};
            
            var currentPriority = currentPlayer.Role.Priority;
            
            if (currentPlayer.HasVoted || currentPriority != gameState.WhoseTurn)
            {
                currentPlayer.State = PlayerState.NightWaiting;
                return new UserResponse {Title = gameSetting.GeneralMessages.NightWaitingMessage};
            }
            
            if (currentPlayer.State == PlayerState.NightAction)
                return HandleNightAction(userRequest, currentPlayer, currentPriority);

            if (currentPriority == gameState.WhoseTurn)
                return HandleNightActionMessage(currentPlayer);

            return new UserResponse();
        }

        private UserResponse HandleNightAction(UserRequest userRequest, Player currentPlayer, int currentPriority)
        {
            currentPlayer.Role.NightAction.DoAction(GetPlayerById(userRequest.Payload));
            currentPlayer.State = PlayerState.NightWaiting;
            currentPlayer.HasVoted = true;

            var countOfNotVotedPlayers = gameState.AlivePlayers.Count(x
                => x.Role.Priority == gameState.WhoseTurn && !x.HasVoted);
            if (countOfNotVotedPlayers == 0)
            {
                var nextPriority = NextPriority(currentPriority);
                if (nextPriority == 0)
                {
                    gameState.TimeOfDay = TimeOfDay.Day;
                    var mafiaVoteResult = gameState.Voting.GetResult();
                    if (mafiaVoteResult.Count == 1)
                        gameState.AboutToKillPlayers.Add(mafiaVoteResult.First());
                    gameState.Voting = new Vote<Player>();
                    nextPriority = 1;
                    if (gameState.HealedPlayer != null)
                        gameState.AboutToKillPlayers.Remove(gameState.HealedPlayer);
                    gameState.AlivePlayers.ExceptWith(gameState.AboutToKillPlayers);
                }

                gameState.WhoseTurn = nextPriority;
            }

            if (currentPlayer.Role is Sheriff)
            {
                var isMafia = gameState.CheckedBySheriff.Role is Mafia;
                var mafiaName = gameSetting.roles["mafia"].Name;
                return new UserResponse {Title = "Игрок" + (isMafia ? "" : "не") + mafiaName};
            }

            return new UserResponse {Title = gameSetting.GeneralMessages.NightWaitingMessage};
        }

        private UserResponse HandleNightActionMessage(Player currentPlayer)
        {
            currentPlayer.State = PlayerState.NightAction;


            return new UserResponse
            {
                Title = gameSetting.roles[currentPlayer.Role.GetType().Name].NightActionMessage,
                Buttons = gameState.AlivePlayers
                    .Where(x => x.Id != currentPlayer.Id)
                    .ToDictionary(keySelector: player => player.Id, elementSelector: player => player.Name)
            };
        }

        public UserResponse ProcessUserRequest(UserRequest request)
        {
            if (gameState.TimeOfDay == TimeOfDay.Day)
                return ProcessRequestWhileDay(request);
            
            return ProcessRequestWhileNight(request);
        }

        public int NextPriority(int priority) => gameState
            .AlivePlayers
            .Select(x => x.Role.Priority)
            .OrderBy(x => x)
            .FirstOrDefault(x => x > priority);

       private Player GetPlayerById(string id) => Players.First(player => player.Id == id);
    }
}