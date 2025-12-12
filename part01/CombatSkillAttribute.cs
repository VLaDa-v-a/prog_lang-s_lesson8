using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace part01
{
    //Этот атрибут нужен для упрощения поиска классов, реализующих логику игры
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GameAttribute : Attribute    {    }

    /// Атрибут-маркер, превращающий обычный метод в исполняемую единицу бизнес-логики.
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CombatSkillAttribute : Attribute
    {
        ///Уникальный идентификатор навыка в системе.
        public string Name { get; }

        ///Точка внедрения в пайплайн обработки.
        public TriggerType Trigger { get; }

        ///Приоритет выполнения (выше значение -> раньше выполнение).
        public int Priority { get; }

        public CombatSkillAttribute(string name, TriggerType trigger, int priority = 1)
        {
            Name = name;
            Trigger = trigger;
            Priority = priority;
        }
    }

}
