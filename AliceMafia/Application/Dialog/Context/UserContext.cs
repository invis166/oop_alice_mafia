namespace AliceMafia.Application
{
    public class UserContext : IUserContext
    {
        public string PlayerName { get; set; }
        public string LobbyId { get; set; }
        
        public void ChangeState(DialogStateBase state)
        {
            throw new System.NotImplementedException();
        }
    }
}