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
        public IEnumerable<FileInfo> SearchFiles() //TODO: fix bug with searching from drive root
        {
            var outList = new List<FileInfo>() ;
         
                foreach (FileInfo fileEntry in new DirectoryInfo(RootPath).EnumerateFiles(FileMask,new EnumerationOptions() {IgnoreInaccessible = true }))
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
           
           
           //      outList = new DirectoryInfo(RootPath).GetFiles(FileMask, SearchOption.TopDirectoryOnly).ToList();
                foreach (DirectoryInfo dirEntry in new DirectoryInfo(RootPath).GetDirectories())
                {
                    SearchFiles(outList, dirEntry.FullName);
                }
           
            return outList;
        }
        public void SearchFiles(List<FileInfo> passList,string folderPath)
        {
            
            {
                foreach (FileInfo fileEntry in new DirectoryInfo(folderPath).EnumerateFiles(FileMask, new EnumerationOptions() { IgnoreInaccessible = true }))
                {

                    try
                    {
                        passList.Add(fileEntry);
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                }
                foreach (DirectoryInfo dirEntry in new DirectoryInfo(folderPath).EnumerateDirectories("*", new EnumerationOptions() { IgnoreInaccessible = true }))
                {
                    try
                    {
                        SearchFiles(passList, dirEntry.FullName);
                    }
                    catch (Exception)
                    {

                        continue;
                    }
                   
                }
            }
           
            

        }

    }
}
