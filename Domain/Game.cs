using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace AliceMafia
{
    public class Game : IGame
    {
        public List<IPlayer> Players { get; }
        
        public void AddPlayer(IPlayer player)
        {
            throw new System.NotImplementedException();
        }

        public void StartNight()
        {
            // просыпается ВСЯ мафия, проходит голосование
            // после, в порядке приоритетности, просыпаются другие игроки и выполняют NightAction (в нем есть
            // строка, которая отвечает за то, что выводить на экран + голосование с 1 игроком)
             
        }

        public void StartDay()
        {
            // алиса говорит, кто умер
            // по очереди на экранах людей появляется надпись о том, что они могут говорить (определенное время),
            // далее переходит очередь к другому
            // наступает голосование
            // GetResult() из Voting
            // алиса говорит о том, кого посадили, вскрывает его роль (или нет)
            // 
        }

        public void Heal(IPlayer player)
        {
            if (player.State == PlayerState.Dead)
                player.State = PlayerState.Alive;
        }

        public void Kill(IPlayer player)
        {
            player.State = PlayerState.Dead;
        }

        public void GiveAlibi(IPlayer player)
        {
            throw new System.NotImplementedException();
        }

        public void CheckRole(IPlayer player)
        {
            throw new System.NotImplementedException();
        }
    }
}