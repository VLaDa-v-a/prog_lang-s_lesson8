using System;
using System.Reflection.Emit;

namespace part01
{
    /// <summary>
    /// Простые арифметические операции, собранные на лету через ILGenerator.
    /// </summary>
    public static class IlMathFactory
    {
        public static Func<int, int> CreateHalveInt()
        {
            var method = new DynamicMethod("HalveInt", typeof(int), new[] { typeof(int) });
            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);   // исходное значение
            il.Emit(OpCodes.Ldc_I4_2);  // константа 2
            il.Emit(OpCodes.Div);       // value / 2
            il.Emit(OpCodes.Ret);

            return (Func<int, int>)method.CreateDelegate(typeof(Func<int, int>));
        }

        public static Func<int, int> CreateSubtractConst(int subtract)
        {
            var method = new DynamicMethod("SubConst" + subtract, typeof(int), new[] { typeof(int) });
            ILGenerator il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);          // исходное значение
            il.Emit(OpCodes.Ldc_I4, subtract); // константа
            il.Emit(OpCodes.Sub);              // value - subtract
            il.Emit(OpCodes.Ret);

            return (Func<int, int>)method.CreateDelegate(typeof(Func<int, int>));
        }
    }
}

