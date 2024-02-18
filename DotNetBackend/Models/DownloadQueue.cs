using System.Collections;
using System.Collections.Generic;

namespace DotNetBackend.Models
{
    public class DownloadQueue
    {
        public Queue Queue {  get; set; }
        
        public DownloadQueue()
        {
            Queue = new Queue();
        }
    }
}
