using AliceMafia.Infrastructure;

namespace AliceMafia.Application
{
    public interface IUserService
    {
        public IDatabaseController Database { get; }
    }
}