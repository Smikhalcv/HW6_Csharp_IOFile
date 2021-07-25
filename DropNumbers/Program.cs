using System;
using System.Collections.Generic;

namespace DropNumbers
{
    class Program
    {
        static private int dropNumber;

        /// <summary>
        /// Проверяет входит ли число в диапозон от 1 до 1_000_000_000
        /// </summary>
        /// <param name="numberForDrop">Разбиваемое число</param>
        static private void CheckDropNumber(int numberForDrop)
        {

        }

        /// <summary>
        /// Определяет количество групп при разбивании числа путём определения логарифма по основанию 2
        /// </summary>
        /// <param name="numberForDrop">Разбиваемое число</param>
        static private int GetNumberGroups(int numberForDrop)
        {
            return Convert.ToInt32(Math.Floor(Math.Log2(numberForDrop))) + 1;
        }


        /// <summary>
        /// Разбивает число на группы, в которых каждое число не делится друг на друга через квадраты
        /// </summary>
        /// <param name="numberForDrop">Разбиваемое число</param>
        static void DropNumber(int numberForDrop)
        {
            //int countGroups = GetNumberGroups(numberForDrop);
            int incCountGroups = 1;
            Console.Write($"Группа {incCountGroups}\n");
            for (int i = 1; i <= numberForDrop; i++)
            {
                if (i < Math.Pow(2, incCountGroups))
                {
                    Console.Write($"{i} ");
                }
                else
                {
                    incCountGroups++;
                    Console.Write($"\nГруппа {incCountGroups}");
                    Console.Write($"\n{i} ");
                        
                }

            }

        }

        static void Main(string[] args)
        {
            dropNumber = 1_000;

            Console.WriteLine(GetNumberGroups(dropNumber));
            DropNumber(dropNumber);
        }

    }
}
