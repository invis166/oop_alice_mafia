namespace AliceMafia
{
    public abstract class RoleFactoryBase
    {
        protected GameState state;
        public RoleFactoryBase(GameState state)
        {
            this.state = state;
        }

        public abstract RoleBase CreateRole<TRole>()
            where TRole : RoleBase;
    }
}