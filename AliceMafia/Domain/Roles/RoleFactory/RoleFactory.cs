using AliceMafia.Infrastructure;
using Ninject;
using Ninject.Parameters;

namespace AliceMafia
{
    public class RoleFactory : RoleFactoryBase
    {
        private IReadOnlyKernel kernel;
        public RoleFactory(GameState state) : base(state)
        {
            kernel = DIContainer.ConfigurateContainer();
        }

        public override RoleBase CreateRole<TRole>()
        {
            return kernel.Get<TRole>(new ConstructorArgument("gameState", state, true));
        }
    }
}