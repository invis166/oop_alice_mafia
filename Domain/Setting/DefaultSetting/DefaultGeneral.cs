using System.Collections.Generic;

namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultGeneral : IGeneralSetting
    {
        public string DayStartMessage { get; }
        public string DayEndMessage { get; }
        public string GetKillMessage(IEnumerable<string> names)
        {
            throw new System.NotImplementedException();
        }
    }
}