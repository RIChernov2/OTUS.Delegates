using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FolderWatcher
{
    public class DocumentsReceiver: IDisposable
    {
        public DocumentsReceiver(List<string> documentNames)
        {
            _documentNames = documentNames;
        }

        private Timer _timer = new Timer();
        private FileSystemWatcher _watcher = new FileSystemWatcher();
        private DirectoryInfo _targetDirectory;

        public event EventHandler DocumentsReady;
        public event ElapsedEventHandler TimedOut;
        private List<string> _documentNames;

   

        public void Start(DirectoryInfo targetDirectory, int waitingIntervalInSeconds)
        {
            _targetDirectory = targetDirectory;
            _timer.Interval = waitingIntervalInSeconds * 1000;
            _timer.Elapsed += TimedOut;

            _watcher.Path = targetDirectory.FullName;
            _watcher.EnableRaisingEvents = true;
            _watcher.Changed += OnChangeHandler;
            _watcher.Created += OnChangeHandler;
            _watcher.Deleted += OnChangeHandler;
            _watcher.Renamed += OnChangeHandler;

            _timer.Start();
            string docList = GetDocumentsList();
            Console.WriteLine($"\nУ вас есть {waitingIntervalInSeconds} секунд на загрузку документов: {docList}в папку:\n{targetDirectory.FullName}\n");
            OnChangeHandler(this, null);
        }

        private void OnChangeHandler(object sender, FileSystemEventArgs e)
        {
            var filesNames = _targetDirectory.GetFiles().Select(f => f.Name).ToList();
            bool filesIsReady = _documentNames.All(x => filesNames.Contains(x));
            if( filesIsReady )
            {
                if(filesNames.Count() == _documentNames.Count)
                {
                    DocumentsReady.Invoke(this, null);
                }
            }
        }

        private string GetDocumentsList()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("\n");
            int counter = 1;
            foreach ( string docName in _documentNames )
            {
                builder.Append($"{counter}. \"{docName}\"\n");
                counter++;
            }
            builder.Append("\n");
            return builder.ToString();
        }

        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if ( !_disposed )
            {
                if ( disposing )
                {
                    _timer.Dispose();
                    _watcher.Dispose();
                }
                _disposed = true;
            }
        }
        ~DocumentsReceiver()
        {
            Dispose(false);
        }
    }
}
