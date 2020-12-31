using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FileSearch.Model
{
    class FoundDirectory : FoundObject
    {
        public ObservableCollection<FoundObject> ChildrenList { get; set; }
        public FoundDirectory(string name) : base(name)
        {
            ChildrenList = new ObservableCollection<FoundObject>();

        }
    }
}
