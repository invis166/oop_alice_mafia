using System.Collections.Generic;

namespace AliceMafia.Setting
{
    public interface IGeneralSetting
    {
        public string DeathMessage { get; }
        public string DayVotingMessage { get; }
        public string DayWaitingMessage { get; }
        public string NightWaitingMessage { get; }
        public string UndecidedJailMessage { get; }
        public string DayEndMessage { get; }
        public string MafiaWinMessage { get; }
        public string PeacefulWinMessage { get; }
        public string AfterVotingMessage { get; }
        public string GetKillMessage(List<string> names);
        public string GetJailMessage(string name);
    }
}