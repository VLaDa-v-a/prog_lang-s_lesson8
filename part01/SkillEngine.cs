using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace part01
{
    public class SkillEngine
    {
        // Реестр найденных навыков, сгруппированный по этапам срабатывания
        private Dictionary<TriggerType, List<(MethodInfo Method, object Target, int Priority)>> _pipeline;


        public SkillEngine()
        {
            _pipeline = new Dictionary<TriggerType, List<(MethodInfo, object, int)>>();

            foreach (TriggerType trigger in Enum.GetValues(typeof(TriggerType)))
            {
                _pipeline[trigger] = new List<(MethodInfo, object, int)>();
            }
        }

        /// Сканирует переданную сборку на наличие методов, помеченных CombatSkillAttribute.
        public void RegisterAssembly(Assembly assembly)
        {
            // Получаем все типы из сборки с атрибутом GameAttribute
            var types = assembly.GetTypes().Where(t => t.GetCustomAttribute<GameAttribute>() != null);
            foreach (Type type in types)
            {
                // Создаем экземпляр класса
                if (Activator.CreateInstance(type) is not object instance)
                {
                    Console.WriteLine($"[Warning] Unable to create instance of {type.FullName}");
                    continue;
                }

                // Итерируемся по всем публичным методам типа
                foreach (MethodInfo method in type.GetMethods())
                {
                    // Проверяем наличие целевого атрибута
                    var attribute = method.GetCustomAttribute<CombatSkillAttribute>();

                    if (attribute != null)
                    {
                        // Валидация сигнатуры метода (должен принимать BattleContext)
                        var parameters = method.GetParameters();
                        if (parameters.Length == 1 && parameters[0].ParameterType == typeof(BattleContext))
                        {
                            // Регистрация в пайплайне
                            Console.WriteLine($"Registered logic: {attribute.Name} (Trigger: {attribute.Trigger})");

                            
                            _pipeline[attribute.Trigger].Add((method, instance, attribute.Priority));
                            

                            Console.WriteLine($"[JIT] Compiled & Optimized: {attribute.Name}");
                        }
                    }
                }
            }

            // Сортировка пайплайнов по приоритету
            foreach (var key in _pipeline.Keys.ToList())
            {
                _pipeline[key] = _pipeline[key].OrderByDescending(x => x.Priority).ToList();
            }
        }


        /// Запуск всех зарегистрированных навыков для конкретного этапа.
        public void ExecutePipeline(TriggerType trigger, BattleContext context)
        {
            if (!_pipeline.ContainsKey(trigger)) return;

            foreach (var (method, target, priority) in _pipeline[trigger])
            {
                // Динамический вызов
                method.Invoke(target, [context]);
            }

        }
    }

}
