using KeyLogger;
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
using System.Windows.Threading;
using MyKey = KeyLogger.Key;

namespace MarwinTool.Windows
{
    /// <summary>
    /// Interakční logika pro KeyLoggerWindow.xaml
    /// </summary>
    public partial class KeyLoggerWindow : Window
    {
        private MainWindow mainWindow;
        public DispatcherTimer tmrKeyLogger;

        public KeyLoggerWindow(MainWindow mw)
        {
            InitializeComponent();

            mainWindow = mw;

            tmrKeyLogger = new DispatcherTimer();
            tmrKeyLogger.Interval = new TimeSpan(1L);
            tmrKeyLogger.Tick += tmrKeyLogger_Tick;
        }

        #region Timer Events
        string text = string.Empty, title = string.Empty, previousTitle = string.Empty;
        void tmrKeyLogger_Tick(object sender, EventArgs e)
        {
            title = ActiveWindow.GetWindowTitle();
            if (!title.Equals(previousTitle))
            {
                tbxKeyLogger.Text += Environment.NewLine + title + ": ";
                previousTitle = title;
            }


            text = MyKey.GetBuffKeys();
            tbxKeyLogger.Text += text;
            tbxKeyLogger.ScrollToEnd();

        }
        #endregion

        #region Events
        private double left = 0, top = 0;
        private void Window_Activated(object sender, EventArgs e)
        {
            left = Application.Current.MainWindow.Left;
            top = Application.Current.MainWindow.Top;

            this.Top = top;
            this.Left = left - this.Width - 10;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            mainWindow.chbxLogKeys.IsChecked = false;
        }
        #endregion

        

        
    }
}
