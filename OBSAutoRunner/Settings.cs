using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSAutoRunner
{
    public class Settings
    {
        public string corePath { get; set; }
        public string startScript { get; set; }
        public string closeScript { get; set; }
        public Profile[] profiles { get; set; }
    }
}
