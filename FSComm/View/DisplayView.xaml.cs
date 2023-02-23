//using GlassCockpit.ViewModel;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
//using static GlassCockpit.Utility.JsonModels;

namespace FSComm.View
{
    /// <summary>
    /// Interaction logic for DisplayView.xaml
    /// </summary>
    public partial class DisplayView : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public FileSystemWatcher FileWatcher { get; set; }
        public DisplayView()
        {
            InitializeComponent();
            this.Background = new SolidColorBrush(Color.FromArgb(1, 1, 1, 1));
            //this.WebView.DefaultBackgroundColor = System.Drawing.Color.FromArgb(1, 1, 1, 1);
            //this.WebView.DefaultBackgroundColor = System.Drawing.Color.FromArgb(0, 255, 255, 255); // System.Drawing.Color.Transparent;
            //this.PanelATC.Source = new Uri("http://127.0.0.1:5000/atc.html");
            //this.Background = new SolidColorBrush(System.Windows.Media.Colors.Transparent);
            this.WebView.DefaultBackgroundColor = System.Drawing.Color.Transparent;
            this.WebView.WebMessageReceived += WebView_WebMessageReceived;

            //this.WebView2.WebMessageReceived += WebView_WebMessageReceived;
            //this.WebView2.DefaultBackgroundColor = System.Drawing.Color.FromArgb(0, 1, 1, 1);
            //this.WebView2.SourceUpdated += WebView_SourceUpdated;
            //this.WebView2.NavigationCompleted += WebView2_NavigationCompleted;

            //var op = new CoreWebView2EnvironmentOptions("--disable-web-security");

            //var env = CoreWebView2Environment.CreateAsync("", "", op);

            // WebView.EnsureCoreWebView2Async(env);


            //_ = InitializeAsync();

            this.timer.Interval = TimeSpan.FromSeconds(3);
            this.timer.Tick += Timer_Tick;
            //this.timer.Start();
        }

        async private Task InitializeAsync()
        {
            // Wait for CoreWebView2 initialization.
            // Explicitly triggers initialization of the control's CoreWebView2.
            await WebView.EnsureCoreWebView2Async();

            // Set a mapping between a virtual host name and a folder path
            // to make available to web sites via that host name.
            WebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "", "", CoreWebView2HostResourceAccessKind.Allow);

            WebView.Source = new Uri($"http://127.0.0.1:5000/index.html");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.timer.Stop();
            this.timer.Tick -= Timer_Tick;

            this.WebView.WebMessageReceived -= WebView_WebMessageReceived;
            //this.WebView2.WebMessageReceived -= WebView_WebMessageReceived;
            //this.WebView2.SourceUpdated -= WebView_SourceUpdated;
            //this.WebView2.NavigationCompleted -= WebView2_NavigationCompleted;
            this.WebView.Dispose();
            this.WebView = null;
            //this.WebView2.Dispose();
            //this.WebView2 = null;
            this.timer = null;
        }

        private  void Timer_Tick(object sender, EventArgs e)
        {
            //var r = this.timer.IsEnabled;
            //this.timer.Stop();
            //this.setPos(WebView2, 19, 47, 10);

            //this.timer.IsEnabled = r;
           

        }

        private void WebView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                WebView2 wv = sender as WebView2;
                Console.WriteLine(wv.Source);
                insertJS(wv);

            }
        }

        private async void setPos(WebView2 vbw, double lat, double lon, double deg)
        {
            //if (this.WebView2.Source.Host.Contains("windy.com"))
            //{
            //    string js = "";

            //    //wbv.

            //    //js += "let myScript = document.createElement('script')\n";
            //    //js += "myScript.setAttribute('src', 'http://127.0.0.1:5000/js/windy.js');\n";
            //    //js += "document.body.appendChild(myScript);\n";
                
                

            //    //js += $"setPosition({MQTTClientViewModel.PLANE_LATITUDE},{MQTTClientViewModel.PLANE_LOGITUDE},{MQTTClientViewModel.PLANE_HEADING_DEGREES_TRUE});\n";
            //    //js = $"setPosition(\"{MQTTClientViewModel.PLANE_LATITUDE}\",\"{MQTTClientViewModel.PLANE_LOGITUDE}\", \"{MQTTClientViewModel.PLANE_HEADING_DEGREES_TRUE}\")";
            //    //await this.WebView2.CoreWebView2.ExecuteScriptAsync(js);
            //}
        }
        private async void insertJS(WebView2 wbv)
        {
            if (wbv.Source.Host.Contains("windy.com"))
            {
                string js = "";

                //wbv.

                js += "let myScript = document.createElement('script')\n";
                js += "myScript.setAttribute('src', 'http://127.0.0.1:5000/js/windy.js');\n";
                js += "document.body.appendChild(myScript);\n";
                //js += "alert('HELLO');\n";

                await wbv.CoreWebView2.ExecuteScriptAsync(js);
                this.timer.Start();
            } else
            {
               // this.timer.Stop();
            }
            //await wbv.CoreWebView2.ExecuteScriptAsync("alert('HELLO')");
        }



        private void WebView_SourceUpdated(object sender, DataTransferEventArgs e)
        {
        }

        private void WebView_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                //string j = e.TryGetWebMessageAsString();
                //CommandModel cm = JsonConvert.DeserializeObject<CommandModel>(j);

                //if (cm.command == "SetDisplaySize")
                //{
                //    DisplaySizeModel dm = JsonConvert.DeserializeObject<DisplaySizeModel>(cm.data.ToString());
                //    this.Left = dm.left;
                //    this.Top = dm.top;
                //    this.Width = dm.width;
                //    this.Height = dm.height;
                //}
                //else if (cm.command == "SetWindowState")
                //{
                //    WindowStateModel wsm = JsonConvert.DeserializeObject<WindowStateModel>(cm.data.ToString());
                //    this.WindowState = wsm.state;
                //}
                //else if (cm.command == "SetTopmost")
                //{
                //    TopmostModel tm = JsonConvert.DeserializeObject<TopmostModel>(cm.data.ToString());
                //    this.Topmost = tm.topmost;
                //}
                //else if (cm.command == "ShowWebView")
                //{
                //    //TopmostModel tm = JsonConvert.DeserializeObject<TopmostModel>(cm.data.ToString());
                //    //this.Topmost = tm.topmost;
                //    MarginModel dm = JsonConvert.DeserializeObject<MarginModel>(cm.data.ToString());
                //    this.WebView2.Margin = new Thickness(dm.left, dm.top, dm.right, dm.bottom);


                //    this.WebView2.Visibility = this.WebView2.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                    
                //    Console.WriteLine(this.WebView2.Visibility.ToString());
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Init(string url, string root_path)
        {
            try
            {

            //int port = 5500;
            this.WebView.Source = new Uri(url);
            //this.WebView2.Source = new Uri("https://www.windy.com/?6.479,26.455,6");
            //this.WebView2.Source = new Uri($"http://127.0.0.1:{MainWindow.Instance.MainWindowViewModel.HTTPServerViewModel.Port}/man_index.html");
            if (Directory.Exists(root_path))
            {
                Console.WriteLine(root_path);
                this.FileWatcher = new FileSystemWatcher();
                this.FileWatcher.Path = root_path;
                this.FileWatcher.IncludeSubdirectories = true;
                this.FileWatcher.EnableRaisingEvents = true;
                this.FileWatcher.Changed += FileWatcher_Changed;
            }
            else
            {
                //Globals.MsgLogger.Add("FileWatcher: DocumentRootPath does not exists!");
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //if (System.IO.Path.GetExtension(e.FullPath) == ".html")
            
                {
                    Dispatcher.Invoke(async () =>
                    {
                        if (this.WebView != null)
                        {
                            await this.WebView.EnsureCoreWebView2Async();
                            this.WebView.Reload();
                        }
                        //if (this.WebView2 != null)
                        //{
                        //    await this.WebView2.EnsureCoreWebView2Async();
                        //    this.WebView2.Reload();
                        //}
                    });
                
            }
        }

        private void PanelTop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            ////Set the window style to noactivate.
            //var helper = new WindowInteropHelper(this);
            //SetWindowLong(helper.Handle, GWL_EXSTYLE,
            //    GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
        }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    }
}
