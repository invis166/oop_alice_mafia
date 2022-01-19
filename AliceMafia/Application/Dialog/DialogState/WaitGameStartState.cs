using System;
using System.Collections.Generic;
using AliceMafia.Application.Dialog;
using AliceMafia.Controllers;

namespace AliceMafia.Application
{
    public class WaitGameStartState : DialogStateBase
    {
        public WaitGameStartState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            var command = request.Request.Command;
            if (!command.Contains("начать игру"))
            {
                context.ChangeState(new WaitGameStartState(context));
                return Utils.CreateResponse(
                    "Я бы хотела понять вас, но я всего лишь студенческий проект. Ожидайте начала!",
                    Utils.CreateButtonList("Начать игру!"));
            }

            if (AliceMafiaController.GetLobbyById(context.LobbyId).GameStarted)
            {
                context.ChangeState(new InGameState(context));
                Random rnd = new Random();
                List<string> gameStartTexts = new List<string>
                {
                    "В мирный и ничем не примечательный городок пришла беда: жители узнали, что теперь среди них живет мафия, " +
                    "готовая уничтожить все и вся на своем пути. Вы не хотите засыпать, ведь понимаете, что эта ночь станет роковой, но все же делаете это.",
                    "Пока вы возвращались с работы, вы заметили, что на всех столбах, на всех витринах висели объявления о том, что среди жителей вашего городка теперь есть злостная шайка мафиози." +
                    "Кажется, теперь ваша жизнь превратиться в сущий кошмар. Нужно поспать и осознать все происходящее.",
                    "Сегодня ваши близкие не выпустили вас из дома, ведь вы еще не были в курсе того, что в городе появилась мафия. Никто не знает, что будет дальше, и с трепетом в сердце засыпают."
                };
                return Utils.CreateResponse(gameStartTexts[rnd.Next(0, gameStartTexts.Count - 1)], Utils.CreateButtonList("Далее"));
            }

            return Utils.CreateResponse(
                "Куда вы торопитесь? Игра еще не началась. Пока соберитесь с духом!",
                Utils.CreateButtonList("Начать игру!"));
        }
    }
}