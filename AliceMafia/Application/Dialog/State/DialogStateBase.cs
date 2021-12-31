
namespace AliceMafia.Application
{
    public abstract class DialogStateBase
    {
        private IUserContext context;
        public DialogStateBase(IUserContext context)
        {
            this.context = context;
        }

        public abstract void HandleUserRequest(AliceRequest request);
    }
}