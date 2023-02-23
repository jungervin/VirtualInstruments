using FSComm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FSComm.ViewModel
{
    public class DisplayViewModel : BaseViewModel
    {
        Brush cOn = new SolidColorBrush(Colors.CornflowerBlue);
        Brush cOff = new SolidColorBrush(Colors.Gainsboro);
        public DisplayViewModel() : base()
        {
            this.Fill = cOff;
            this.CommandOpen = new RelayCommand((p) =>
            {
                this.Fill = cOn;
                this.Window = new DisplayView();
                this.Window.Init("http://127.0.0.1:5000/index.html", MainWindow.Instance.MainWindowViewModel.AppSettingsViewModel.HTTPServerDocumentRootPath);
                this.Window.Left = this.PosLeft;
                this.Window.Top = this.PosTop;
                this.Window.Width = this.PosWidth;
                this.Window.Height = this.PosHeight;
                this.Window.Show();
            }, (p) =>
            {
                return this.Window == null;
            });

            this.CommandClose = new RelayCommand((p) =>
            {
                if (this.Window != null)
                {
                    this.Fill = cOff;
                    this.Window.Close();
                    this.Window = null;
                }
            }, (p) =>
             {
                 return this.Window != null;
             });
        }

        private int fDisplayNum;

        public int DisplayNum
        {
            get { return fDisplayNum; }
            set
            {
                fDisplayNum = value;
                this.OnPropertyChanged();
            }
        }


        private string fDisplayName;

        public string DisplayName
        {
            get { return fDisplayName; }
            set
            {
                fDisplayName = value;
                this.OnPropertyChanged();
            }
        }

        private Brush fFill;

        public Brush Fill
        {
            get { return fFill; }
            set { fFill = value; 
            this.OnPropertyChanged();
            }
        }

        private double fPosLeft;

        public double PosLeft
        {
            get { return fPosLeft; }
            set
            {
                fPosLeft = value;
                this.OnPropertyChanged();
            }
        }

        private double fPosTop;

        public double PosTop
        {
            get { return fPosTop; }
            set
            {
                fPosTop = value;
                this.OnPropertyChanged();
            }
        }

        private double fPosWidth;

        public double PosWidth
        {
            get { return fPosWidth; }
            set
            {
                fPosWidth = value;
                this.OnPropertyChanged();
            }
        }

        private double fPosHeight;

        public double PosHeight
        {
            get { return fPosHeight; }
            set
            {
                fPosHeight = value;
                this.OnPropertyChanged();
            }
        }

        private DisplayView window;

        public DisplayView Window
        {
            get { return window; }
            set
            {
                window = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand CommandOpen { get; }
        public RelayCommand CommandClose { get; }
    }
}
