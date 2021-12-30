using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultGeneral : IGeneralSetting
    {
        public string GameStartMessage { get; }
        public string DeathMessage => "К сожалению, вы мертвы.";
        public string DayVotingMessage => "Проголосуйте за того, кого считаете мафией:";
        public string DayWaitingMessage => "Ожидайте начала ночи.";
        public string NightWaitingMessage => "Ожидайте начала дня.";
        public string UndecidedJailMessage => "Игроки не пришли к единогласному решению на дневном голосовании.";
        public string DayEndMessage => "День окончен.";
        public string MafiaWinMessage => "Мафия победила.";
        public string PeacefulWinMessage => "Мирные жители победили.";

        public string GetKillMessage(List<string> names)
        {
            var joinedNames = string.Join(" ", names);
            
            return "Сегодня ночью не стало следующих игроков: " + joinedNames;
        }

        public string GetJailMessage(string name)
        {
            return $"По результатам голосования за решетку отправляется {name}";
        }
    }
}