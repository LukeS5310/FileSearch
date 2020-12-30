using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearch.Search
{
    class FoundDirectory : FoundObject
    {
        public IEnumerable<FoundObject> ChildrenList { get; }
        public FoundDirectory(string path, string fileMask) : base(path)
        {
            ChildrenList = new SearchFactory(path).PerformSearch(fileMask);

        }
    }
}
