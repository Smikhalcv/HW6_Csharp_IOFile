using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace DropNumbers
{
    class Program
    {
        static private int dropNumber = 0;
        private static string pathToFileForRead = "";
        private static string pathToFileForWrite = "";

        /// <summary>
        /// Получает путь к файлу и возвращает его. Если путь не указан, будет искать файл Number.txt в каталоге программы.
        /// </summary>
        /// <returns>Путь к файлу string</returns>
        /// <param name="expansion">Расширение файла, по умолчанию .txt</param>
        private static string GetPathToFile(string expansion = ".txt")
        {
            Console.WriteLine("Укажите путь к файлу:");
            string path = Console.ReadLine();
            if (path == "")
            {
                path = "Number.txt";
            }
            if (path.EndsWith(expansion) == false)
            {
                path += expansion;
            }
            return path;
        }

        /// <summary>
        /// Считывает число из файла, если в файле записано число удовлетворяющее условиям Int32 перезаписывает dropNumber
        /// </summary>
        private static void GetNumberFromFile()
        {
            try
            {
                using (StreamReader readNumber = new StreamReader(pathToFileForRead))
                {
                    dropNumber = Convert.ToInt32(readNumber.ReadLine());
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Файл не содержит данных подходящих под описание целого цисла!");
            }
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Запись файла");
            Console.ResetColor();
            pathToFileForWrite = GetPathToFile(); // получаем путь к файлу для записи
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Запись начата");
            Console.ResetColor();
            WriteStringInFile("Время начала записи - " + DateTime.Now.ToString() + "\n", pathToFileForWrite);
            string stringForWrite = $"Группа {incCountGroups}\n"; 
            for (int i = 1; i <= dropNumber; i++)
            {
                if (i < Math.Pow(2, incCountGroups))
                {
                    stringForWrite += $"{i} ";
                }
                else
                {
                    WriteStringInFile(stringForWrite, pathToFileForWrite); // записывает строку в файл
                    incCountGroups++;
                    stringForWrite = $"\nГруппа {incCountGroups}\n{i} "; // обновняет строку записи для дальнейшей работы                
                }
            }
            WriteStringInFile(stringForWrite, pathToFileForWrite); // записывает последнюю строку
            WriteStringInFile("\nВремя завершния записи - " + DateTime.Now.ToString(), pathToFileForWrite);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Запись закончена");
            Console.ResetColor();
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

        /// <summary>
        /// Дозаписывает данные в файл
        /// </summary>
        /// <param name="dataForWrite">Данные для записи</param>
        /// <param name="pathToFileForWrite">Путь к записываемому файлу</param>
        private static void WriteStringInFile(string dataForWrite, string pathToFileForWrite)
        {
            using (StreamWriter writeInFile = new StreamWriter(pathToFileForWrite, true))
            {
                writeInFile.Write(dataForWrite, true);
            }
        }

        /// <summary>
        /// Архивирует файл
        /// </summary>
        /// <param name="pathToFileSource">Путь к файлу для архивации</param>
        /// <param name="pathToFileCompress">Путь к архиву</param>
        private static void ArchiveFile(string pathToFileSource, string pathToFileCompress)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Архивация файла!");
            Console.ResetColor();
            using (FileStream fileSource = new FileStream(pathToFileSource, FileMode.Open))
            {
                using (FileStream fileCompress = File.Create(pathToFileCompress))
                {
                    using (GZipStream archive = new GZipStream(fileCompress, CompressionMode.Compress))
                    {
                        fileSource.CopyTo(archive);
                        Console.WriteLine($"Размер файла {pathToFileForWrite} до архивации составлял - {fileSource.Length} и стал - {fileCompress.Length}");
                    }
                }
            } 
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Разбиваем числа на группы взаимно не делимых чисел.");
            Console.ResetColor();
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
                GetNumberFromFile();
                if (CheckDropNumber())
                {
                    Console.WriteLine("Вывести в консоль количество групп или только записать в файл? (да/нет)");
                    string answer = Console.ReadLine();
                    if (answer.ToLower().StartsWith('д'))
                    {
                        Console.WriteLine(GetNumberGroups());
                        DropNumber();
                    }
                    else
                    {
                        DropNumber();
                    }                   
                }
                Console.WriteLine("Желаете заархивировать файл? (да/нет)");
                string flagArchive = Console.ReadLine();
                if (flagArchive.ToLower().StartsWith('д'))
                {
                    ArchiveFile(pathToFileForWrite, GetPathToFile(".zip"));
                }
                if (StartAgain()) { continue; } else { break; }
            }
        }

    }
}
