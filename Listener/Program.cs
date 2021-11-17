using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using FolderWatcher;

namespace Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo _directory = Helper.GetFolder();
            int _waitingIntervalInSeconds = Helper.SetInterval();
            List<string> documentNames = new List<string>() { "Паспорт.jpg", "Заявление.txt", "Фото.jpg" };
            using ( DocumentsReceiver receiver = new DocumentsReceiver(documentNames) )
            {
                receiver.TimedOut += OnTimeoutHandler;
                receiver.DocumentsReady += OnDocumentsReadyHandler;
                receiver.Start(_directory, _waitingIntervalInSeconds);
                while ( !timeIsOut && !documentsIsReady && !successFullLoading )
                {

                }
            }
            Console.WriteLine("\nНажмите любую клавишу.");
            Console.ReadLine();
        }

        private static int _waitingIntervalInSeconds;
        private static bool documentsIsReady = false;
        private static bool timeIsOut = false;
        private static bool successFullLoading = false;

        private static void OnTimeoutHandler(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine($"Время ожидания истекло. Документы не загружены. Повторите попытку.");
            timeIsOut = true;
        }
        private static void OnDocumentsReadyHandler(object sender, EventArgs e)
        {
            successFullLoading = SomeLoadingDocumenOperation();
            if ( !successFullLoading )
            {
                Console.WriteLine("Что-то пошло не так. Повторите попытку позже.");
                return;
            }
            Console.WriteLine(">>> Документы приняты в обработку!");
            documentsIsReady = true;
        }

        private static  bool SomeLoadingDocumenOperation()
        {
            bool success = true;
            return success;
        }
    }
}
