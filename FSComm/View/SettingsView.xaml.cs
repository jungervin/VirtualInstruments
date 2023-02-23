using FSComm.ViewModel;

using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FSComm.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsView(MainWindowViewModel main_window_view_model)
        {
            InitializeComponent();
            this.ViewModel = new SettingsViewModel(main_window_view_model);
            this.DataContext = this.ViewModel;
            this.Topmost = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.SaveSettings())
            {
                this.DialogResult = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
