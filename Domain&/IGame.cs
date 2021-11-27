using System.Collections.Generic;

namespace AliceMafia
{
    public interface IGame
    {
        public List<IPlayer> Players{ get; }
        
        public void AddPlayer(IPlayer player);

        public void StartNight();
        public void StartDay();

        public void Heal(IPlayer player);
        public void Kill(IPlayer player);
        public void GiveAlibi(IPlayer player);
        public void CheckRole(IPlayer player);
        

    }
}