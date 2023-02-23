using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSComm.Utility
{
    public class JsonModels
    {
        public class CommandModel
        {
            public string command { get; set; }
            public object data { get; set; }
        }
        public class DisplaySizeModel
        {
            public DisplaySizeModel()
            {

            }

            public int left { get; set; }
            public int top { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class MarginModel
        {
            public MarginModel()
            {

            }

            public int left { get; set; }
            public int top { get; set; }
            public int right { get; set; }
            public int bottom { get; set; }
        }


        public class WindowStateModel
        {
            public WindowState state { get; set; }
        }

        public class TopmostModel
        {
            public bool topmost { get; set; }
        }
    }
}
