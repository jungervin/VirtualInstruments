using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSComm.Utility
{
    internal class HtmlFolders : List<string>
    {
        public HtmlFolders()
        {
            
        }

        public void FindDirectories(string folder)
        {
            if (Directory.Exists(folder))
            {
                foreach (string dir in Directory.GetDirectories(folder))
                {

                    if (dir.EndsWith("\\html_ui"))
                    {
                        this.Add(dir);
                    }
                    else
                    {
                        this.FindDirectories(dir);
                    }
                }
            }
            else
            {
                MainWindow.Instance.MainWindowViewModel.LogViewModel.Add($"Could not find a folder: {folder}");
            }
        }
    }
}
