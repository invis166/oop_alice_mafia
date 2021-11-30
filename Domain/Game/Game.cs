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
            throw new System.NotImplementedException();
        }

        public async void StartNight()
        {
            var playersToKill = GetAlivePlayersNames(player => !(player.Role is Mafia));
            var mafiaPlayers = gameState.AlivePlayers
                .Where(player => player.Role is Mafia)
                .ToList();
            await StartVote(mafiaPlayers, playersToKill, gameSetting.GeneralMessages.NightVotingMessage);
            
            foreach (var player in Players
                .Where(player => !(player.Role is Mafia))
                .OrderBy(player => player.Role.Priority))
            {
                var id = player.Role.NightAction.CanActWithItself
                    ? askPlayer(player, GetAlivePlayersNames(x => true))
                    : askPlayer(player, GetAlivePlayersNames(x => x.PlayerId != player.PlayerId));
                sendMessageTo(player, player.Role.Setting.NightActionMessage);
                player.Role.NightAction.DoAction(GetPlayerById(id)); 
            }

            foreach (var player in gameState.SemiDeadPlayers)
                gameState.AlivePlayers.Remove(player);
        }

        private async Task StartVote(List<IPlayer> votingPlayers, List<string> candidatesNames, 
        string message)
        {
            var tasks = new List<Task<string>>();

            foreach (var player in votingPlayers)
            {
                sendMessageTo(player, message);
                tasks.Add(Task<string>.Run(() => askPlayer(player, candidatesNames
                    .Where(name => name != player.Name)
                    .ToList())));
            }

            await Task.WhenAll(tasks);

            var vote = new Vote<IPlayer>();
            foreach (var votedPlayer in tasks.Select(task => task.Result).Select(GetPlayerById))
                vote.AddVote(votedPlayer);
            
            var toKill = vote.GetResult(); 
            if (toKill.Count == 1) // если выбрали не одного, то никого не убивают, короче костыль чтобы работало
                gameState.SemiDeadPlayers.Add(toKill[0]);
        }

        public async void StartDay()
        {
            var deadPlayers = gameState.SemiDeadPlayers
                .Select(player => player.Name)
                .ToList();
            // алиса говорит, кто умер
            foreach (var player in Players)
            {
                sendMessageTo(player, gameSetting.GeneralMessages.DayStartMessage);
                sendMessageTo(player, gameSetting.GeneralMessages.GetKillMessage(deadPlayers));
            }
            gameState.SemiDeadPlayers.Clear();    
            // по очереди на экранах людей появляется надпись о том, что они могут говорить (определенное время),
            // далее переходит очередь к другому
            for (var i = 0; i < Players.Count; i++)
            {
                var currPlayer = Players[(i + gameState.GameCycleCount) % Players.Count];
                askPlayer(currPlayer, new List<string> { "закончить речь" });
            }

            // наступает голосование

            var candidates = gameState.AlivePlayers
                .Where(p => p.PlayerId != gameState.FuckedPlayer.PlayerId)
                .Select(p => p.Name)
                .OrderBy(p => p)
                .ToList();
            await StartVote(gameState.AlivePlayers.ToList(), candidates, gameSetting.GeneralMessages.DayVotingMessage);
            // алиса говорит о том, кого посадили, вскрывает его роль (или нет)
            gameState.SemiDeadPlayers.Clear();
            gameState.GameCycleCount++;
            // очистка gameState
            // todo
        }

        public IPlayer GetPlayerById(string id)
        {
            return Players.First(player => player.PlayerId == id);
        }

        public List<string> GetAlivePlayersNames(Func<IPlayer, bool> filter)
        {
            return gameState.AlivePlayers
                .Where(filter)
                .Select(player => player.Name)
                .OrderBy(x => x)
                .ToList();
        }
    }
}