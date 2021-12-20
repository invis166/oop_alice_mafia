using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliceMafia.Setting;
using AliceMafia.Voting;
using System.Security.Policy;


namespace AliceMafia
{
    public class Game : IGame
    {
        private IGameSetting gameSetting;
        private GameState gameState;
        public List<IPlayer> Players { get; }

        public Game(IGameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
            gameState = new GameState();
        }
        
        public void AddPlayer(IPlayer player)
        {
            Players.Add(player);
        }

        //todo
        public void StartGame()
        {
            throw new NotImplementedException();
        }

        private UserResponse ProcessRequestWhileDay(UserRequest userRequest)
        {
            var currentPlayer = GetPlayerById(userRequest.id);
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
                
                return new UserResponse {Title = gameSetting.GeneralMessages.DayVotingMessage, 
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
                    return new UserResponse {Title = "ты шериф..."};
                return new UserResponse {Title = gameSetting.GeneralMessages.NightWaitingMessage};
            }

            if (currentPriority == gameState.WhoseTurn)
            {
                currentPlayer.State = PlayerState.NightAction;
                
                var countOfNotVotedPlayers = gameState.AlivePlayers.Count(x => x.Role.Priority == gameState.WhoseTurn && !x.HasVoted);
                if (countOfNotVotedPlayers == 1)
                {
                    var nextPriority = NextPriority(currentPriority);
                    if (nextPriority == 0)
                    {
                        gameState.TimeOfDay = TimeOfDay.Day;
                        var mafiaVoteResult = gameState.Voting.GetResult();
                        if (mafiaVoteResult.Count != 1)
                            gameState.AboutToKillPlayers.Add(mafiaVoteResult.First());
                        gameState.Voting = new Vote<IPlayer>();
                        nextPriority = 1;
                    }
                    gameState.WhoseTurn = nextPriority;
                }

                var sss = gameSetting.GetType().GetCustomAttributes(true);
                return new UserResponse {Title = gameSetting.Role.NightActionMessage, 
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

        private IPlayer GetPlayerById(string id) => Players.First(player => player.Id == id);
    }
}