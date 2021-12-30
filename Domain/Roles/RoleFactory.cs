using AliceMafia.Action;

namespace AliceMafia
{
    public class RoleFactory : RoleFactoryBase
    {
        public RoleFactory(GameState state) : base(state)
        {
        }
        
        public override Mafia CreateMafia()
        {
            return new Mafia(new MafiaAction(state));
        }

        public override Civilian CreateCivilian()
        {
            return new Civilian(new EmptyAction(state));
        }

        public override Doctor CreateDoctor()
        {
            return new Doctor(new DoctorAction(state));
        }

        public override Maniac CreateManiac()
        {
            return new Maniac(new ManiacAction(state));
        }

        public override Sheriff CreateSheriff()
        {
            return new Sheriff(new SheriffAction(state));
        }

        public override Courtesan CreateCourtesan()
        {
            return new Courtesan(new CourtesanAction(state));
        }

    }
}