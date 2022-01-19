using AliceMafia.Controllers;
using AliceMafia.Infrastructure;
using AliceMafia.Setting;
using Ninject;
using Ninject.Parameters;

namespace AliceMafia.Application
{
    public class UserContext : UserContextBase
    {
        public override void ChangeState(DialogStateBase state)
        {
            this.state = state;
        }
    }
}