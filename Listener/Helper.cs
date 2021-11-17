using System;
using System.IO;
using System.Linq;

namespace Listener
{
    public class Helper
    {
        public static DirectoryInfo GetFolder()
        {
            string folderPath = "";
            string baseFolderPath = @"D:\Otus\HW7";

            DirectoryInfo folder = null;
            do
            {
                Console.WriteLine($"\nВведите путь папки для отслеживания, например:\n{baseFolderPath}");
                string inputValue = Console.ReadLine();
                if ( inputValue == "" ) folderPath = baseFolderPath;
                else folderPath = inputValue;
                folder = GerFolderOrNull(folderPath);

            } while ( folder == null );
            return folder;
        }
        private static int _minTime = 10;
        private static int _maxTime = 60;
        public static int SetInterval()
        {
            int waitingInterval = 0;
            do
            {
                Console.WriteLine($"\nВведите время ожидания от {_minTime} до {_maxTime} (сек), или нажмите Enter для установки значения по умолчанию.");
                string input = Console.ReadLine();
                waitingInterval = GetIntervalFromInputOrZero(input);
            } while ( waitingInterval == 0 );
            return waitingInterval;
        }

        public static int GetIntervalFromInputOrZero(string input)
        {
            int defaultWaitingInterval = 10;
            int waitingInterval = 0;
            if ( input == "" ) return defaultWaitingInterval;
            try
            {
                waitingInterval = int.Parse(input);
            }
            catch
            {
                return 0;
            }

            if ( _minTime <= waitingInterval && waitingInterval <= _maxTime )
            {
                return waitingInterval;
            }
            else
            {
                return 0;
            }
        }

        private static DirectoryInfo GerFolderOrNull(string folderPath)
        {
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            if ( !folder.Exists )
            {
                Console.WriteLine("\nУказанной папки не существует.\n");
                return null;
            }
            else
            {
                return folder;
            }
        }
    }

}
