using System.Linq;
using NUnit.Framework;

namespace AliceMafia
{
    [TestFixture]
    public class GameTests
    {
        [TestCase(5)]
        [TestCase(4)]
        [TestCase(3)]
        public void TestFirstDayEnds(int playersCount)
        {
            var game = InitializeGame(playersCount);
            
            // узнаем свои роли
            for (var j = 0; j < playersCount; j++)
            {
                game.ProcessUserRequest(new UserRequest {Data = null, UserId = j.ToString()});
                var player = game.gameState.AlivePlayers.First(plr => plr.Id == j.ToString());
                Assert.True(player.State == PlayerState.DayWaiting);
            }

            Assert.True(game.gameState.TimeOfDay == TimeOfDay.Night);
            Assert.False(game.gameState.IsFirstDay);
        }
        
        [TestCase]
        public void TestThreePlayersMafiaKillAfterFirstNight()
        {
            var playersCount = 3;
            var game = InitializeGame(playersCount);
            
            SkipFirstDay(game);
            
            // переходим к ночи
            UserResponse mafiaChoice = new UserResponse();
            for (var j = 0; j < playersCount; j++)
            {
                if (game.Players.First(player => player.Name == j.ToString()).Role is Mafia)
                    mafiaChoice = game.ProcessUserRequest(new UserRequest {Data = null, UserId = j.ToString()});
            }

            // выбираем жертву за мафию
            var mafiaPlayer = game.Players.First(x => x.Role is Mafia);
            var victim = mafiaChoice.Buttons.First().Key;
            game.ProcessUserRequest(new UserRequest {UserId = mafiaPlayer.Id, Payload = victim});
            
            Assert.True(game.gameState.TimeOfDay == TimeOfDay.Day);
            Assert.IsEmpty(game.gameState.AlivePlayers.Where(player => player.Id == victim));
        }

        public static Game InitializeGame(int playersCount)
        {
            var game = new Game();
            for (var j = 0; j < playersCount; j++)
                game.AddPlayer(j.ToString(), j.ToString());
            
            game.StartGame();

            return game;
        }

        public static void SkipFirstDay(Game game)
        {
            foreach (var player in game.gameState.AlivePlayers)
            {
                game.ProcessUserRequest(new UserRequest {UserId = player.Id});
            }
        }
    }
}
