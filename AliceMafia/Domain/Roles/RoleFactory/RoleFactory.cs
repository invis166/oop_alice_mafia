using AliceMafia.Infrastructure;
using Ninject;
using Ninject.Parameters;

namespace AliceMafia
{
    public class RoleFactory : RoleFactoryBase
    {
        public RoleFactory(GameState state) : base(state)
        {
        }

        public override RoleBase CreateRole<TRole>()
        {
            var kernel = new StandardKernel(new ServiceModule());
            
            return kernel.Get<TRole>(new ConstructorArgument("gameState", state, true));
        }
    }
}