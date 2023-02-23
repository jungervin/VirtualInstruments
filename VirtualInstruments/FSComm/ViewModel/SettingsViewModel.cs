using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSComm.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(MainWindowViewModel main_window_view_model)
        {
            this.fMainWindowViewModel = main_window_view_model;

            this.HTTPServerRoot = this.AppSettings.HTTPServerDocumentRootPath;
            this.HTTPServerPort = this.AppSettings.HTTPServerPort;

            this.DatabaseFile = this.AppSettings.DatabaseFile;

            this.FSFolder = this.AppSettings.FSFolder;
            this.FSContentsFolder = this.AppSettings.FSContentsFolder;
            string sep = "";
//            this.WebSearchFolders = "";

            //foreach (string l in this.AppSettings.WEBSearchFolders)
            //{
            //    this.WebSearchFolders += sep + l;
            //    sep = "\r\n";
            //}


            this.OpenFolderBrowserDialog = new RelayCommand(p =>
            {
                string dir = dir = this.AppSettings.HTTPServerDocumentRootPath;

                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.SelectedPath = dir;
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (dir != dialog.SelectedPath)
                    {
                        this.HTTPServerRoot = dialog.SelectedPath;

                    }

                }
            });


            this.SelectDatabaseFileCommand = new RelayCommand(p =>
            {
                var d = new System.Windows.Forms.OpenFileDialog();

                if (this.AppSettings.DatabaseFile != "")
                {
                    string f = Path.GetDirectoryName(this.AppSettings.DatabaseFile);
                    if (Directory.Exists(f))
                    {
                        d.InitialDirectory = f;
                    }
                }
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.DatabaseFile = d.FileName;
                }
            });

            this.SelectFSContentsFolderCommand = new RelayCommand(p =>
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    dialog.SelectedPath = this.FSContentsFolder;
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.FSContentsFolder = dialog.SelectedPath;
                    }
                }
            });

            this.SelectFSFolderCommand = new RelayCommand(p =>
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    if (Directory.Exists(this.FSFolder))
                    {
                        dialog.SelectedPath = this.FSFolder;
                    }
                    else
                    {
                        dialog.SelectedPath = @"c:\Program Files\WindowsApps";
                    }

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.FSFolder = dialog.SelectedPath;
                    }
                }
            });




            this.LastDir = "";
            //this.AddSearchFolderBrowserDialogCommand = new RelayCommand(p => {

            //    using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            //    {
            //        dialog.SelectedPath = this.LastDir;
            //        System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            //        {
            //            if(this.WebSearchFolders.Length > 0)
            //            {
            //                this.WebSearchFolders += "\r\n";
            //            }
            //            this.WebSearchFolders += dialog.SelectedPath;
            //            this.LastDir = dialog.SelectedPath;
            //        }

            //    }
            //});
            

            this.OpenLink = new RelayCommand(p =>
            {
                Process.Start(new ProcessStartInfo(p.ToString()));
            });
        }

        public bool SaveSettings()
        {
            this.AppSettings.HTTPServerDocumentRootPath = this.HTTPServerRoot;
            this.AppSettings.HTTPServerPort = this.HTTPServerPort;
            this.AppSettings.DatabaseFile = this.DatabaseFile;
            this.AppSettings.FSFolder = this.FSFolder;
            this.AppSettings.FSContentsFolder = this.FSContentsFolder;

            //this.AppSettings.WEBSearchFolders.Clear();
            //if (this.WebSearchFolders != null)
            //{
            //    string[] lines = this.WebSearchFolders.Split(new char[] { '\r', '\n' });
            //    foreach (string line in lines)
            //    {
            //        if (line.Trim().Length > 0)
            //        {
            //            this.AppSettings.WEBSearchFolders.Add(line.Trim());
            //        }
            //    }
            //}

            this.AppSettings.SaveSettings();


            return true;
        }

        private string fHTTPServerRoot;

        public string HTTPServerRoot
        {
            get { return fHTTPServerRoot; }
            set
            {
                fHTTPServerRoot = value;
                this.OnPropertyChanged();
            }
        }

        private int fHTTPServerPort;

        public int HTTPServerPort
        {
            get { return fHTTPServerPort; }
            set
            {
                fHTTPServerPort = value;
                this.OnPropertyChanged();
            }
        }

        private string fDatabaseFile;

        public string DatabaseFile
        {
            get { return fDatabaseFile; }
            set
            {
                fDatabaseFile = value;
                this.OnPropertyChanged();
            }
        }

        private string fFSContentsFolder;

        public string FSContentsFolder
        {
            get { return fFSContentsFolder; }
            set
            {
                fFSContentsFolder = value;
                this.OnPropertyChanged();
            }
        }

        private string fFSFolder;

        public string FSFolder
        {
            get { return fFSFolder; }
            set
            {
                fFSFolder = value;
                this.OnPropertyChanged();
            }
        }


        //private string fWebSearchFolders;

        //public string WebSearchFolders
        //{
        //    get { return fWebSearchFolders; }
        //    set { fWebSearchFolders = value;
        //        this.OnPropertyChanged();
        //    }
        //}


        private MainWindowViewModel fMainWindowViewModel;
        private AppSettingsViewModel AppSettings
        {
            get { return this.fMainWindowViewModel.AppSettingsViewModel; }
        }

        public RelayCommand OpenFolderBrowserDialog { get; }
        public RelayCommand SelectDatabaseFileCommand { get; }
        public RelayCommand SelectFSContentsFolderCommand { get; }
        public RelayCommand SelectFSFolderCommand { get; }

        private string LastDir;

        public RelayCommand AddSearchFolderBrowserDialogCommand { get; }
        public RelayCommand OpenLink { get; }
    }
}
