using AutoStart;
using MarwinTool.Windows;
using Microsoft.VisualBasic.Devices;
using MUtility;
using ShutDown;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SendingSMS;
using System.Windows.Forms;
using WIN = System.Windows;

namespace MarwinTool
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region constant
        private const string AppName = "Marwin`s Tool";
        private const string AppVersion = "1.5";
        #endregion

        #region properties
        // Ping method
        DispatcherTimer tmrNetworkConnection;

        // Shutdown OR Logoff
        DispatcherTimer tmrAction;
        TimeSpan Countdown;

        // Time of Application running
        DateTime ApplicationStart;
        DispatcherTimer tmrApplicationUpdate;

        DispatcherTimer tmrKeyCounter;

        // KeyLoggerWindow instance
        KeyLoggerWindow KeyLogWin;

        Data AppData;

        // Notification icon
        NotifyIcon NotifyIcone;
        #endregion

        #region c-tor
        public MainWindow()
        {
            InitializeComponent();
            MyInit();
            CreateMyNotifyIcon();
        }
        #endregion 

        #region myinit
        private void MyInit()
        {
            AppData = new Data();

            this.Title = string.Format("{0} {1}", AppName, AppVersion);
            tmrNetworkConnection = new DispatcherTimer();
            tmrNetworkConnection.Tick += tmrNetworkConnection_Tick;
            tmrNetworkConnection_Tick(null, null);
            tmrNetworkConnection.Interval = new TimeSpan(0, 0, 10);
            tmrNetworkConnection.Start();

            tmrKeyCounter = new DispatcherTimer();
            tmrKeyCounter.Tick += tmrKeyCounter_Tick;
            tmrKeyCounter.Interval = new TimeSpan(0,0,1);
            tmrKeyCounter.Start();

            

            if (AutostartCore.IsOnStartup())
            {
                lblAutostartInfo.Content = string.Format("[{0}] Autostart is set.", AutostartCore.GetVersion());
                lblAutostartInfo.Foreground = Brushes.DarkGreen;
            }
            else { lblAutostartInfo.Content = "Autostart is not set."; lblAutostartInfo.Foreground = Brushes.DarkRed; }

            tmrAction = new DispatcherTimer();
            tmrAction.Interval = new TimeSpan(0, 0, 1);
            tmrAction.Tick += tmrAction_Tick;

            ApplicationStart = DateTime.Now;
            tmrApplicationUpdate_Tick(null, null);
            tmrApplicationUpdate = new DispatcherTimer();
            tmrApplicationUpdate.Interval = new TimeSpan(0, 1, 13);
            tmrApplicationUpdate.Tick += tmrApplicationUpdate_Tick;
            tmrApplicationUpdate.Start();

            KeyLogWin = new KeyLoggerWindow(this);
        }

        private void CreateMyNotifyIcon()
        {
            NotifyIcone = new NotifyIcon();
            NotifyIcone.Icon = new System.Drawing.Icon("spy.ico");
            NotifyIcone.Visible = true;
            this.WindowState = WIN.WindowState.Minimized;
            NotifyIcone.DoubleClick +=
                delegate(object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = System.Windows.WindowState.Normal;
                };
            Window_StateChanged(null, null);
        }
        #endregion

        #region button events
        private void btnSetAutostart_Click(object sender, RoutedEventArgs e)
        {
            if (AutostartCore.IsOnStartup())
            {
                lblAutostartInfo.Content = "Autostart is already set.";
                return;
            }
            else 
            {
                bool result = AutostartCore.SetOnStartup(true);

                if (result) { lblAutostartInfo.Content = "Autostart set correctelly."; lblAutostartInfo.Foreground = Brushes.DarkGreen; }
                else { lblAutostartInfo.Content = "Set autostart failed."; lblAutostartInfo.Foreground = Brushes.DarkRed; }
            }
        }

        private void btnRemoveAutostart_Click(object sender, RoutedEventArgs e)
        {
            if (AutostartCore.IsOnStartup())
            {
                bool result = AutostartCore.SetOnStartup(false);

                if (result) { lblAutostartInfo.Content = "Autostart removed correctelly."; lblAutostartInfo.Foreground = Brushes.DarkGreen; }
                else { lblAutostartInfo.Content = "Remove autostart failed."; lblAutostartInfo.Foreground = Brushes.DarkRed; }
            }
            else
            {
                lblAutostartInfo.Content = "Autostart is not set.";
                lblAutostartInfo.Foreground = Brushes.DarkRed;
            }
        }

        private void btnPing_Click(object sender, RoutedEventArgs e)
        {
            tmrNetworkConnection.Stop();
            tmrNetworkConnection_Tick(null, null);
            tmrNetworkConnection.Start();
        }

        private void btnDoItNow_Click(object sender, RoutedEventArgs e)
        {
            ShutdownCore SDcore = new ShutdownCore();
            SDcore.LogOff();
        }
         
        private void btnCountingDown_Click(object sender, RoutedEventArgs e)
        {
            short minutes;
            if (tmrAction.IsEnabled)
            {
                tmrAction.Stop();
                lblMinutes.Foreground = Brushes.Black;
                btnCountingDown.Foreground = Brushes.Black;
                this.Title = string.Format("{0} {1}", AppName, AppVersion);
                btnCountingDown.Content = "Start";

                if (Int16.TryParse(txbMinutes.Text, out minutes)) { Countdown = new TimeSpan(0, minutes, 0); lblMinutes.Content = string.Format("{0}min. {1}sec.", Countdown.Minutes, Countdown.Seconds); }
                else { lblMinutes.Content = "???"; }
            }
            else
            {
                btnCountingDown.Content = "Stop";
                if (Int16.TryParse(txbMinutes.Text, out minutes)) { Countdown = new TimeSpan(0, minutes, 0); tmrAction.Start(); }
                else { btnCountingDown.Content = "Start"; lblMinutes.Content = "???"; }
            }
        }

        private void btnGetRAM_Click(object sender, RoutedEventArgs e)
        {
            ComputerInfo info = new ComputerInfo();
            lblRAM.Content = string.Format("RAM: {0} MB / {1} MB", (info.AvailablePhysicalMemory / 1024 / 1024).ToString("G"), (info.TotalPhysicalMemory / 1024 / 1024).ToString("G"));

            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000); // wait a second to get a valid reading
            lblCPU.Content = "CPU: " + (int)cpuCounter.NextValue() + "%";
        }

        private void SendSMS_Click(object sender, RoutedEventArgs e)
        {
            lblSMSresult.Content = "---";
            bool b = SMS.sendEmail(string.Format("00{0}@sms.cz.o2.com", tbxPhoneNumber.Text), tbxSMStext.Text);
            if (b) { lblSMSresult.Content = "Sent."; lblSMSresult.Foreground = new SolidColorBrush(Colors.DarkGreen); }
            else { lblSMSresult.Content = "Err."; lblSMSresult.Foreground = new SolidColorBrush(Colors.DarkRed); }
        }
        #endregion

        #region checkbox events
        private void chbxLogKeys_Checked(object sender, RoutedEventArgs e)
        {
            if (chbxLogKeys.IsChecked.Value)
            {
                KeyLogWin.Show();
                KeyLogWin.tmrKeyLogger.Start();
            }
        }

        private void chbxLogKeys_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!chbxLogKeys.IsChecked.Value)
            {
                KeyLogWin.Hide();
                KeyLogWin.tmrKeyLogger.Stop();
            }
        }
        #endregion

        #region Timer Events
        private void tmrAction_Tick(object sender, EventArgs e)
        {
            Countdown = Countdown.Add(new TimeSpan(0, 0, -1));
            lblMinutes.Content = string.Format("{0}min. {1}sec.", Countdown.Minutes, Countdown.Seconds);

            if (Countdown.TotalSeconds % 2 == 0)
            {
                lblMinutes.Foreground = Brushes.DarkRed;
                btnCountingDown.Foreground = Brushes.DarkRed;
                this.Title = string.Format("{0} {1} /", AppName, AppVersion);
            }
            else
            {
                lblMinutes.Foreground = Brushes.Black;
                btnCountingDown.Foreground = Brushes.Black;
                this.Title = string.Format("{0} {1} \\", AppName, AppVersion);
            }

            if (Countdown.TotalMinutes == 0)
            {
                tmrAction.Stop();
                Sound.MakeSound(Properties.Resources.beep_13);
                WIN.MessageBox.Show("Do Action...");
            }
        }

        bool previousPing = true;
        private void tmrNetworkConnection_Tick(object sender, EventArgs e)
        {
            if (MUtility.Network.Ping())
            {
                AppData.LastOnlineTime = DateTime.Now;
                lblInternetConnectionInfo.Content = string.Format("Internet connection is available. [{0}]", AppData.LastOnlineTime);
                lblInternetConnectionInfo.Foreground = Brushes.DarkGreen;
                previousPing = true;
            }
            else
            {
                if (previousPing)
                {
                    previousPing = false;
                    return;
                }

                // testovaci komentar
                // testovaci komentar
                string q = "abc";
                Sound.MakeSound(Properties.Resources.beep_13);
                lblInternetConnectionInfo.Content = string.Format("Internet connection is NOT available. [{0}]", DateTime.Now);
                lblInternetConnectionInfo.Foreground = Brushes.DarkRed;
            }
        }

        private void tmrApplicationUpdate_Tick(object sender, EventArgs e)
        {
            TimeSpan during = DateTime.Now - ApplicationStart;
            //lblRunningInfo.Content = string.Format("Application start at: {0} (during: {1})", ApplicationStart, during.ToString2());
            lblRunningInfo.Content = string.Format("Application start at: {0} (running: {1}d {2}h {3}m)", ApplicationStart, during.Days, during.Hours, during.Minutes);
        }
        #endregion

        #region app events
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                this.Hide();
                NotifyIcone.BalloonTipText = "Application was minimalized to system tray.";
                NotifyIcone.ShowBalloonTip(2000);
            }
            //base.OnStateChanged(e);
        }
        #endregion

        #region expander
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions[6].Height = new GridLength(MainGrid.RowDefinitions[6].Height.Value + 40);
            this.Height += 40;
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions[6].Height = new GridLength(MainGrid.RowDefinitions[6].Height.Value - 40);
            this.Height -= 40;
        }

        private void Expander_Collapsed_1(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions[6].Height = new GridLength(MainGrid.RowDefinitions[6].Height.Value - 40);
            this.Height -= 40;
        }

        private void Expander_Expanded_1(object sender, RoutedEventArgs e)
        {
            MainGrid.RowDefinitions[6].Height = new GridLength(MainGrid.RowDefinitions[6].Height.Value + 40);
            this.Height += 40;
        }
        #endregion 

        private void cmbProcesses_Initialized(object sender, EventArgs e)
        {
            Process[] allProcceses = Process.GetProcesses();
            List<string> processes = new List<string>();
            foreach (Process p in allProcceses) { processes.Add(p.ProcessName + " - " + (p.PrivateMemorySize64 / 1024 / 1024).ToString("G") + " MB"); }
            processes.Sort();

            cmbProcesses.ItemsSource = processes;
        }






        private static int LeftMouseClick = 0;
        private static int RightMouseClick = 0;
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LeftMouseClick++;
            this.Title = string.Format("{0} {1} | mouse [l:{2}, r:{3}]", AppName, AppVersion, LeftMouseClick, RightMouseClick);
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            RightMouseClick++;
            this.Title = string.Format("{0} {1} | mouse [l:{2}, r:{3}]", AppName, AppVersion, LeftMouseClick, RightMouseClick);
        }

        private static int keyCounter;
        private static string text;
        private void tmrKeyCounter_Tick(object sender, EventArgs e)
        {
            text = KeyLogger.Key.GetBuffKeys();
            keyCounter += text.Length;
            this.Title = string.Format("{0} {1} | mouse [l:{2}, r:{3}] key: {4}", AppName, AppVersion, LeftMouseClick, RightMouseClick, keyCounter);
        }

        #region WatchObjects
        public void WatchObject(object obj)
        {
            INotifyPropertyChanged watchableObject = obj as INotifyPropertyChanged;
            if (watchableObject != null)
            {
                //watchableObject.PropertyChanged += new PropertyChangedEventHandler(data_PropertyChanged);
            }
        }
        #endregion

        public void data_PropertyChanged(object sender, PropertyChangingEventArgs e)
        {
            WIN.MessageBox.Show("---");
        }
    }
}
