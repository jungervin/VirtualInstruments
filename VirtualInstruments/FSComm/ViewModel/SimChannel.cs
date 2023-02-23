using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace FSComm.ViewModel
{
    internal class SimChannel : WebSocketBehavior
    {

        //private string _name;
        //private static int _number = 0;
        //private string _prefix;

        public SimChannel()
        {
            //_prefix = "anon#";
            //MainWindowViewModel mainwindowviewmodel
            //this.MainWindowViewModel = mainwindowviewmodel;
            this.ClientID = "unknown";
        }

        //public string Prefix
        //{
        //    get
        //    {
        //        return _prefix;
        //    }

        //    set
        //    {
        //        _prefix = !value.IsNullOrEmpty() ? value : "anon#";
        //    }
        //}

        //private string getName()
        //{
        //    var name = QueryString["name"];

        //    return !name.IsNullOrEmpty() ? name : _prefix + getNumber();
        //}

        //private static int getNumber()
        //{
        //    return Interlocked.Increment(ref _number);
        //}

        protected override void OnClose(CloseEventArgs e)
        {
            var fmt = "Client got logged off...";

            //var msg = String.Format(fmt, "");
            var msg = "{\"clientId\": \"SERVER\", \"topic\": \"status\", \"payload\": \"Client got logged off...!\", \"timestamp\": 0}";

            Sessions.Broadcast(msg);
        }

        protected override void OnMessage(MessageEventArgs e)
        {

            if (this.MainWindowViewModel != null && this.MainWindowViewModel.vJoyViewModel != null)
            {
                if (e.Data.StartsWith("client/vjoy/button"))
                {
                    string json = e.Data;
                    vJoyButtonModel a = JsonConvert.DeserializeObject<vJoyButtonModel>(json);
                    this.MainWindowViewModel.vJoyViewModel.SetButton(a.value, a.id, a.btn);
                }
                else if (e.Data.StartsWith("client/vjoy/pushbutton"))
                {
                    string json = e.Data;
                    vJoyPushButtonModel a = JsonConvert.DeserializeObject<vJoyPushButtonModel>(json);

                    this.MainWindowViewModel.vJoyViewModel.SetPushButton(a.id, a.btn, a.sleep);

                }
            }

            Match m = Regex.Match(e.Data, "\"clientId\":\"(.*?)\"");
            if (m.Success)
            {
                this.ClientID = m.Groups[1].Value;
            }

            //List<string> l = Sessions.IDs.ToList();


            Sessions.Broadcast(e.Data);
        }

        protected override void OnOpen()
        {
            var msg = "{\"clientId\": \"SERVER\", \"topic\": \"status\", \"payload\": \"Client Connected!\", \"timestamp\": 0}";
            Sessions.Broadcast(msg);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }

        private string fClientID;
        private MainWindowViewModel MainWindowViewModel
        {
            get { return MainWindow.Instance.MainWindowViewModel; }
        }

        public string ClientID
        {
            get { return fClientID; }
            set { fClientID = value; }
        }


    }
}
