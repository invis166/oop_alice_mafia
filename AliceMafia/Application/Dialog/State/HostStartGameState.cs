using System;
using System.Collections.Generic;
using AliceMafia.Application.Dialog;
using AliceMafia.Controllers;

namespace AliceMafia.Application
{
    public class HostStartGameState : DialogStateBase
    {
        public HostStartGameState(UserContextBase context) : base(context)
        {
        }

        private Random rnd = new Random();
        private List<string> gameStartTexts = new List<string>
        {
            "В мирный и ничем не примечательный городок пришла беда: жители узнали, что теперь среди них живет мафия, " +
            "готовая уничтожить все и вся на своем пути. Вы не хотите засыпать, ведь понимаете, что эта ночь станет роковой, но все же делаете это.",
            "Пока вы возвращались с работы, вы заметили, что на всех столбах, на всех витринах висели объявления о том, что среди жителей вашего городка теперь есть злостная шайка мафиози." +
            "Кажется, теперь ваша жизнь превратиться в сущий кошмар. Нужно поспать и осознать все происходящее.",
            "Сегодня ваши близкие не выпустили вас из дома, ведь вы еще не были в курсе того, что в городе появилась мафия. Никто не знает, что будет дальше, и с трепетом в сердце засыпают."
        };

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            if (!request.Request.Command.Contains("начать игру"))
            {
                return Utils.CreateResponse(
                    $"Мне жаль, я не говорю на испанском. Номер комнаты: {context.LobbyId}." +
                    " Когда все игроки присоединятся, нажмите \"Начать игру!\".",
                    Utils.CreateButtonList("Начать игру!"));
            }

            var lobby = AliceMafiaController.GetLobbyById(context.LobbyId);
            if (lobby.PlayersCount < 3)
            {
                context.ChangeState(new HostStartGameState(context));
                return Utils.CreateResponse(
                    $"Для игры нужно минимум трое. Пока что присоединилось всего {lobby.PlayersCount}.",
                    Utils.CreateButtonList("Начать игру!"));
            }

            lobby.StartGame();
            context.ChangeState(new InGameState(context));

            return Utils.CreateResponse(gameStartTexts[rnd.Next(0, gameStartTexts.Count - 1)], Utils.CreateButtonList("Далее"));
        }
    }
}