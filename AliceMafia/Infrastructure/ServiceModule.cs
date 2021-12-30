using AliceMafia.Action;
using Ninject.Modules;

namespace AliceMafia.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<RoleActionBase>().To<MafiaAction>().WhenInjectedInto<Mafia>();
            Bind<RoleActionBase>().To<ManiacAction>().WhenInjectedInto<Maniac>();
            Bind<RoleActionBase>().To<SheriffAction>().WhenInjectedInto<Sheriff>();
            Bind<RoleActionBase>().To<DoctorAction>().WhenInjectedInto<Doctor>();
            Bind<RoleActionBase>().To<CourtesanAction>().WhenInjectedInto<Courtesan>();
            Bind<RoleActionBase>().To<EmptyAction>().WhenInjectedInto<Civilian>();
            Bind<IGame>().To<Game>();
        }
    }
}