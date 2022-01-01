
namespace AliceMafia.Application
{
    public abstract class DialogStateBase
    {
        protected UserContextBase context;
        public DialogStateBase(UserContextBase context)
        {
            this.context = context;
        }

        public abstract AliceResponse HandleUserRequest(AliceRequest request);
    }
}