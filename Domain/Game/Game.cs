using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliceMafia.Setting;
using AliceMafia.Voting;


namespace AliceMafia
{
    public class Game : IGame
    {
        private IGameSetting gameSetting;
        private GameState gameState;
        private Func<IPlayer, List<string>, string> askPlayer;
        private Action<IPlayer, string> sendMessageTo;
        public List<IPlayer> Players { get; }

        public Game(IGameSetting gameSetting, Func<IPlayer, List<string>, string> askPlayer, Action<IPlayer, string> sendMessageTo)
        {
            this.askPlayer = askPlayer;
            this.gameSetting = gameSetting;
            this.sendMessageTo = sendMessageTo;
            gameState = new GameState();
        }
        
        public void AddPlayer(IPlayer player)
        {
            Players.Add(player);
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }

        public UserResponse ProcessUserRequest(UserRequest request)
        {
            // Где-то еще надо чистить GameState и прибавлять Cycle, а еще где-то надо голосование обработать
            var currentPlayer = GetPlayerById(request.id);
            
            // ДЕНЬ
            if (gameState.TimeOfDay == TimeOfDay.Day)
            {
                currentPlayer.HasVoted = false;
                if (currentPlayer.State == PlayerState.NightWaiting || currentPlayer.State == PlayerState.NightVoting)
                {
                    if (gameState.VoteCounter == gameState.AlivePlayers.Count)
                    {
                        currentPlayer.State = PlayerState.DayVoting;
                        return new UserResponse {Title = "голосуй днем", 
                            Buttons = gameState.AlivePlayers
                                .Where(x => x.Id != currentPlayer.Id)
                                .Select(x => x.Name)
                                .ToList()};
                        
                    }
                }
                currentPlayer.State = PlayerState.DayWaiting;
                return new UserResponse {Title = "жди днем"};
            }
            
            //НОЧЬ
            var currentPriority = currentPlayer.Role.Priority;
            if (currentPlayer.State == PlayerState.NightVoting || currentPriority != gameState.WhoseTurn)
            {
                currentPlayer.State = PlayerState.NightWaiting;
                return new UserResponse {Title = "жди ночью"};
            }

            currentPlayer.State = PlayerState.NightVoting;
            
            var countOfNotVotedPlayers = gameState.AlivePlayers.Count(x => x.Role.Priority == gameState.WhoseTurn && !x.HasVoted);
            
            if (countOfNotVotedPlayers == 1)
            {
                var nextPriority = NextPriority(currentPriority);
                gameState.WhoseTurn = nextPriority == 0 ? 1 : nextPriority;
            }

            currentPlayer.HasVoted = true;
            return new UserResponse {Title = "голосуй ночью", 
                Buttons = gameState.AlivePlayers
                    .Where(x => x.Id != currentPlayer.Id)
                    .Select(x => x.Name)
                    .ToList()};
        }

        public int NextPriority(int priority) => gameState
            .AlivePlayers
            .Select(x => x.Role.Priority)
            .OrderBy(x => x)
            .FirstOrDefault(x => x > priority);

        public void StartNight()
        {
            var notMafiaPlayers = GetAlivePlayersNames(player => !(player.Role is Mafia));
            var mafiaPlayers = gameState.AlivePlayers
                .Where(player => player.Role is Mafia)
                .ToList();
            var voteResult =  HandleVote(mafiaPlayers, notMafiaPlayers, 
                gameSetting.GeneralMessages.NightVotingMessage);
            if (voteResult.Count == 1)
                gameState.AboutToKillPlayers.Add(voteResult.First());
            
            HandleNightActions();

            gameState.AboutToKillPlayers = gameState.AboutToKillPlayers
                .Where(player => player.Id != gameState.HealedPlayer.Id)
                .ToList();
        }

        private void HandleNightActions()
        {
            foreach (var player in Players
                .Where(player => !(player.Role is Civilian) && !(player.Role is Mafia))
                .OrderBy(player => player.Role.Priority))
            {
                var id = player.Role.NightAction.CanActWithItself
                    ? askPlayer(player, GetAlivePlayersNames(x => true))
                    : askPlayer(player, GetAlivePlayersNames(x => x.Id != player.Id));
                sendMessageTo(player, player.Role.Setting.NightActionMessage);
                player.Role.NightAction.DoAction(GetPlayerById(id)); 
            }
        }

        private List<IPlayer> HandleVote(List<IPlayer> votingPlayers, List<string> candidatesNames, string message)
        {
            var tasks = new List<Task<string>>();

            foreach (var player in votingPlayers)
            {
                sendMessageTo(player, message);
                tasks.Add(Task<string>.Run(() => askPlayer(player, candidatesNames
                    .Where(name => name != player.Name)
                    .ToList())));
            }
            var task = Task.WhenAll(tasks);
            
            task.Wait();
            
            var vote = new Vote<IPlayer>();
            foreach (var votedPlayer in tasks.Select(task => task.Result).Select(GetPlayerById))
            {
                vote.AddVote(votedPlayer);
                gameState.VoteCounter++;
            }
            return vote.GetResult();
        }

        public async void StartDay()
        {
            // var deadPlayersNames = gameState.GameCycleCount != 0
            //     ? gameState.AboutToKillPlayers
            //         .Select(player => player.Name)
            //         .ToList()
            //     : null;
            
            var candidates = GetAlivePlayersNames(player => 
                !gameState.AboutToKillPlayers.Contains(player) && player.Id != gameState.PlayerWithAlibi.Id );
            HandleDayVoting(candidates);
            
            gameState.GameCycleCount++;
            gameState.Clear();
        }

        private void NotifyPlayers(string message)
        {
            if (message == null) return;
            foreach (var player in gameState.AlivePlayers)
                sendMessageTo(player, message);
        }

        private void HandleDayVoting(List<string> candidates)
        {
            if (gameState.GameCycleCount != 0)
            {
                var voteResult = HandleVote(
                    gameState.AlivePlayers.ToList(),
                    candidates,
                    gameSetting.GeneralMessages.DayVotingMessage);
                if (voteResult.Count == 1)
                {
                    var jailed = voteResult.First();
                    NotifyPlayers(gameSetting.GeneralMessages.GetJailMessage(jailed.Name));
                    gameState.AlivePlayers.Remove(jailed);
                }
            } 
        }

        private IPlayer GetPlayerById(string id) => Players.First(player => player.Id == id);

        private List<string> GetAlivePlayersNames(Func<IPlayer, bool> filter)
        {
            return gameState.AlivePlayers
                .Where(filter)
                .Select(player => player.Name)
                .OrderBy(x => x)
                .ToList();
        }
    }
}