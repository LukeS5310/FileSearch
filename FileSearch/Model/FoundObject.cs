using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearch.Model

{
    public class FoundObject
    {
        public string Name { get; }
        public FoundObject(string name)
        {
           
            this.Name = name ;
            
        }
    }
}
