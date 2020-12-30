using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearch.Search
{
    public class FoundObject
    {
        public string Name { get; }
        public string Path { get; }
        public FoundObject(string path)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
        }
    }
}
