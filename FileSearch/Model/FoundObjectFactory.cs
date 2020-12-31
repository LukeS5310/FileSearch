using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace FileSearch.Model

{
   public class FoundObjectFactory
    {
        private DirectoryInfo RootDirectory { get; }
        public FoundObjectFactory(string RootDirectoryPath)
        {
            this.RootDirectory = new DirectoryInfo(RootDirectoryPath);
        }
        
        public ObservableCollection<FoundObject> PerformSearch(string FileMask)
        {
            var outList = new ObservableCollection<FoundObject>();
            var rootDir = new FoundDirectory(RootDirectory.Name);
            outList.Add(rootDir);
            var parentDir = rootDir;
            
            foreach (FileInfo file in new SearchObjects(RootDirectory.FullName,FileMask).SearchFiles())
            {
                parentDir = rootDir;
                bool isReachedRoot = false;
                foreach (string pathItem in file.FullName.Split('\\'))
                {
                    if (pathItem == RootDirectory.Name)
                    {
                        isReachedRoot = true;
                        continue;
                    }
                    if (isReachedRoot == false) continue;
                    if (file.Name == pathItem)
                    {
                        parentDir.ChildrenList.Add(new FoundFile(pathItem));
                        continue;
                    }
                    var subDir = parentDir.ChildrenList.FirstOrDefault(x => x.Name.Equals(pathItem));
                    if (subDir == null)
                    {
                        subDir = new FoundDirectory(pathItem);
                        parentDir.ChildrenList.Add(subDir);
                    }
                    parentDir = (FoundDirectory)subDir;
                }
                
            }
            return outList;
        }
    }
}
