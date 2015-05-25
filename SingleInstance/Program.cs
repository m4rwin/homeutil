using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace SingleInstance
{
    static class Program
    {
        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool createdNew;
            Mutex appMutex = new Mutex(true, Application.ProductName, out createdNew);

            if (createdNew)
                Application.Run(new Form1());
            else
                MessageBox.Show("Tato aplikace může být spuštěna pouze jednou!");
        }
    }
}
