using System.Collections.Generic;

namespace AliceMafia.Setting
{
    public interface IGeneralSetting
    {
        public string GameStartMessage { get; }
        public string DayVotingMessage { get; }
        public string DayWaitingMessage { get; }
        public string NightWaitingMessage { get; }
        public string GetKillMessage(List<string> names);
        public string GetJailMessage(string name);
    }
}