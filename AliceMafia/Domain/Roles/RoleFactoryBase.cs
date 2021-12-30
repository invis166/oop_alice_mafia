namespace AliceMafia
{
    public abstract class RoleFactoryBase
    {
        protected GameState state;
        public RoleFactoryBase(GameState state)
        {
            this.state = state;
        }

        public abstract Mafia CreateMafia();
        public abstract Civilian CreateCivilian();
        public abstract Doctor CreateDoctor();
        public abstract Maniac CreateManiac();
        public abstract Sheriff CreateSheriff();
        public abstract Courtesan CreateCourtesan();
    }
}