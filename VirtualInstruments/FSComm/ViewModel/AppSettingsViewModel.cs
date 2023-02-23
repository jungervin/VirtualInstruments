using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace FSComm.ViewModel
{
    public class AppSettingsViewModel : BaseViewModel
    {
        public static string ConfFileName = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Data", "conf.xml");
        public AppSettingsViewModel()
        {
            this.MQTTServerAddress = "127.0.0.1";
            this.MQTTServerPort = 1883;
            this.MQTTServerUsername = "user";
            this.MQTTServerPassword = "titok";

            this.HTTPServerPort = 5000;
            this.HTTPServerDocumentRootPath = @"D:\HTML\www";

            this.DatabaseFile = "";

            this.Display1Name = "";
            this.Display2Name = "";

            this.IgnoredSerialPorts = "COM1,COM2";

            //this.WEBSearchFolders = new List<string>();

            this.FSFolder = "";
            this.FSContentsFolder = "";

            this.LastProfileFile = "";
            //this.WEBSearchFolders.Add(@"C:\MSFS2020\Community\ikarosz");
        }

        public string MQTTServerAddress { get; set; }
        public int MQTTServerPort { get; set; }
        public string MQTTServerUsername { get; set; }
        public string MQTTServerPassword { get; set; }


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

        //public string HTTPServerDocumentRootPath { get; set; }
        private string fHTTPServerDocumentRootPath;

        public string HTTPServerDocumentRootPath
        {
            get { return fHTTPServerDocumentRootPath; }
            set
            {
                fHTTPServerDocumentRootPath = value;
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


        public string Display1Name { get; set; }
        public string Display2Name { get; set; }
        public string IgnoredSerialPorts { get; set; }

        public static AppSettingsViewModel LoadSettings()
        {
            AppSettingsViewModel s = null;
            try
            {
                if (File.Exists(AppSettingsViewModel.ConfFileName))
                {
                    FileStream ReadFileStream = new FileStream(AppSettingsViewModel.ConfFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    XmlSerializer SerializerObj = new XmlSerializer(typeof(AppSettingsViewModel));
                    s = (AppSettingsViewModel)SerializerObj.Deserialize(ReadFileStream);
                    ReadFileStream.Close();
                    return s;
                }
                else
                {
                    MessageBox.Show("Nincsenek beállítások!\r\nAz alapértemezések lesznek használva!");
                    return new AppSettingsViewModel();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nem sikerült betölteni a beállításokat!\r\nAz alapértemezések lesznek használva!\r\n" + ex.Message);
            }
            return new AppSettingsViewModel();
        }

        //private List<string> fWEBSearchFolders;

        //public List<string> WEBSearchFolders
        //{
        //    get { return fWEBSearchFolders; }
        //    set {
        //        fWEBSearchFolders = value;
        //        this.OnPropertyChanged();
        //    }
        //}

        public string FSFolder { get; set; }
        public string FSContentsFolder { get; set; }
        public string LastProfileFile { get; set; }
        public void SaveSettings()
        {
            string d = Path.GetDirectoryName(AppSettingsViewModel.ConfFileName);
            try
            {
                if (!Directory.Exists(d))
                {
                    Directory.CreateDirectory(d);
                }


                XmlSerializer SerializerObj = new XmlSerializer(typeof(AppSettingsViewModel));
                TextWriter WriteFileStream = new StreamWriter(AppSettingsViewModel.ConfFileName);
                SerializerObj.Serialize(WriteFileStream, this);
                WriteFileStream.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}