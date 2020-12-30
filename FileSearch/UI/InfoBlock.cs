using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearch.UI
{
    //All relevant info about search process goes here
    public class InfoBlock
    {
        public int FilesFound { get; set; }
        public int FilesTotal { get; set; }
        public string CurrentDirectory { get; set; }
        

    }
}
