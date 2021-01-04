using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace FileSearch.Model

{
    public class FoundObjectFactoryEventArgs : EventArgs
    {
        public int FilesTotal { get; set; }
        public int FilesFound { get; set; }  
        public string CurrentFolder { get; set; }
        public FoundObjectFactoryEventArgs(int filesTotal, int filesFound, string currentFolder)
        {
            this.FilesTotal = filesTotal;
            this.FilesFound = filesFound;
            this.CurrentFolder = currentFolder;
        }
    }
   public class FoundObjectFactory
    {
        public event EventHandler<FoundObjectFactoryEventArgs> LogPass;
        private DirectoryInfo RootDirectory { get; }
        private SearchWorkerController Controller { get; }
        public FoundObjectFactory(string RootDirectoryPath, SearchWorkerController controller = null)
        {
            this.Controller = controller;
            this.RootDirectory = new DirectoryInfo(RootDirectoryPath);
        }
        
        public void PerformSearchRealTime(Regex fileMask, ObservableCollection<FoundObject> externalTree )
        {
            var rootDir = new FoundDirectory(RootDirectory.Name);
            int filesFound = 0;
            ProperlyUpdateNode(externalTree, rootDir);
            FoundDirectory parentDir;          
            foreach (FileInfo fileEntry in RootDirectory.EnumerateFiles("*", new EnumerationOptions() {IgnoreInaccessible = true, RecurseSubdirectories = true}))
            {
                try
                {
                    if (!fileMask.IsMatch(fileEntry.Name)) continue;
                    parentDir = rootDir;
                    bool isReachedRoot = false;
                    foreach (string pathItem in fileEntry.FullName.Split('\\'))
                    {
                        if (pathItem == RootDirectory.Name.Replace("\\", string.Empty))
                        {
                            isReachedRoot = true;
                            continue;
                        }
                        if (isReachedRoot == false) continue;
                        if (fileEntry.Name == pathItem)
                        {
                            ProperlyUpdateNode(parentDir.ChildrenList, new FoundFile(pathItem));
                           
                            continue;
                        }
                        var subDir = parentDir.ChildrenList.FirstOrDefault(x => x.Name.Equals(pathItem));
                        if (subDir == null)
                        {
                            subDir = new FoundDirectory(pathItem);
                            ProperlyUpdateNode(parentDir.ChildrenList, subDir);
                            
                        }
                        
                        parentDir = (FoundDirectory)subDir;    
                    }
                    filesFound++;
                    OnRaiseLogPass(new FoundObjectFactoryEventArgs(0, filesFound, rootDir.Name));
                    CheckPause();
                    if (Controller?.IsAborted == true) return;
                }
                catch (Exception)
                {

                    continue;
                }
            }

        }
        private void CheckPause()
        {
            Controller?.Pauser.WaitOne();
        }
        protected virtual void OnRaiseLogPass(FoundObjectFactoryEventArgs e)
        {
            LogPass?.Invoke(this, e);
        }
        private static void ProperlyUpdateNode(ObservableCollection<FoundObject> targetCollection,FoundObject targetItem)
        {
           
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    targetCollection.Add(targetItem);
                });
          
        }

    }
}
