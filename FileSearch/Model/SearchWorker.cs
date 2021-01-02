using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;

namespace FileSearch.Model
{
    class SearchWorker
    {
        private ManualResetEvent Pauser = new ManualResetEvent(true);
       
        private ObservableCollection<FoundObject> ExternalTree {get;set;}
        private Thread WorkerInProgress { get; set; }
        public SearchWorker(string rootFolder, string fileMask, ObservableCollection<FoundObject> externalTree)
        {
            this.WorkerInProgress = new Thread(() =>
            {
                var Searcher = new FoundObjectFactory(rootFolder);
                Searcher.PerformSearchRealTime(fileMask, externalTree);
            });
        }
        public void DoWork()
        {
            this.WorkerInProgress.Start();
        }
        public void PauseWork()
        {
            Pauser.Reset();
        }
        public void ResumeWork()
        {
            Pauser.Set();
        }
        public void CancelWork()
        {
            //this.WorkerInProgress.;
        }

    }
}
