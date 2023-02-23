using FSComm.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Media;

namespace FSComm.ViewModel
{
    public static class SWP
    {
        public static readonly int
        NOSIZE = 0x0001,
        NOMOVE = 0x0002,
        NOZORDER = 0x0004,
        NOREDRAW = 0x0008,
        NOACTIVATE = 0x0010,
        DRAWFRAME = 0x0020,
        FRAMECHANGED = 0x0020,
        SHOWWINDOW = 0x0040,
        HIDEWINDOW = 0x0080,
        NOCOPYBITS = 0x0100,
        NOOWNERZORDER = 0x0200,
        NOREPOSITION = 0x0200,
        NOSENDCHANGING = 0x0400,
        DEFERERASE = 0x2000,
        ASYNCWINDOWPOS = 0x4000;
    }
    public class InstrumentsViewModel : BaseViewModel
    {
        const short SWP_NOMOVE = 0x2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0x4;
        const int SWP_SHOWWINDOW = 0x0040;

        const short HWND_BOTTOM = 1;
        const int HWND_NOTOPMOST = -2;
        const int HWND_TOP = 0;
        const int HWND_TOPMOST = -1;

        const int SW_HIDE = 0;
        const int SW_SHOW = 1;

        const int GWL_STYLE = -16;
        const uint WS_BORDER = 0x00800000;
        const uint WS_DLGFRAME = 0x00400000;
        const uint WS_THICKFRAME = 0x00040000;
        const uint WS_CAPTION = WS_BORDER | WS_DLGFRAME;
        const uint WS_MINIMIZE = 0x20000000;
        const uint WS_MAXIMIZE = 0x01000000;
        const uint WS_SYSMENU = 0x00080000;
        const uint WS_VISIBLE = 0x10000000;
        const int WS_SIZEBOX = 0x00040000;

        const int GWL_EXSTYLE = -20;
        const uint WS_EX_APPWINDOW = 0x00040000;
        const uint WS_EX_NOACTIVATE = 0x08000000;


        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }


        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }


        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        static IntPtr hHook = IntPtr.Zero;
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private const int WH_MOUSE_LL = 14;



        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rectangle rect);
        public enum HookModes { None, G1000PFD, G1000MFD };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);



        static SolidColorBrush colActive = new SolidColorBrush(Colors.Orange);
        static SolidColorBrush colInActive = new SolidColorBrush(Colors.CornflowerBlue);
        public InstrumentsViewModel(MainWindowViewModel mainwindow)
        {
            this.MainWindowViewModel = mainwindow;
            this.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "default.profile");
            this.AddInstrumentCommand = new RelayCommand(p => 
            {
                this.AddInstrument();
            });
            this.RemoveInstrumentCommand = new RelayCommand(p => { this.RemoveInstrument(); }, p => { return this.SelectedIntrument != null; });
            this.GetHandleCommand = new RelayCommand(p =>
            { 
                FindHandle(p);
            });
            this.Instruments = new ObservableCollection<InstrumentModel>();


            this.LoadProfileCommand = new RelayCommand(p =>
            {
                OpenFileDialog f = new OpenFileDialog();
                f.Filter = "Profile|*.profile";
                if (f.ShowDialog() == true)
                {
                    this.Load(f.FileName);
                }

            });
            this.NewProfileCommand = new RelayCommand(p =>
            {
                SaveFileDialog f = new SaveFileDialog();
                f.Filter = "Profile|*.profile";
                f.RestoreDirectory = true;
                if (f.ShowDialog() == true)
                {
                    this.FileName = f.FileName;
                    //string json = JsonConvert.SerializeObject(this.Instruments);
                    //File.WriteAllText(f.FileName, json);

                }

            });
            this.SaveProfileCommand = new RelayCommand(p =>
            {
                try
                {
                    if(this.FileName != null)
                    {
                        this.SaveProfile(this.FileName);

                    }

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

            }, p => { return this.FileName != ""; });

            this.SaveAsProfileCommand = new RelayCommand(p =>
            {
                try
                {
                    SaveFileDialog f = new SaveFileDialog();
                    f.Filter = "Profile|*.profile";
                    if (f.ShowDialog() == true)
                    {
                        this.SaveProfile(f.FileName);
                        //string json = JsonConvert.SerializeObject(this.Instruments);
                        //File.WriteAllText(f.FileName, json);
                        //this.FileName = f.FileName;
                    }

                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

            }, p => { return this.FileName != ""; });


            this.ReadPosCommand = new RelayCommand(
                p =>
                {
                    Rectangle rect = new Rectangle();
                    InstrumentModel m = (p as InstrumentModel);
                    GetWindowRect(m.Handle, out rect);
                    m.PosX = Convert.ToInt32(rect.X);
                    m.PosY = Convert.ToInt32(rect.Y);
                    m.Width = Convert.ToInt32(rect.Width - rect.X);
                    m.Height = Convert.ToInt32(rect.Height - rect.Y);

                },
                //p => { return true; });
                p => { 
                    
                    if(p == null)
                    {
                        return false;
                    }
                    
                    return (p as InstrumentModel).Handle != IntPtr.Zero; });
        }

        private void SaveProfile(string fileName)
        {
            try
            {
                string json = JsonConvert.SerializeObject(this.Instruments);
                File.WriteAllText(this.FileName, json);
                this.FileName = fileName;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        static IntPtr HANDLE = IntPtr.Zero;
        static InstrumentModel ProfileModel = null;
        
        public IntPtr FindHandle(object ipm)
        {

            if (IntPtr.Zero == hHook)
            {
                ProfileModel = ipm as InstrumentModel;
                ProfileModel.ButtonBg = colActive;// new SolidColorBrush(Colors.Orange);

                //WinManViewModel.HookMode = HookModes.G1000PFD;
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    // App.Current.MainWindow.Cursor = System.Windows.Input.Cursors.Cross;
                    hHook = SetWindowsHookEx(WH_MOUSE_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
            return IntPtr.Zero;
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(MouseMessages.WM_RBUTTONDOWN == (MouseMessages)wParam)
            {
                UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;
                if (ProfileModel != null)
                {
                    //ProfileModel.Button.Background =
                    ProfileModel.ButtonBg = colInActive;
                }

            }
            else if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                POINT cusorPoint;
                bool ret = GetCursorPos(out cusorPoint);
                IntPtr winHandle = WindowFromPoint(cusorPoint);
                HANDLE = winHandle;
                //var HANDLE =   (int)winHandle;

                UnhookWindowsHookEx(hHook);
                hHook = IntPtr.Zero;
                if (ProfileModel != null)
                {
                        ProfileModel.Handle = HANDLE;
                        ProfileModel.ButtonBg = colInActive;
                        ProfileModel.Owner.SetPos(ProfileModel);
                        //MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.SelectedIntrument.Handle = HANDLE;
                        //MainWindow.Instance.MainWindowViewModel.InstrumentsProfileViewModel.OnPropertyChanged(nameof(SelectedIntrument));
                        Console.WriteLine("HANDLE: " + HANDLE.ToString());
                }

                //if (ProfileModel.Button != null)
                //{
                //    ProfileModel.Button.Background = new SolidColorBrush(Colors.LimeGreen);
                //    ProfileModel.Button = null;
                //}

                //Console.WriteLine("HookCallBack");
                //WinManagerManViewModel.HookMode = HookModes.None;

                // Here I do not use the GetActiveWindow(). Let's call the window you clicked "DesWindow" and explain my reason.
                // I think the hook intercepts the mouse click message before the mouse click message delivered to the DesWindow's 
                // message queue. The application came to this function before the DesWindow became the active window, so the handle 
                // abtained from calling GetActiveWindow() here is not the DesWindow's handle, I did some tests, and What I got is always 
                // the Form's handle, but not the DesWindow's handle. You can do some test too.

                //IntPtr handle = GetActiveWindow();


            }

            return  CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        public void SetPos(InstrumentModel ipm)
        {

            if (ipm.Handle != IntPtr.Zero)
            {
                    SetWindowPos(ipm.Handle, HWND_TOP, ipm.PosX, ipm.PosY, ipm.Width, ipm.Height, SWP.NOACTIVATE);
            }
        }
        public void SetStyle(InstrumentModel ipm)
        {
            //if (ipm.Handle != IntPtr.Zero)
            //{
            //    var currentStyle = GetWindowLong(ipm.Handle, GWL_STYLE).ToInt64();

            //    if (ipm.FrameVisible)
            //    {
            //        SetWindowLong(ipm.Handle, GWL_STYLE, (uint)(currentStyle | (WS_CAPTION | WS_SIZEBOX)));
            //    }
            //    else
            //    {
            //        SetWindowLong(ipm.Handle, GWL_STYLE, (uint)(currentStyle & ~(WS_CAPTION | WS_SIZEBOX)));
            //    }
                    


            //    //// HWND winHandle = (HWND)winId();
            //    //ShowWindow(ipm.Handle, SW_HIDE);
            //    //uint s = (uint)GetWindowLongPtr(ipm.Handle, GWL_EXSTYLE);
            //    ////SetWindowLong(ipm.Handle, GWL_EXSTYLE, s | WS_EX_NOACTIVATE | WS_EX_APPWINDOW);
            //    //// SetWindowLong(ipm.Handle, GWL_EXSTYLE, s | WS_ | WS_EX_APPWINDOW);
            //    //ShowWindow(ipm.Handle, SW_SHOW);
            //}
        }

        internal void SetFullFullScreen(InstrumentModel ipm)
        {

        }

        private void GetHandle(object p)
        {
            this.FindHandle(p as InstrumentModel);
        }

        private void RemoveInstrument()
        {
            throw new NotImplementedException();
        }

        private void AddInstrument()
        {
            InstrumentModel m = new InstrumentModel();
            m.InstrumentName = "Instrument";
            m.Handle = IntPtr.Zero;
            m.PosX = 0;
            m.PosY = 0;
            m.Width = 0;
            m.Height = 0;
            m.ButtonBg = colInActive;
            m.Owner = this;
            this.Instruments.Add(m);
        }

        public void Load(string lastProfileFile)
        {
            if (File.Exists(lastProfileFile))
            {

                try
                {
                    string json = File.ReadAllText(lastProfileFile);
                    List<InstrumentModel> list = JsonConvert.DeserializeObject<List<InstrumentModel>>(json);

                    foreach (InstrumentModel model in list)
                    {
                        model.Handle = IntPtr.Zero;
                        model.Owner = this;
                    }

                    this.Instruments.Clear();
                    this.Instruments = new ObservableCollection<InstrumentModel>(list);
                    this.FileName = lastProfileFile;


                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else
            {
                System.Windows.MessageBox.Show($"Could not find the last profile file: {lastProfileFile}");
            }

        }


        private InstrumentModel fSelecteInstrument;
        public InstrumentModel SelectedIntrument
        {
            get { return fSelecteInstrument; }

            set
            {
                this.fSelecteInstrument = value;
                this.OnPropertyChanged();
            }
        }


        private ObservableCollection<InstrumentModel> fInstruments;

        public ObservableCollection<InstrumentModel> Instruments
        {
            get { return fInstruments; }
            set
            {
                fInstruments = value;
                this.OnPropertyChanged();
            }
        }

        public RelayCommand NewProfileCommand { get; }
        public RelayCommand FindHandleCommand { get; }
        public MainWindowViewModel MainWindowViewModel { get; }
        public RelayCommand AddInstrumentCommand { get; }
        public RelayCommand RemoveInstrumentCommand { get; }
        public RelayCommand GetHandleCommand { get; }
        public RelayCommand LoadProfileCommand { get; }
        public RelayCommand SaveProfileCommand { get; }
        public RelayCommand SaveAsProfileCommand { get; }
        public RelayCommand ReadPosCommand { get; }

        private string fFileName;

        public string FileName
        {
            get { return this.MainWindowViewModel.AppSettingsViewModel.LastProfileFile; }
            set
            {
                fFileName = value;
                this.OnPropertyChanged();
                this.MainWindowViewModel.AppSettingsViewModel.LastProfileFile = value;
                this.MainWindowViewModel.AppSettingsViewModel.SaveSettings();
            }
        }

    }
}
