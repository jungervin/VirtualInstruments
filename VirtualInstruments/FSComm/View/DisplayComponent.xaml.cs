using FSComm.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace FSComm.View
{
    /// <summary>
    /// Interaction logic for DisplayComponent.xaml
    /// </summary>
    public partial class DisplayComponent : UserControl //, INotifyPropertyChanged
    {
        public DisplayComponent(DisplayViewModel viewmodel)
        {
            InitializeComponent();
            //this.PropertyChanged += this.OnPropertyChanged;
            this.DataContext = viewmodel;
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName] string _sPropertyName = null)
        //{
        //    PropertyChangedEventHandler hEventHandler = this.PropertyChanged;
        //    if (hEventHandler != null && !string.IsNullOrEmpty(_sPropertyName))
        //    {
        //        hEventHandler(this, new PropertyChangedEventArgs(_sPropertyName));
        //    }
        //}
        //private int fDispNum;

        //public int DispNum
        //{
        //    get { return fDispNum; }
        //    set { fDispNum = value;
        //        this.OnPropertyChanged();
        //    }
        //}

    }
}
