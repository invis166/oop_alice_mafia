using AliceMafia.Setting;

namespace AliceMafia
{
    public class GameContext
    {
        public IGameSetting Setting { get; set; }
        public GameState State { get; set; }
        public GameContext(IGameSetting setting, GameState gameState)
        {
            Setting = setting;
            State = gameState;
        }
    }
}