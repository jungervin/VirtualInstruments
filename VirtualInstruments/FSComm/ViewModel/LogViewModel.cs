using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSComm.ViewModel
{
    public class LogViewModel : BaseViewModel
    {

        public LogViewModel() : base()
        {
            this.LogClearCommand = new RelayCommand(p =>
            {
                this.LogText = "";
            });


        }

        public void Add(string text)
        {
            this.LogText += "\r\n" + text;
        }

        private string fLogText;

        public string LogText
        {
            get { return fLogText; }
            set
            {
                fLogText = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand LogClearCommand { get; }
    }


}
