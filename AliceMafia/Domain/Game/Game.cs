using System;
using System.Collections.Generic;
using System.Linq;
using AliceMafia.Setting;
using AliceMafia.Voting;

namespace AliceMafia
{
    public class Game : IGame
    {
        private IGameSetting gameSetting;
        private RoleFactoryBase roleFactory;
        private GameState gameState;
        public List<Player> Players { get; }

        public Game(IGameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
            gameState = new GameState();
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

        private UserResponse ProcessRequestWhileDay(UserRequest userRequest)
        {
            var gameOver = CheckGameOver();
            if (gameOver.IsGameOver)
                return gameOver;
            
            var currentPlayer = GetPlayerById(userRequest.UserId);
            currentPlayer.HasVoted = false;
            if (!gameState.AlivePlayers.Contains(currentPlayer))
                return new UserResponse {Title = gameSetting.GeneralMessages.DeathMessage};

            if (currentPlayer.State == PlayerState.DayWaiting)
                return new UserResponse {Title = gameSetting.GeneralMessages.DayWaitingMessage};

            if (gameState.DaysCounter == 0)
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
            var killMessage = gameSetting.GeneralMessages.GetKillMessage(gameState
                .KilledAtNightPlayers
                .Where(x => x.Id != gameState.HealedPlayer?.Id)
                .Select(x => x.Name).ToList());
            
            return new UserResponse
            {
                Title = $"{killMessage}. {gameSetting.GeneralMessages.DayVotingMessage}",
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
                gameState.DaysCounter++;
                
                var votingResult = gameState.Voting.GetResult();
                if (votingResult.Count == 1)
                    gameState.AlivePlayers.Remove(votingResult.First());
                
                return new UserResponse {Title = gameSetting.GeneralMessages.DayEndMessage};
            }

            return new UserResponse {Title = gameSetting.GeneralMessages.DayWaitingMessage};
        }

        private UserResponse HandleFirstDay(Player currentPlayer)
        {
            currentPlayer.State = PlayerState.DayWaiting;
            string roleName;
            if (gameState.AlivePlayers.Count(player => player.State == PlayerState.DayWaiting) == gameState.AlivePlayers.Count)
            {
                gameState.TimeOfDay = TimeOfDay.Night;
                gameState.DaysCounter++;
                roleName = gameSetting.roles[currentPlayer.Role.GetType().Name].Name;
                return new UserResponse {Title = $"Ваша роль {roleName}"};
            }

            roleName = gameSetting.roles[currentPlayer.Role.GetType().Name].Name;
            return new UserResponse {Title = $"Ваша роль {roleName}"};
        }

        private UserResponse ProcessRequestWhileNight(UserRequest userRequest)
        {
            var gameOver = CheckGameOver();
            if (gameOver.IsGameOver)
                return gameOver;
            
            var currentPlayer = GetPlayerById(userRequest.UserId);
            
            if (!gameState.AlivePlayers.Contains(currentPlayer))
                return new UserResponse {Title = gameSetting.GeneralMessages.DeathMessage};
            
            if (gameState.AlivePlayers.All(player => player.State != PlayerState.DayWaiting))
                gameState.Clear();

            if (currentPlayer.State == PlayerState.DayWaiting && gameState.DaysCounter != 1)
                return HandleDayResult(userRequest);
            
            var currentPriority = currentPlayer.Role.Priority;
            if (currentPlayer.HasVoted || currentPriority != gameState.WhoseTurn)
            {
                currentPlayer.State = PlayerState.NightWaiting; // можно убрать наверное
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
                    foreach (var player in gameState.AlivePlayers)
                        player.State = PlayerState.NightWaiting;
                    var mafiaVoteResult = gameState.Voting.GetResult();
                    if (mafiaVoteResult.Count == 1)
                        gameState.KilledAtNightPlayers.Add(mafiaVoteResult.First());
                    gameState.Voting = new Vote<Player>();
                    nextPriority = 1;
                    if (gameState.HealedPlayer != null)
                        gameState.KilledAtNightPlayers.Remove(gameState.HealedPlayer);
                    gameState.AlivePlayers.ExceptWith(gameState.KilledAtNightPlayers);
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

        private UserResponse HandleDayResult(UserRequest request)
        {
            var currentPlayer = GetPlayerById(request.UserId);
            currentPlayer.State = PlayerState.NightWaiting;
            var voteResult = gameState.Voting.GetResult();
            if (voteResult.Count == 1)
            {
                var jailedPlayer = voteResult.First();
                return new UserResponse { Title = gameSetting.GeneralMessages.GetJailMessage(jailedPlayer.Name) };
            }

            return new UserResponse { Title = gameSetting.GeneralMessages.UndecidedJailMessage };
        }

        private UserResponse CheckGameOver()
        {
            var mafiaPlayersCount = gameState.AlivePlayers.Count(player => player.Role is Mafia);
            var peacefulPlayersCount = gameState.AlivePlayers.Count(player => !(player.Role is Mafia));

            if (peacefulPlayersCount == mafiaPlayersCount)
                return new UserResponse {Title = gameSetting.GeneralMessages.MafiaWinMessage, IsGameOver = true};
            if (mafiaPlayersCount == 0)
                return new UserResponse {Title = gameSetting.GeneralMessages.PeacefulWinMessage, IsGameOver = true};
            
            return new UserResponse {IsGameOver = false};
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