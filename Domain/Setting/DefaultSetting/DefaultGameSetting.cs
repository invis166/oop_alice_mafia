using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AliceMafia.Setting.DefaultSetting
{
    public class DefaultGameSetting : IGameSetting
    {
        public IGeneralSetting GeneralMessages { get; }
        // public IRoleSetting Civilian { get; }
        // public IRoleSetting Mafia { get; }
        // public IRoleSetting Doctor { get; }
        // public IRoleSetting Sheriff { get; }
        // public IRoleSetting Courtesan { get; }
        public Dictionary<string, IRoleSetting> roles { get; }

        public DefaultGameSetting()
        {
            GeneralMessages = new DefaultGeneral();
            roles = new Dictionary<string, IRoleSetting>
            {
                ["civil"] = new DefaultCivilian(),
                ["doctor"] = new DefaultDoctor(),
                ["mafia"] = new DefaultMafia(),
                ["sheriff"] = new DefaultSheriff(),
                ["courtesan"] = new DefaulttCourtesan()
            };
            // var theList = Assembly.GetExecutingAssembly().GetTypes()
            //     .Where(t => t.Namespace == "AliceMafia.Setting.DefaultSetting" && t.IsSubclassOf(typeof(IRoleSetting)))
            //     .ToList();
            // // Civilian = new DefaultCivilian();
            // Mafia = new DefaultMafia();
            // Doctor = new DefaultDoctor();
            // Sheriff = new DefaultMafia();
            // Courtesan = new DefaulttCourtesan();
        }
        
        // public List<string> GetAllEntities()
        // {
        //     return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
        //         .Where(x => typeof(IDomainEntity).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
        //         .Select(x => x.Name).ToList();
        // }
    }
}