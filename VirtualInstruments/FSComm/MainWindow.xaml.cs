using FSComm.Model;
using FSComm.View;
using FSComm.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FSComm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        DispatcherTimer Timer;
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            MainWindow.Instance = this;
            this.MainWindowViewModel = new MainWindowViewModel();
            this.DataContext = this.MainWindowViewModel;
            // this.Topmost = true;

            this.Timer = new DispatcherTimer();
            this.Timer.Interval = new TimeSpan(0, 0, 1);
            this.Timer.Tick += Timer_Tick;
            this.Timer.Start();

            //for(int i = 0; i < 5; i++ )
            //{
            //    DisplayComponent dc = new DisplayComponent();
            //    dc.DispNum = i + 1;
                
            //}

            int i = 0;
            foreach(DisplayViewModel dv in this.MainWindowViewModel.Displays)
            {
                DisplayComponent dc = new DisplayComponent(dv);
                
                
                this.Displays.Children.Add(dc);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.MainWindowViewModel.HTTPServerViewModel != null)
                {

                    int c = this.MainWindowViewModel.HTTPServerViewModel.SimChannel.Sessions.Count;
                    var txt = "";
                    txt += $"Clients: {c}\r\n";

                    foreach (var id in this.MainWindowViewModel.HTTPServerViewModel.SimChannel.Sessions.ActiveIDs)
                    {

                        var sch = this.MainWindowViewModel.HTTPServerViewModel.SimChannel.Sessions.Sessions.Where(k => k.ID == id).FirstOrDefault();
                        if (sch != null)
                        {
                            SimChannel simChannel = sch as SimChannel;
                            txt += $"{id} - {simChannel.ClientID}\r\n";
                        }
                        else
                        {
                            txt += $"{id}\r\n";

                        }
                    }

                    this.WSClients.Text = txt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal MainWindowViewModel MainWindowViewModel { get; private set; }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            //this.BrowserView2 = new BrowserView();
            //this.BrowserView2.Left = this.SelectedBrowser2Screen.Bounds.Left;
            //this.BrowserView2.Top = this.SelectedBrowser2Screen.Bounds.Top;
            //this.BrowserView2.Width = this.SelectedBrowser2Screen.Bounds.Width;
            //this.BrowserView2.Height = this.SelectedBrowser2Screen.Bounds.Height;
            //this.BrowserView2.Load("http://127.0.0.1:" + this.HTTPServerViewModel.Port.ToString());
            //this.BrowserView2.Show();

            DisplayView dv = new DisplayView();

            dv.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.MainWindowViewModel.SerialDeviceViewModel != null)
            {
                this.MainWindowViewModel.SerialDeviceViewModel.Stop();
            }

            if(this.MainWindowViewModel.Displays != null)
            {
                foreach(DisplayViewModel d in MainWindowViewModel.Displays)
                {
                    if (d.Window != null)
                    {
                        d.Window.Close();
                        d.Window = null;
                    }
                }
            }
            //if (this.MainWindowViewModel.DisplayView1 != null)
            //{
            //    this.MainWindowViewModel.DisplayView1.Close();
            //    this.MainWindowViewModel.DisplayView1 = null;
            //}

            //if (this.MainWindowViewModel.DisplayView2 != null)
            //{
            //    this.MainWindowViewModel.DisplayView2.Close();
            //    this.MainWindowViewModel.DisplayView2 = null;
            //}
        }

        private void LogTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.LogTextBox.ScrollToEnd();
        }

        private void btnFindHandle_Click(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;



            //ipm.Button = sender as Button;
            this.MainWindowViewModel.InstrumentsProfileViewModel.FindHandle(ipm);
        }

        private void InstrumentMoveLeft(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            ipm.PosX--;
            this.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(ipm);


        }

        private void InstrumentMoveRight(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            ipm.PosX++;

            //this.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(ipm);
        }

        private void InstrumentMoveUp(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            ipm.PosY--;
            //this.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(ipm);
        }

        private void InstrumentMoveDown(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            ipm.PosY++;
            //this.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(ipm);
        }

        private void InstrumentWidthDec(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            ipm.Width--;
            //this.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(ipm);

        }

        private void InstrumentWidthInc(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            ipm.Width++;
            //this.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(ipm);
        }

        private void InstrumentHeightDec(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            ipm.Height--;
            //this.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(ipm);
        }
        private void InstrumentHeightInc(object sender, RoutedEventArgs e)
        {
            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            ipm.Height++;
            //this.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(ipm);
        }

        private void ReadPos(object sender, RoutedEventArgs e)
        {

            InstrumentModel ipm = (sender as Control).DataContext as InstrumentModel;
            //this.MainWindowViewModel.InstrumentsProfileViewModel.ReadPos(ipm);
            if (ipm.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Find Handle First!");
                return;
            }
            this.MainWindowViewModel.InstrumentsProfileViewModel.ReadPosCommand.Execute(ipm);
        }

        //private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.OriginalSource is TextBox == true)
        //    {
        //        if ((e.Key.Equals(Key.Enter)) || (e.Key.Equals(Key.Return)))
        //        {
        //            e.Handled = false;
        //        }
        //    }else
        //    {
        //        e.Handled =true;
        //    }
        //}
    }
}
