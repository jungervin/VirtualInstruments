using FSComm.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FSComm.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {


        public MainWindowViewModel()
        {
            try
            {

                //                Process[] processes = Process.GetProcesses();
                //                foreach (var process in processes)
                //                {
                ////                    if (process.MainWindowTitle.Contains("Microsoft Flight Simulator"))
                //                        if (process.MainWindowTitle.Contains("AS530"))
                //                        {
                //                            foreach (var thread in process.Threads)
                //                           // if (thread. .ProcessName.Contains("AS530_Screen"))
                //                            {
                //                                Console.WriteLine(process.Handle.ToString("X"));
                //                            }
                //                    }
                //                }



                this.AppSettingsViewModel = AppSettingsViewModel.LoadSettings();

                this.LogViewModel = new LogViewModel();



                //if(this.AppSettingsViewModel.FS)
                string err = "";
                string www = "";
                if (!Directory.Exists(this.AppSettingsViewModel.FSFolder))
                {
                    err += "Flight Simulator Core Folder does not exists!\r\n";
                }

                if (!Directory.Exists(this.AppSettingsViewModel.FSContentsFolder))
                {
                    err += "Flight Simulator Contents Folder does not exists!\r\n";
                }
                else
                {
                    www = Path.Combine(this.AppSettingsViewModel.FSContentsFolder, @"Community\ikarosz-mqttpanel\www");
                    if (!Directory.Exists(www))
                    {
                        err += $"Could not find www fonder!\r\n{www} is missing?";
                    }
                }


                if (err == "")
                {

                    this.DatabaseViewModel = new DatabaseViewModel(this);
                    this.DatabaseViewModel.Init();


                    //this.MQTTClientViewModel = new MQTTClientViewModel(this);
                    //this.MQTTClientViewModel.Init();

                    this.SerialDeviceViewModel = new SerialDeviceViewModel(this);
                    this.SerialDeviceViewModel.Start();
                    try
                    {
                        this.vJoyViewModel = new vJoyViewModel(this);
                        this.vJoyViewModel.Init();
                    }
                    catch(Exception ex)
                    {
                        this.LogViewModel.Add(ex.Message);
                    }

                    this.HTTPServerViewModel = new SimpleHTTPServerViewModel(this, this.AppSettingsViewModel.HTTPServerPort, www);
                    this.HTTPServerViewModel.Start();

                    this.InstrumentsProfileViewModel = new InstrumentsViewModel(this);

                    if (!String.IsNullOrEmpty(this.AppSettingsViewModel.LastProfileFile))
                    {
                        try
                        {
                            this.InstrumentsProfileViewModel.Load(this.AppSettingsViewModel.LastProfileFile);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }
                else
                {
                    err += "\r\nGoto Settings and restart the app!";
                    MessageBox.Show(err, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }


            //if (this.AppSettingsViewModel.Display1Name != "")
            //{
            //    this.SelectedDisplay1 = WpfScreenHelper.Screen.AllScreens.Where(k => k.DeviceName == this.AppSettingsViewModel.Display1Name).FirstOrDefault();
            //}

            //if (this.AppSettingsViewModel.Display2Name != "")
            //{
            //    this.SelectedDisplay2 = WpfScreenHelper.Screen.AllScreens.Where(k => k.DeviceName == this.AppSettingsViewModel.Display2Name).FirstOrDefault();
            //}

            //this.Display1OpenCommand = new RelayCommand((p) =>
            //{
            //    this.DisplayView1 = new DisplayView();
            //    this.DisplayView1.Init(
            //        "http://127.0.0.1:5000",
            //        this.AppSettingsViewModel.HTTPServerDocumentRootPath);

            //    this.DisplayView1.Left = this.SelectedDisplay1.Bounds.Left;
            //    this.DisplayView1.Top = this.SelectedDisplay1.Bounds.Top;
            //    this.DisplayView1.Width = this.SelectedDisplay1.Bounds.Width;
            //    this.DisplayView1.Height = this.SelectedDisplay1.Bounds.Height;
            //    //this.DisplayView1.Load("http://127.0.0.1:" + this.HTTPServerViewModel.Port.ToString());
            //    this.DisplayView1.Show();

            //    this.AppSettingsViewModel.Display1Name = this.SelectedDisplay1.DeviceName;
            //    this.AppSettingsViewModel.SaveSettings();

            //}, (p) => { return this.SelectedDisplay1 != null && this.DisplayView1 == null; });

            //this.Display1CloseCommand = new RelayCommand((p) =>
            //{
            //    this.DisplayView1.Close();
            //    this.DisplayView1 = null;
            //}, (p) => { return this.DisplayView1 != null; });


            //this.Display2OpenCommand = new RelayCommand((p) =>
            //{
            //    this.DisplayView2 = new DisplayView();
            //    this.DisplayView2.Init(
            //        "http://127.0.0.1:5000",

            //        this.AppSettingsViewModel.HTTPServerDocumentRootPath);
            //    this.DisplayView2.Left = this.SelectedDisplay2.Bounds.Left;
            //    this.DisplayView2.Top = this.SelectedDisplay2.Bounds.Top;
            //    this.DisplayView2.Width = this.SelectedDisplay2.Bounds.Width;
            //    this.DisplayView2.Height = this.SelectedDisplay2.Bounds.Height;
            //    //this.DisplayView1.Load("http://127.0.0.1:" + this.HTTPServerViewModel.Port.ToString());
            //    this.DisplayView2.Show();

            //    this.AppSettingsViewModel.Display2Name = this.SelectedDisplay2.DeviceName;
            //    this.AppSettingsViewModel.SaveSettings();

            //}, (p) => { return this.SelectedDisplay2 != null && this.DisplayView2 == null; });

            //this.Display2CloseCommand = new RelayCommand((p) =>
            //{
            //    this.DisplayView2.Close();
            //    this.DisplayView2 = null;
            //}, (p) => { return this.DisplayView2 != null; });

            //this.BrowserOpenCommand = new RelayCommand((p) =>
            //{
            //    Process.Start("http://127.0.0.1:5000");


            //});




            this.SettingsCommand = new RelayCommand(p =>
            {
                SettingsView d = new SettingsView(this);
                if (d.ShowDialog() == true)
                {

                    if (this.HTTPServerViewModel != null && this.HTTPServerViewModel.IsListening)
                    {
                        this.HTTPServerViewModel.Stop();
                        this.HTTPServerViewModel = null;
                    }
                    this.HTTPServerViewModel = new SimpleHTTPServerViewModel(this, this.AppSettingsViewModel.HTTPServerPort, this.AppSettingsViewModel.HTTPServerDocumentRootPath);
                    this.HTTPServerViewModel.Start();

                }
            });








            //this.AppSettingsViewModel = AppSettingsViewModel.LoadSettings();

            


            this.OpenCommand = new RelayCommand(
            (p) =>
            {
                MessageBox.Show("OK", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            },
            (p) =>
            {
                return true;
            });

            List<DisplayViewModel> l = new List<DisplayViewModel>();
            foreach (WpfScreenHelper.Screen s in  WpfScreenHelper.Screen.AllScreens)
            {
                DisplayViewModel d = new DisplayViewModel();
                d.DisplayName = s.DeviceName.Replace("\\", "").Replace(".", "").Replace("DISPLAY", "");
                
                d.PosLeft = s.Bounds.Left;
                d.PosTop = s.Bounds.Top;
                d.PosWidth = s.Bounds.Width;
                d.PosHeight = s.Bounds.Height;
                l.Add(d);
            }

            this.Displays = l;
        }

        internal void Close()
        {
            foreach(DisplayViewModel d in this.Displays)
            {
                if(d.Window != null)
                {
                    d.Window.Close();
                    d.Window = null;
                }
            }
        }

        private List<DisplayViewModel> fDisplays;

        public List<DisplayViewModel> Displays
        {
            get { return fDisplays; }
            set { fDisplays = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand SettingsCommand { get; }
        public RelayCommand OpenCommand { get; }
        public AppSettingsViewModel AppSettingsViewModel { get; set; }
        public LogViewModel LogViewModel { get; set; }
        public SimpleHTTPServerViewModel HTTPServerViewModel { get; set; }
        public InstrumentsViewModel InstrumentsProfileViewModel { get; }
        public SerialDeviceViewModel SerialDeviceViewModel { get; set; }
        public vJoyViewModel vJoyViewModel { get; set; }
        public DatabaseViewModel DatabaseViewModel { get; }
    }
}
