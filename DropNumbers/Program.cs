using System;
using System.Collections.Generic;
using System.IO;

namespace DropNumbers
{
    class Program
    {
        static private int dropNumber;
        private static string pathToFileForRead = "";
        private static string pathToFileForWrite = "";

        /// <summary>
        /// Получает путь к файлу и возвращает его
        /// </summary>
        /// <returns>Путь к файлу string</returns>
        private static string GetPathToFile()
        {
            Console.WriteLine("Укажите путь к файлу:");
            string path = Console.ReadLine();
            if (path == "")
            {
                path = "Number.txt";
            }
            if (path.EndsWith(".txt") == false)
            {
                path += ".txt";
            }
            return path;
        }

        /// <summary>
        /// Считывает число из файла, если в файле записано число удовлетворяющее условиям Int32 перезаписывает dropNumber
        /// </summary>
        private static void GetNumberFromFile()
        {
            int number = 0;
            try
            {
                using (StreamReader readNumber = new StreamReader(pathToFileForRead))
                {
                    number = Convert.ToInt32(readNumber.ReadLine());
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Файл не содержит данных подходящих под описание целого цисла!");
            }
            dropNumber = number;
        }

        /// <summary>
        /// Проверяет входит ли число в диапозон от 1 до 1_000_000_000
        /// </summary>
        static private bool CheckDropNumber()
        {
            if (dropNumber >= 1 && dropNumber <= 1_000_000_000)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Определяет количество групп при разбивании числа путём определения логарифма по основанию 2
        /// </summary>
        static private int GetNumberGroups()
        {
            return Convert.ToInt32(Math.Floor(Math.Log2(dropNumber))) + 1;
        }


        /// <summary>
        /// Разбивает число на группы, в которых каждое число не делится друг на друга через квадраты
        /// </summary>
        static void DropNumber()
        {
            int incCountGroups = 1;
            Console.WriteLine("ЗАПИСЬ ФАЙЛА");
            pathToFileForWrite = GetPathToFile(); // получаем путь к файлу для записи
            Console.WriteLine($"Путь к файлу для записи - {pathToFileForWrite}");
            string stringForWrite = $"Группа {incCountGroups}\n"; 
            Console.Write($"Группа {incCountGroups}\n");
            for (int i = 1; i <= dropNumber; i++)
            {
                if (i < Math.Pow(2, incCountGroups))
                {
                    stringForWrite += $"{i} ";
                    Console.Write($"{i} ");
                }
                else
                {
                    WriteStringInFile(stringForWrite, pathToFileForWrite);
                    incCountGroups++;
                    stringForWrite = $"\nГруппа {incCountGroups}\n{i} ";
                    Console.Write($"\nГруппа {incCountGroups}");
                    Console.Write($"\n{i} ");
                        
                }
            }
            WriteStringInFile(stringForWrite, pathToFileForWrite);
        }

        /// <summary>
        /// Спрашивает перезапускать ли основной алгоритм.
        /// </summary>
        /// <returns>true если да</returns>
        private static bool StartAgain()
        {
            Console.WriteLine("Желаете перезапустить? (да/нет)");
            string answer = Console.ReadLine();
            if (answer.ToLower().StartsWith('д'))
            {
                return true;
            }
            return false;
        }

        private static void WriteStringInFile(string dataForWrite, string pathToFileForWrite)
        {
            Console.WriteLine("Началась запись");
            using (StreamWriter writeInFile = new StreamWriter(pathToFileForWrite, true))
            {
                writeInFile.Write(dataForWrite, true);
            }
        }

        static void Main(string[] args)
        {
            bool flag = true;
            while (flag)
            {
                pathToFileForRead = GetPathToFile();
                FileInfo pathToFile = new FileInfo(pathToFileForRead);
                if (pathToFile.Exists == false) // проверяет существует ли указанный файл
                {
                    Console.WriteLine("Ошибка чтения файла, проверьте правильно ли указан путь!");
                    if (StartAgain())
                    {
                        continue;
                    }
                    break;
                }

                Console.WriteLine("файл проверен");

                GetNumberFromFile(); // получаем число из файла

                Console.WriteLine("получили число");

                Console.WriteLine(GetNumberGroups());
                if (CheckDropNumber())
                {
                    Console.WriteLine("Вывести в консоль количество групп или только записать в файл? (да/нет)");
                    string answer = Console.ReadLine();
                    if (answer.ToLower().StartsWith('д'))
                    {
                        Console.WriteLine(GetNumberGroups());
                        DropNumber();
                        break;
                    }
                    else
                    {
                        DropNumber();
                        break;
                    }
                    
                }
            }

            //Console.WriteLine(GetNumberFromFile());

            //WriteStringInFile("привет", "Hello.txt");
        }

    }
}
