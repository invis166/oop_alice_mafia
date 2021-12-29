using System;
using System.Collections.Generic;
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
        private GameState gameState;
        public List<Player> Players { get; }

        public Game(IGameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
            gameState = new GameState();
        }

        public Game()
        {
            gameSetting = new DefaultGameSetting();
            gameState = new GameState();
        }
        
        public void AddPlayer(string id, string name)
        {
            Players.Add(new Player(id, name));
        }

        //todo
        public void StartGame()
        {
            var playersCopyArray = new Player[Players.Count];
            Players.CopyTo(playersCopyArray);
            var playersCopyList = playersCopyArray.ToList();
            var random = new Random();
            // распределить роли
            // запустить игру
            if (Players.Count < 8)
            {
                var mafia = Players[random.Next(Players.Count)];
                Players.Remove(mafia);
               
            }
            else
            {
                
            }
        }

        private UserResponse ProcessRequestWhileDay(UserRequest userRequest)
        {
            var currentPlayer = GetPlayerById(userRequest.id);
            if (!gameState.AlivePlayers.Contains(currentPlayer))
                return new UserResponse {Title = "Чел ты в гробе"};
            currentPlayer.HasVoted = false;

            if (gameState.IsFirstDay)
            {
                if (gameState.AlivePlayers.Count(player => player.HasVoted) == gameState.AlivePlayers.Count)
                {
                    gameState.TimeOfDay = TimeOfDay.Night;
                    gameState.IsFirstDay = false;
                    return new UserResponse {Title = "завершить день"};
                }
                currentPlayer.HasVoted = true;
                return new UserResponse {Title = gameSetting.GeneralMessages.GameStartMessage};
            }
            
            if (currentPlayer.State == PlayerState.DayVoting)
            {
                gameState.Voting.AddVote(GetPlayerById(userRequest.data));
                currentPlayer.State = PlayerState.DayWaiting;

                if (gameState.Voting.totalVoteCounter == gameState.AlivePlayers.Count)
                {
                    gameState.TimeOfDay = TimeOfDay.Night;
                    gameState.Clear();
                    return new UserResponse {Title = "завершить день"};
                }
                
                return new UserResponse {Title = gameSetting.GeneralMessages.DayWaitingMessage};
            }
            
            if (currentPlayer.State == PlayerState.DayWaiting)
                return new UserResponse {Title = gameSetting.GeneralMessages.DayWaitingMessage};
            if (currentPlayer.State == PlayerState.NightWaiting || currentPlayer.State == PlayerState.NightAction)
            {
                currentPlayer.State = PlayerState.DayVoting;
                
                return new UserResponse {Title = gameSetting
                                                     .GeneralMessages
                                                     .GetKillMessage(gameState
                                                         .AboutToKillPlayers
                                                         .Where(x => x.Id != gameState.HealedPlayer.Id)
                                                         .Select(x => x.Name).ToList()) 
                                                 + gameSetting.GeneralMessages.DayVotingMessage, 
                    Buttons = gameState.AlivePlayers
                        .Where(x => x.Id != currentPlayer.Id && x.Id != gameState.PlayerWithAlibi.Id)
                        .Select(x => x.Name)
                        .ToList()};   
            }
            
            return new UserResponse();
        }

        private UserResponse ProcessRequestWhileNight(UserRequest userRequest)
        {
            var currentPlayer = GetPlayerById(userRequest.id);
            if (!gameState.AlivePlayers.Contains(currentPlayer))
                return new UserResponse {Title = "Чел ты в гробе"};
            var currentPriority = currentPlayer.Role.Priority;
            if (currentPlayer.HasVoted || currentPriority != gameState.WhoseTurn || currentPlayer.State == PlayerState.DayWaiting)
            {
                currentPlayer.State = PlayerState.NightWaiting;
                return new UserResponse {Title = gameSetting.GeneralMessages.NightWaitingMessage};
            }
            
            if (currentPlayer.State == PlayerState.NightAction)
            {
                currentPlayer.Role.NightAction.DoAction(GetPlayerById(userRequest.data));
                currentPlayer.State = PlayerState.NightWaiting;
                currentPlayer.HasVoted = true;
                if (currentPlayer.Role is Sheriff)
                {
                    var isMafia = gameState.CheckedBySheriff.Role is Mafia;
                    var mafiaName = gameSetting.roles["mafia"].Name;
                    return new UserResponse {Title = "Игрок" + (isMafia ? "" : "не") + mafiaName};
                }

                return new UserResponse {Title = gameSetting.GeneralMessages.NightWaitingMessage};
            }

            if (currentPriority == gameState.WhoseTurn)
            {
                currentPlayer.State = PlayerState.NightAction;
                
                var countOfNotVotedPlayers = gameState.AlivePlayers.Count(x 
                    => x.Role.Priority == gameState.WhoseTurn && !x.HasVoted);
                if (countOfNotVotedPlayers == 1)
                {
                    var nextPriority = NextPriority(currentPriority);
                    if (nextPriority == 0)
                    {
                        gameState.TimeOfDay = TimeOfDay.Day;
                        var mafiaVoteResult = gameState.Voting.GetResult();
                        if (mafiaVoteResult.Count != 1)
                            gameState.AboutToKillPlayers.Add(mafiaVoteResult.First());
                        gameState.Voting = new Vote<Player>();
                        nextPriority = 1;
                        if (gameState.HealedPlayer != null)
                            gameState.AboutToKillPlayers.Remove(gameState.HealedPlayer);
                        gameState.AlivePlayers.ExceptWith(gameState.AboutToKillPlayers);
                    }
                    gameState.WhoseTurn = nextPriority;
                }

                return new UserResponse {Title = gameSetting.roles[currentPlayer.Role.GetType().Name].NightActionMessage, 
                    Buttons = gameState.AlivePlayers
                        .Where(x => x.Id != currentPlayer.Id)
                        .Select(x => x.Name)
                        .ToList()};
            }

            throw new StackOverflowException();
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