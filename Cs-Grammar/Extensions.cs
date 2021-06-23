using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    /// <summary>
    /// расширяющий класс для типа string
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// проверяет находится ли слово в нижнем регистре
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLower(this string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// проверяет состоит ли слово только из пустых символов
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotEpsilon(this string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != 'ε')
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// функция определяет изменилось ли количество терминалов
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CountTerminal(this string value)
        {
            int count = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (Char.IsLower(value[i]))
                {
                    count++;
                }
            }
            return count == 0 ? false : true;
        }
    }
}
