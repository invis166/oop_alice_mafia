namespace AliceMafia.Application
{
    public interface IUserContext
    {
        public string PlayerName { get; set; }
        public string LobbyId { get; set; }
        public void ChangeState(DialogStateBase state);
    }
}