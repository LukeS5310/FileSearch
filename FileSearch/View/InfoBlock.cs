using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearch.View
{
    //All relevant info about search process goes here
    public class InfoBlock
    {
       
        public int FilesFound { get; set; }
        public int FilesTotal { get; set; }
        public string CurrentDirectory { get; set; }
        private DateTime TimeStarted { get; set; }
        public InfoBlock()
        {
            this.TimeStarted = DateTime.Now;
        }

        public string GetTimePassed()
        {
            TimeSpan passed = DateTime.Now.Subtract(TimeStarted);
            return String.Format("{0}:{1}",passed.Minutes.ToString("D2"),passed.Seconds.ToString("D2"));
        }
        

    }
}
