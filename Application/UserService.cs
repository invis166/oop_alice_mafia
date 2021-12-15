using AliceMafia.Infrastructure;

namespace AliceMafia.Application
{
    public class UserService : IUserService
    {
        private IDatabaseController _database;

        public IDatabaseController Database => _database;

        public UserService(IDatabaseController database)
        {
            _database = database;
        }
    }
}