namespace AliceMafia.Application
{
    public class SelectSettingState : DialogStateBase
    {
        public SelectSettingState(IUserContext context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}