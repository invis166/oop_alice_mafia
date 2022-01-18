using AliceMafia.Setting;

namespace AliceMafia
{
    public class GameContext
    {
        public IGameSetting Setting { get; }
        public GameState State { get; }
        public GameContext(IGameSetting setting, GameState gameState)
        {
            Setting = setting;
            State = gameState;
        }
    }
}