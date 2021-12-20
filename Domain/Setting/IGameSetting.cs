using System;
using System.Collections.Generic;

namespace AliceMafia.Setting
{
    public interface IGameSetting
    {
        public IGeneralSetting GeneralMessages { get; }
        //[   ]
        public IRoleSetting Civilian { get; }
        public IRoleSetting Mafia { get; }
        public IRoleSetting Doctor { get; }
        public IRoleSetting Sheriff { get; }
        public IRoleSetting Courtesan { get; }

        private Dictionary<Type, IRoleSetting> воттакойсловарьделаем;//ostavlyaem

        public void set_up()
        {
            //рефлексией ищем всех наследников rolebase
            //и достаем для каждого роле базе айроллсеттинг
            
        }
    }
}