using System.Collections.Generic;

namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultGeneral : IGeneralSetting
    {
        public string DayStartMessage { get; }
        public string DayEndMessage { get; }
        public string NightVotingMessage { get; }
        public string DayVotingMessage { get; }

        public string GetKillMessage(IEnumerable<string> names)
        {
            throw new System.NotImplementedException();
        }
    }
}