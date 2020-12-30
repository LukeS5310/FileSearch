using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSearch.Search
{
   public class SearchFactory
    {
        private DirectoryInfo RootDirectory { get; }
        public SearchFactory(string RootDirectoryPath)
        {
            this.RootDirectory = new DirectoryInfo(RootDirectoryPath);
        }
        
        public IEnumerable<FoundObject> PerformSearch(string FileMask)
        {
            var outList = new List<FoundObject>();
            foreach (FileInfo file in RootDirectory.GetFiles())
            {
                outList.Add(new FoundObject(file.FullName));
            }
            foreach (DirectoryInfo dir in RootDirectory.GetDirectories())
            {
                outList.Add(new FoundDirectory(dir.FullName,FileMask));
            }
            return outList;
        }
    }
}
