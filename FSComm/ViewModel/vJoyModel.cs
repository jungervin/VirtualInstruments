using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSComm.ViewModel
{
    public class vJoyButtonModel
    {
        public uint id { get; set; } = 0;
        public uint btn { get; set; } = 0;
        public bool value { get; set; } = false;


    }
    public class vJoyPushButtonModel
    {
        public uint id { get; set; } = 0;

        public uint btn { get; set; } = 0;

        public int sleep { get; set; } = 100;

    }
}
