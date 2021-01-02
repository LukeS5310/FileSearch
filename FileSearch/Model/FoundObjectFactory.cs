using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections.ObjectModel;

namespace FileSearch.Model

{
   public class FoundObjectFactory
    {
        private DirectoryInfo RootDirectory { get; }
        private ManualResetEvent Pauser;
        public FoundObjectFactory(string RootDirectoryPath, ManualResetEvent pauser = null)
        {
            this.Pauser = pauser;
            this.RootDirectory = new DirectoryInfo(RootDirectoryPath);
        }
        
        public void PerformSearchRealTime(string fileMask, ObservableCollection<FoundObject> externalTree )
        {
            var rootDir = new FoundDirectory(RootDirectory.Name);
            ProperlyUpdateNode(externalTree, rootDir);
            System.Threading.Thread.Sleep(1000);
            
            FoundDirectory parentDir;
            
            foreach (FileInfo fileEntry in RootDirectory.EnumerateFiles(fileMask, new EnumerationOptions() {IgnoreInaccessible = true, RecurseSubdirectories = true}))
            {
                try
                {
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
                        CheckPause();
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }

        }
        private void CheckPause()
        {
            Pauser?.WaitOne();
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
