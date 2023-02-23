using FSComm.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using vJoyInterfaceWrap;

namespace FSComm.ViewModel
{
    public class vJoyViewModel : BaseViewModel
    {
        public MainWindowViewModel MainWindowViewModel { get; }

        private vJoy Joystick;
        private DispatcherTimer Timer;
        uint id = 1;
        public vJoyViewModel(MainWindowViewModel mainwindowviewmodel)
        {
            this.MainWindowViewModel = mainwindowviewmodel;
            this.Joystick = new vJoy();
            //this.Init();

            //this.Timer = new DispatcherTimer();
            //this.Timer.Interval = TimeSpan.FromSeconds(1);
            //this.Timer.Tick += Timer_Tick;
            //this.Timer.Start();

        }

        public void Init()
        {
            VjdStat status = Joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    this.Status = String.Format("\tvJoy Device {0} is already owned by this feeder\n", id);
                    break;
                case VjdStat.VJD_STAT_FREE:
                    this.Status = String.Format("\tvJoy Device {0} is free\n", id);
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    this.Status = String.Format("\tvJoy Device {0} is already owned by another feeder\n\tCannot continue\n", id);
                    return;
                case VjdStat.VJD_STAT_MISS:
                    this.Status = String.Format("\tvJoy Device {0} is not installed or disabled\n\tCannot continue\n", id);
                    return;
                default:
                    this.Status = String.Format("\tvJoy Device {0} general error\n\tCannot continue\n", id);
                    return;
            };
            string log = $"\nvJOY Init:\n\tStatus: {this.Status}";

            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!Joystick.AcquireVJD(id))))
            {
                this.MainWindowViewModel.LogViewModel.Add($"{log}\tFailed to acquire vJoy device number {id}.\n");
                
                return;
            }
            else
            {
                this.MainWindowViewModel.LogViewModel.Add($"{log}\tAcquired: vJoy device number {id}.\n");
            }

            Joystick.ResetVJD(id);
            this.Connected = true;

        }

        bool toggle;
        private void Timer_Tick(object sender, EventArgs e)
        {

            if (this.Joystick.vJoyEnabled())
            {
                this.Vendor = this.Joystick.GetvJoyManufacturerString();
                this.Product = this.Joystick.GetvJoyProductString();
                this.VersionNumber = this.Joystick.GetvJoySerialNumberString();

                toggle = !toggle;

                int nButtons = Joystick.GetVJDButtonNumber(id);
                int ContPovNumber = Joystick.GetVJDContPovNumber(id);
                int DiscPovNumber = Joystick.GetVJDDiscPovNumber(id);
                this.SetButton(toggle, id, 1);
                this.SetButton(toggle, id, 2);
                this.SetButton(toggle, id, 3);
            }
        }



        public void SetButton(bool value, uint id, uint btn)
        {
            this.MainWindowViewModel.LogViewModel.Add($"vJOY Button:\r\n\tid: {id}\r\n\tbtn: {btn}\r\n\tvalue: {value}");
            this.Joystick.SetBtn(value, id, btn);
            //VjdStat status = Joystick.GetVJDStatus(id);
            //var position = new vJoy.JoystickState();
            //Console.WriteLine(position.Buttons);
        }

        public void SetPushButton(uint id, uint btn, int sleep)
        {
            this.MainWindowViewModel.LogViewModel.Add($"vJOY PushButton:\r\n\tid: {id}\r\n\tbtn: {btn}\r\n\tsleep: {sleep}");
            this.Joystick.SetBtn(true, id, btn);
            Thread.Sleep(sleep);
            this.Joystick.SetBtn(false, id, btn);
        }



        private string fVendor;

        public string Vendor
        {
            get { return fVendor; }
            set
            {
                fVendor = value;
                this.OnPropertyChanged();
            }
        }

        private string fProduct;

        public string Product
        {
            get { return fProduct; }
            set
            {
                fProduct = value;
                this.OnPropertyChanged();
            }
        }

        private string fVersionNumber;

        public string VersionNumber
        {
            get { return fVersionNumber; }
            set
            {
                fVersionNumber = value;
                this.OnPropertyChanged();
            }
        }

        private string fStatus;

        public string Status
        {
            get { return fStatus; }
            set
            {
                fStatus = value;
                this.OnPropertyChanged();
            }
        }

        private bool fConnected;

        public bool Connected
        {
            get { return fConnected; }
            set
            {
                fConnected = value;
                this.OnPropertyChanged();
            }
        }

    }
}
