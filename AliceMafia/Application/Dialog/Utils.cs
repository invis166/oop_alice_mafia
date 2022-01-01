using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AliceMafia.Setting;

namespace AliceMafia.Application.Dialog
{
    public static class Utils
    {
        public static AliceResponse CreateResponse(string responseText, List<ButtonModel> buttons = null)
        {
            return new AliceResponse
            {
                Response = new ResponseModel
                {
                    Text = responseText,
                    Buttons = buttons,
                },
            };
        }
        
        public static List<ButtonModel> CreateButtonList(params string[] buttonTexts) =>
            buttonTexts.Select(buttonText => new ButtonModel {Title = buttonText}).ToList();
        
        public static Dictionary<IGameSetting, string> FillSettings()
        {
            var settingsConstructorInfos = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterface("IGameSetting") != null && t.Namespace.Contains("AliceMafia.Setting"))
                .Select(t  => t.GetConstructor(Type.EmptyTypes))
                .ToList();
            

            var result = new Dictionary<IGameSetting, string>();
            foreach (var settingsConstructor in settingsConstructorInfos)
            {
                var setting = (IGameSetting)settingsConstructor.Invoke(new object[0]);
                result[setting] = setting.SettingName;
            }

            return result;
        }
        
        public static Dictionary<string, IGameSetting> FillInverseSettings()
        {
            var settingsConstructorInfos = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterface("IGameSetting") != null && t.Namespace.Contains("AliceMafia.Setting"))
                .Select(t  => t.GetConstructor(Type.EmptyTypes))
                .ToList();
            

            var result = new Dictionary<string, IGameSetting>();
            foreach (var settingsConstructor in settingsConstructorInfos)
            {
                var setting = (IGameSetting)settingsConstructor.Invoke(new object[0]);
                result[setting.SettingName.ToLower()] = setting;
            }

            return result;
        }
    }
}