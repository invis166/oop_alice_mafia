using System.Collections.Generic;

namespace AliceMafia.Setting
{
    public interface IGeneralSetting
    {
        public string DayStartMessage { get; }
        public string NightVotingMessage { get; }
        public string DayVotingMessage { get; }
        public string GetKillMessage(List<string> names);
        public string GetJailMessage(string name);
    }
}