using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AliceMafia.Setting;
using AliceMafia.Voting;


namespace AliceMafia
{
    public class Game : IGame
    {
        private IGameSetting gameSetting;
        private GameState gameState;
        private Func<IPlayer, string> askPlayerForId;
        private Action<IPlayer, string> sendMessageTo;
        public List<IPlayer> Players { get; }

        public Game(IGameSetting gameSetting, Func<IPlayer, string> askPlayerForId, Action<IPlayer, string> sendMessageTo)
        {
            this.askPlayerForId = askPlayerForId;
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
            var tasks = new List<Task<string>>();
            foreach (var player in Players.Where(player => player.Role is Mafia))
            {
                sendMessageTo(player, gameSetting.Mafia.NightActionMessage);
                tasks.Add(Task<string>.Run(() => askPlayerForId(player)));
            }
            await Task.WhenAll(tasks);

            var vote = new Vote<IPlayer>();
            foreach (var votedPlayer in tasks.Select(task => task.Result).Select(GetPlayerById))
                vote.AddVote(votedPlayer);
            //todo разбить на методы
            
            foreach (var player in Players
                .Where(player => !(player.Role is Mafia))
                .OrderBy(player => player.Role.Priority))
            {
                var id = askPlayerForId(player);
                player.Role.NightAction.DoAction(GetPlayerById(id)); 
            } 
        }

        public void StartDay()
        {
            // алиса говорит, кто умер
            // по очереди на экранах людей появляется надпись о том, что они могут говорить (определенное время),
            // далее переходит очередь к другому
            // наступает голосование
            // GetResult() из Voting
            // алиса говорит о том, кого посадили, вскрывает его роль (или нет)
        }

        public IPlayer GetPlayerById(string id)
        {
            return Players.First(player => player.PlayerId == id);
        }
    }
    
    public class 
}