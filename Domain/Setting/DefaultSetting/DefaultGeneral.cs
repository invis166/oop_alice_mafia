using System.Collections.Generic;

namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultGeneral : IGeneralSetting
    {
        public string DayStartMessage { get; }
        public string DayEndMessage { get; }
        public string NightVotingMessage { get; }
        public string DayVotingMessage { get; }
        public string GetKillMessage(List<string> names)
        {
            throw new System.NotImplementedException();
        }

        public string GetJailMessage(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}