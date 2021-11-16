namespace AliceMafia
{
    public class LOL_Grandma : IRole
    {
        public string Name => "Бабка с дробовиком"; //Snapfire
        
        //an innocent who visits during the night and kills any player they wish;
        //if investigated by the Detective, they die, but if the Mafia attempts to kill her,
        //a random Mafia is killed; cannot be killed by the Mafia; no one knows if deaths are caused by her or another player

        public int ShootPlayer()
        {
            return 1;
        }
    }
}