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
        public IEnumerable<FileInfo> SearchFiles() //TODO make search recursive thus allowing to skip protected folders 
        {
            try
            {
                return new DirectoryInfo(RootPath).GetFiles(FileMask, SearchOption.AllDirectories);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

    }
}
