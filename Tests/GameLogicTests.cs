using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AliceMafia.Setting;
using AliceMafia.Setting.DefaultSetting;
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
            var gameState = GetGameState(game);
            
            // узнаем свои роли
            for (var j = 0; j < playersCount; j++)
            {
                game.HandleUserRequest(new UserRequest {Data = null, UserId = j.ToString()});
                var player = gameState.AlivePlayers.First(plr => plr.Id == j.ToString());
                Assert.True(player.State == PlayerState.DayWaiting);
            }

            Assert.True(gameState.TimeOfDay == TimeOfDay.Night);
            Assert.True(gameState.DaysCounter != 0);
        }

        [TestCase(6)]
        [TestCase(5)]
        [TestCase(4)]
        public void TestSecondDayVoteKillPlayers(int playersCount)
        {
            var game = InitializeGame(playersCount);
            var gameState = GetGameState(game);
            gameState.DaysCounter = 2;

            var mafia = gameState.AlivePlayers.First(x => x.Role is Mafia);
            foreach (var player in gameState.AlivePlayers)
                game.HandleUserRequest(new UserRequest {UserId = player.Id, Payload = mafia.Id});
            
            Assert.True(gameState.TimeOfDay == TimeOfDay.Night);
            Assert.IsNotEmpty(gameState.AlivePlayers);
            Assert.IsEmpty(gameState.AlivePlayers.Where(player => player.Id == mafia.Id));
        }

        [TestCase(5)]
        public void TestJailMessage(int playersCount)
        {
            
            var game = InitializeGame(playersCount);
            var gameState = GetGameState(game);
            var gameSetting = GetGameSetting(game);

            gameState.DaysCounter = 2;
            var victim = gameState.AlivePlayers.First(player => player.Role is not Mafia);
            foreach (var player in gameState.AlivePlayers)
                game.HandleUserRequest(new UserRequest {UserId = player.Id, Payload = victim.Id});
            
            Assert.True(gameState.TimeOfDay == TimeOfDay.Night);
            foreach (var player in gameState.AlivePlayers)
            {
                var response = game.HandleUserRequest(new UserRequest {UserId = player.Id});
                Assert.True(response.Title == gameSetting.GeneralMessages.GetJailMessage(victim.Name));
            }

        }

        [TestCase]
        public void TestThreePlayersMafiaKillAfterFirstNight()
        {
            var playersCount = 3;
            var game = InitializeGame(playersCount);
            var gameState = GetGameState(game);

            gameState.DaysCounter = 1;
            gameState.TimeOfDay = TimeOfDay.Night;
            

            var mafiaPlayer = game.Players.First(x => x.Role is Mafia);
            var victim = gameState.AlivePlayers.First(player => player.Role is not Mafia);
            game.HandleUserRequest(new UserRequest {UserId = mafiaPlayer.Id});
            game.HandleUserRequest(new UserRequest {UserId = mafiaPlayer.Id, Payload = victim.Id});
            
            Assert.True(gameState.TimeOfDay == TimeOfDay.Day);
            Assert.IsEmpty(gameState.AlivePlayers.Where(player => player.Id == victim.Id));
        }

        [TestCase(3, 2, 1, 0, 0, 0)]
        [TestCase(4, 3, 1, 0, 0, 0)]
        [TestCase(5, 2, 1, 1, 1, 0)]
        [TestCase(6, 3, 1, 1, 1, 0)] 
        [TestCase(7, 2, 2, 1, 1, 1)]
        [TestCase(8, 3, 2, 1, 1, 1)]
        public void TestRolesDistribution(int playersCount, int civilianCount, int mafiaCount, int doctorCount, int sheriffCount, int courtesanCount)
        {
            var game = InitializeGame(playersCount);
            var gameState = GetGameState(game);
            
            Assert.True(gameState.AlivePlayers.Count == playersCount);
            Assert.True(gameState.AlivePlayers.Count(player => player.Role is Civilian) == civilianCount);
            Assert.True(gameState.AlivePlayers.Count(player => player.Role is Mafia) == mafiaCount);
            Assert.True(gameState.AlivePlayers.Count(player => player.Role is Doctor) == doctorCount);
            Assert.True(gameState.AlivePlayers.Count(player => player.Role is Sheriff) == sheriffCount);
            Assert.True(gameState.AlivePlayers.Count(player => player.Role is Courtesan) == courtesanCount);
        }

        [TestCase]
        public void TestDoctorHeals()
        {
            var playersCount = 5;
            var game = InitializeGame(playersCount);
            var gameState = GetGameState(game);
            
            gameState.DaysCounter = 2;
            gameState.TimeOfDay = TimeOfDay.Night;

            var doctorPlayer = gameState.AlivePlayers.First(player => player.Role is Doctor);
            var victim = gameState.AlivePlayers.First(player => player.Role is Civilian);
            var mafiaPlayers = gameState.AlivePlayers.Where(player => player.Role is Mafia);
            
            Assert.True(gameState.AlivePlayers.Contains(victim));
            
            foreach (var player in gameState.AlivePlayers)
                game.HandleUserRequest(new UserRequest {UserId = player.Id});
            
            
            foreach (var mafia in mafiaPlayers)
                game.HandleUserRequest(new UserRequest {UserId = mafia.Id, Payload = victim.Id});
            
            game.HandleUserRequest(new UserRequest {UserId = doctorPlayer.Id});
            game.HandleUserRequest(new UserRequest {UserId = doctorPlayer.Id, Payload = victim.Id});
            
            var endNightMethod = GetEndNightMethod(game);

            endNightMethod.Invoke(game, System.Array.Empty<object>());

            Assert.True(gameState.TimeOfDay == TimeOfDay.Day);
            Assert.True(gameState.AlivePlayers.Contains(victim));
        }
        
        public static Game InitializeGame(int playersCount)
        {
            var game = new Game();
            for (var j = 0; j < playersCount; j++)
                game.AddPlayer(j.ToString(), j.ToString());
            
            game.StartGame();

            return game;
        }

        private static GameState GetGameState(Game game)
        {
            var field = game.GetType().GetField("gameState", BindingFlags.NonPublic | BindingFlags.Instance);

            return (GameState) field.GetValue(game);
        }

        private static IGameSetting GetGameSetting(Game game)
        {
            var field = game.GetType().GetField("gameSetting", BindingFlags.NonPublic | BindingFlags.Instance);

            return (IGameSetting) field.GetValue(game);
        }
        
        private static MethodInfo GetEndNightMethod(Game game)
        {
            var field = game.GetType().GetMethod("EndNight", BindingFlags.NonPublic | BindingFlags.Instance);

            return field;
        }
    }
}
