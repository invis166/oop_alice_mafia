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

        public void StartNight()
        {
            var notMafiaPlayers = GetAlivePlayersNames(player => !(player.Role is Mafia));
            var mafiaPlayers = gameState.AlivePlayers
                .Where(player => player.Role is Mafia)
                .ToList();
            var voteResult =  HandleVote(mafiaPlayers, notMafiaPlayers, gameSetting.GeneralMessages.NightVotingMessage);
            if (voteResult.Count == 1)
                gameState.AboutToKillPlayers.Add(voteResult.First());
            
            foreach (var player in Players
                .Where(player => !(player.Role is Mafia))
                .OrderBy(player => player.Role.Priority))
            {
                var id = player.Role.NightAction.CanActWithItself
                    ? askPlayer(player, GetAlivePlayersNames(x => true))
                    : askPlayer(player, GetAlivePlayersNames(x => x.Id != player.Id));
                sendMessageTo(player, player.Role.Setting.NightActionMessage);
                player.Role.NightAction.DoAction(GetPlayerById(id)); 
            }

            gameState.AboutToKillPlayers = gameState.AboutToKillPlayers
                .Where(player => player.Id != gameState.HealedPlayer.Id)
                .ToList();
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
                vote.AddVote(votedPlayer);

            return vote.GetResult();
        }

        public async void StartDay()
        {
            //todo в первый день не надо говорить кто умер, просто знакомство
            var deadPlayers = gameState.AboutToKillPlayers
                .Select(player => player.Name)
                .ToList();
            // алиса говорит, кто умер
            foreach (var player in Players)
            {
                sendMessageTo(player, gameSetting.GeneralMessages.DayStartMessage);
                sendMessageTo(player, gameSetting.GeneralMessages.GetKillMessage(deadPlayers));
            }
            // по очереди на экранах людей появляется надпись о том, что они могут говорить (определенное время),
            // далее переходит очередь к другому
            for (var i = 0; i < Players.Count; i++)
            {
                var currPlayer = Players[(i + gameState.GameCycleCount) % Players.Count];
                askPlayer(currPlayer, new List<string> { "закончить речь" });
            }

            // наступает голосование
            // todo в первый день не голосуем
            var candidates = GetAlivePlayersNames(player => player.Id != gameState.PlayerWithAlibi.Id);
            var voteResult = HandleVote(
                gameState.AlivePlayers.ToList(),
                candidates,
                gameSetting.GeneralMessages.DayVotingMessage);
            // алиса говорит о том, кого посадили, вскрывает его роль (или нет)
            if (voteResult.Count == 1)
            {
                var jailed = voteResult.First();
                foreach (var player in Players)
                    sendMessageTo(player, gameSetting.GeneralMessages.GetJailMessage(jailed.Name));
                gameState.AlivePlayers.Remove(jailed);
            }
            
            gameState.GameCycleCount++;
            gameState.Clear();
        }

        private IPlayer GetPlayerById(string id)
        {
            return Players.First(player => player.Id == id);
        }

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