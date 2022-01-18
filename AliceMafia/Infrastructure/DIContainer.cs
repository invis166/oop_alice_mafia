using AliceMafia.Action;
using AliceMafia.Application;
using Ninject;
using Ninject.Modules;

namespace AliceMafia.Infrastructure
{
    public static class DIContainer
    {
        public static IReadOnlyKernel ConfigurateContainer()
        {
            var kernel = new KernelConfiguration();
            kernel.Bind<RoleActionBase>().To<MafiaAction>().WhenInjectedInto<Mafia>();
            kernel.Bind<RoleActionBase>().To<ManiacAction>().WhenInjectedInto<Maniac>();
            kernel.Bind<RoleActionBase>().To<SheriffAction>().WhenInjectedInto<Sheriff>();
            kernel.Bind<RoleActionBase>().To<DoctorAction>().WhenInjectedInto<Doctor>();
            kernel.Bind<RoleActionBase>().To<CourtesanAction>().WhenInjectedInto<Courtesan>();
            kernel.Bind<RoleActionBase>().To<EmptyAction>().WhenInjectedInto<Civilian>();

            kernel.Bind<RoleFactoryBase>().To<RoleFactory>();
            kernel.Bind<UserContextBase>().To<UserContext>();
            kernel.Bind<IGame>().To<Game>();

            return kernel.BuildReadonlyKernel();
        }
        
    }
}