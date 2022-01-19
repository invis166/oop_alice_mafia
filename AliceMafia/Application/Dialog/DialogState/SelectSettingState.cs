using System.Linq;
using AliceMafia.Application.Dialog;

namespace AliceMafia.Application
{
    public class SelectSettingState : DialogStateBase
    {
        public SelectSettingState(UserContextBase context) : base(context)
        {
        }

        public override AliceResponse HandleUserRequest(AliceRequest request)
        {
            var settings = Utils.FillSettings();
            var buttons = Utils.CreateButtonList(settings.Keys.Select(key => settings[key]).ToArray());

            context.ChangeState(new CreateLobbyState(context));
            
            return Utils.CreateResponse(
                "Пожалуйста, выберите сеттинг для игры", 
                buttons);
        }
    }
}