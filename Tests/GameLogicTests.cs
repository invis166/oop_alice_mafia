using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using AliceMafia.PlayerState;
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
            var gameState = GetPrivateField<GameContext>(game, "context").State;
            
            for (var j = 0; j < playersCount; j++)
            {
                game.HandleUserRequest(new UserRequest {Data = null, UserId = j.ToString()});
                var player = gameState.AlivePlayers.First(plr => plr.Id == j.ToString());
                Assert.True(player.State is DayWaitingState || j == playersCount - 1);
            }

            foreach (var player in gameState.AlivePlayers)
                Assert.True(player.State is NightWaitingState);
        }

        [TestCase(6)]
        [TestCase(5)]
        [TestCase(4)]
        public void TestSecondDayVoteKillPlayers(int playersCount)
        {
            var game = InitializeGame(playersCount);
            var gameState = GetPrivateField<GameContext>(game, "context").State;

            foreach (var player in gameState.AlivePlayers)
                player.State = new DayVotingState(player, GetPrivateField<GameContext>(game, "context"));

            var mafia = gameState.AlivePlayers.First(x => x.Role is Mafia);
            foreach (var player in gameState.AlivePlayers)
                game.HandleUserRequest(new UserRequest {UserId = player.Id, Payload = mafia.Id});
            
            foreach (var player in gameState.AlivePlayers)
                Assert.True(player.State is DayResultState);
            Assert.IsNotEmpty(gameState.AlivePlayers);
            Assert.IsEmpty(gameState.AlivePlayers.Where(player => player.Id == mafia.Id));
        }

        [TestCase(5)]
        public void TestJailMessage(int playersCount)
        {
            
            var game = InitializeGame(playersCount);
            var gameState = GetPrivateField<GameContext>(game, "context").State;
            var gameSetting= GetPrivateField<GameContext>(game, "context").Setting;

            foreach (var player in gameState.AlivePlayers)
                player.State = new DayVotingState(player, GetPrivateField<GameContext>(game, "context"));
            
            var victim = gameState.AlivePlayers.First(player => player.Role is not Mafia);
            foreach (var player in gameState.AlivePlayers)
                game.HandleUserRequest(new UserRequest {UserId = player.Id, Payload = victim.Id});

            foreach (var player in gameState.AlivePlayers)
            {
                Assert.True(player.State is DayResultState);
            }
            
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
            var gameState = GetPrivateField<GameContext>(game, "context").State;

            foreach (var player in gameState.AlivePlayers)
                player.State = new NightWaitingState(player, GetPrivateField<GameContext>(game, "context"));

            var mafiaPlayer = game.Players.First(x => x.Role is Mafia);
            var victim = gameState.AlivePlayers.First(player => player.Role is not Mafia);
            game.HandleUserRequest(new UserRequest {UserId = mafiaPlayer.Id});
            game.HandleUserRequest(new UserRequest {UserId = mafiaPlayer.Id, Payload = victim.Id});
            
            foreach (var player in gameState.AlivePlayers)
                Assert.True(player.State is NightResultState);
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
            var gameState = GetPrivateField<GameContext>(game, "context").State;
            
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
            var gameState = GetPrivateField<GameContext>(game, "context").State;

            foreach (var player in gameState.AlivePlayers)
                player.State = new NightWaitingState(player, GetPrivateField<GameContext>(game, "context"));

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
            
            var endNightMethod = GetEndNightMethod();
            var nightAction = new NightActionState(doctorPlayer, GetPrivateField<GameContext>(game, "context"));

            endNightMethod.Invoke(nightAction, System.Array.Empty<object>());

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

        public static T GetPrivateField<T>(Game game, string fieldName)
        {
            var field = game.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            return (T) field.GetValue(game);
        }
        
        private static MethodInfo GetEndNightMethod()
        {
            var field = typeof(NightActionState).GetMethod("HandleNightEnd", BindingFlags.NonPublic | BindingFlags.Instance);

            return field;
        }
    }
}
