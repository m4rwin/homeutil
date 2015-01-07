using AutoStart;
using MarwinTool.Windows;
using MUtility;
using ShutDown;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace MarwinTool
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region constant
        private const string AppName = "Marwin`s Tool";
        private const string AppVersion = "1.4";
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

        // KeyLoggerWindow instance
        KeyLoggerWindow KeyLogWin;
        #endregion

        #region c-tor
        public MainWindow()
        {
            InitializeComponent();
            MyInit();
        }
        #endregion 

        #region myinit
        private void MyInit()
        {
            this.Title = string.Format("{0} {1}", AppName, AppVersion);
            tmrNetworkConnection = new DispatcherTimer();
            tmrNetworkConnection.Tick += tmrNetworkConnection_Tick;
            tmrNetworkConnection_Tick(null, null);
            tmrNetworkConnection.Interval = new TimeSpan(0, 0, 10);
            tmrNetworkConnection.Start();
            

            if (AutostartCore.IsOnStartup())
            {
                lblAutostartInfo.Content = "Autostart is set.";
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
                MessageBox.Show("Do Action...");
            }
        }

        bool previousPing = true;
        private void tmrNetworkConnection_Tick(object sender, EventArgs e)
        {
            if (Network.Ping())
            {
                lblInternetConnectionInfo.Content = string.Format("Internet connection is available. [{0}]", DateTime.Now);
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
                int i = 99;
                Sound.MakeSound(Properties.Resources.beep_13);
                lblInternetConnectionInfo.Content = string.Format("Internet connection is NOT available. [{0}]", DateTime.Now);
                lblInternetConnectionInfo.Foreground = Brushes.DarkRed;
            }
        }

        private void tmrApplicationUpdate_Tick(object sender, EventArgs e)
        {
            TimeSpan during = DateTime.Now - ApplicationStart;
            //lblRunningInfo.Content = string.Format("Application start at: {0} (during: {1})", ApplicationStart, during.ToString2());
            lblRunningInfo.Content = string.Format("Application start at: {0} (during: {1})", ApplicationStart, during);
        }
        #endregion
    }
}
