using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;

namespace FileSearch.Model
{
    public class SearchObjects
    { //look for all occurences of required file
        private string FileMask { get; }
        private string RootPath { get; }
       public SearchObjects(string rootPath, string fileMask)
        {
            this.FileMask = fileMask;
            this.RootPath = rootPath;
        }
        public IEnumerable<FileInfo> SearchFiles() 
        {
            var outList = new List<FileInfo>() ;
         
                foreach (FileInfo fileEntry in new DirectoryInfo(RootPath).EnumerateFiles(FileMask,new EnumerationOptions() {IgnoreInaccessible = true, RecurseSubdirectories = true }))
                {
                    try
                    {
                        outList.Add(fileEntry);
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }

            return outList;
        }
    }
}
