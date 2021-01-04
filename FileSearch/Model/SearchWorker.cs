using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace FileSearch.Model
{
    public class SearchWorkerController
    {
        public ManualResetEvent Pauser { get; set; }
        public bool IsAborted { get; set; }
        public SearchWorkerController()
        {
            this.Pauser = new ManualResetEvent(true);
        }

    }
    public class SearchWorker
    {
        public event EventHandler<EventArgs> WorkerCompleted;
        public bool IsBusy { get; set; }
        public bool IsPaused { get; set; }
        private SearchWorkerController Controller = new SearchWorkerController();
        private Thread WorkerInProgress { get; set; }
        public FoundObjectFactory Searcher {get;}
        public SearchWorker(string rootFolder,Regex fileMask, ObservableCollection<FoundObject> externalTree)
        {
            Searcher = new FoundObjectFactory(rootFolder,Controller);
            
            WorkerInProgress = new Thread(() =>
            {
                IsBusy = true;
                Searcher.PerformSearchRealTime(fileMask, externalTree);
                OnWorkerCompleted(new EventArgs());
            });
        }
        public void DoWork()
        {
            WorkerInProgress.Start();
        }
        public void PauseWork()
        {
            IsPaused = true;
            Controller.Pauser.Reset();
        }
        public void ResumeWork()
        {
            IsPaused = false;
            Controller.Pauser.Set();
        }
        public void CancelWork()
        {
            Controller.IsAborted = true;
        }
        protected virtual void OnWorkerCompleted(EventArgs e)
        {
            IsBusy = false;
            WorkerCompleted?.Invoke(this,e);
        }
    }
}
