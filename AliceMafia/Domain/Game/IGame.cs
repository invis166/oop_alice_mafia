using System.Collections.Generic;
using AliceMafia.Setting;

namespace AliceMafia
{
    public interface IGame
    {
        public List<Player> Players{ get; }
        public void AddPlayer(string id, string name);
        public void StartGame();
        public void SetSetting(IGameSetting setting);
        public UserResponse HandleUserRequest(UserRequest request);
    }
}