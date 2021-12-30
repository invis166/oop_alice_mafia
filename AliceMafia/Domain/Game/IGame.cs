using System.Collections.Generic;

namespace AliceMafia
{
    public interface IGame
    {
        public List<Player> Players{ get; }
        public void AddPlayer(string id, string name);
        public void StartGame();
        public UserResponse ProcessUserRequest(UserRequest request);
    }
}