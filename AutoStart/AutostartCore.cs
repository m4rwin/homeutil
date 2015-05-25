using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace AutoStart
{
    public class AutostartCore
    {
        public static string GetVersion()
        {
            return "0.1";
        }

        private static RegistryKey onStartup1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);

        public static bool SetOnStartup(bool run)
        {
            if (run)
            {
                try
                {
                    onStartup1.SetValue(Application.ProductName, Application.ExecutablePath.ToString());
                }
                catch { return false; }
                return true;
            }
            else
            {
                try
                {
                    onStartup1.DeleteValue(Application.ProductName);
                }
                catch { return false; }
                
                return true;
            }
        }

        public static bool IsOnStartup()
        {
            if (onStartup1.GetValue(Application.ProductName) == null) return false;

            if (!System.IO.File.Exists(onStartup1.GetValue(Application.ProductName).ToString()))
            {
                onStartup1.SetValue(Application.ProductName, Application.ExecutablePath.ToString());
            }

            return true;
        }
    }
}
