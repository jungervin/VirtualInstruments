using FSComm.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace FSComm.Model
{
    public class InstrumentModel : BaseViewModel
    {
        private string fInstrumentName;


        private Brush fButtonBg;

        [JsonIgnore]
        public Brush ButtonBg
        {
            get { return fButtonBg; }
            set
            {
                fButtonBg = value;
                this.OnPropertyChanged();
            }
        }

        public string InstrumentName
        {
            get { return fInstrumentName; }
            set
            {
                fInstrumentName = value;
                this.OnPropertyChanged();
            }
        }

        private IntPtr fHandle;

        [JsonIgnore]
        public IntPtr Handle
        {
            get { return fHandle; }
            set
            {
                fHandle = value;
                this.OnPropertyChanged();
                if (this.Handle != IntPtr.Zero)
                {
                    //MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(this);
                    //MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SetStyle(this);
                }
                this.IsHandleValid = value != IntPtr.Zero;
            }
        }

        private int fPosX;

        public int PosX
        {
            get { return fPosX; }
            set
            {
                fPosX = value;
                this.OnPropertyChanged();
                if (this.Handle != IntPtr.Zero)
                {
                    MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(this);
                }
            }
        }

        private int fPosY;

        public int PosY
        {
            get { return fPosY; }
            set
            {
                fPosY = value;
                this.OnPropertyChanged();
                if (this.Handle != IntPtr.Zero)
                {
                    MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(this);
                }
            }
        }

        private int fWidth;

        public int Width
        {
            get { return fWidth; }
            set
            {

                fWidth = Math.Max(100, value);
                this.OnPropertyChanged();
                if (this.Handle != IntPtr.Zero)
                {
                    MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(this);
                }
            }

        }

        private int fHeight;

        public int Height
        {
            get { return fHeight; }
            set
            {
                fHeight = Math.Max(10, value);
                this.OnPropertyChanged();
                if (this.Handle != IntPtr.Zero)
                {
                    MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SetPos(this);
                }
            }
        }

        private bool fFrameVisible = true;
        public bool FrameVisible
        {
            get { return fFrameVisible; }
            set
            {
                fFrameVisible = value;
                if (this.Handle != IntPtr.Zero)
                {
                    MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SetStyle(this);
                }
            }
        }

        private bool fIsFullScreen;

        public bool IsFullScreen
        {
            get { return fIsFullScreen; }
            set
            {
                //if (MainWindow.Instance.MainWindowViewModel != null)
                //{
                //    fIsFullScreen = value;
                //    MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SetFullFullScreen(this);
                //}
            }
        }

        private bool fIsHandleValid;

        public bool IsHandleValid
        {
            get { return fIsHandleValid; }
            set
            {
                fIsHandleValid = value;
                this.OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public InstrumentsViewModel Owner { get; internal set; }
    }
}
