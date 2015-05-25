using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendingSMS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Vodafone 
            // Nejprve si musíte mobilní e-mailovou adresu zdarma aktivovat. 
            // Přes SMS: Pošlete zprávu ve tvaru "EMAILZAP jméno" na číslo 2255, kde jako jméno zadáte své jméno či přezdívku (pozor, dost přezdívek už je zabraných!). 
            // Nyní budete mít e-mail ve tvaru jmeno@vodafonemail.cz.

            // T-Mobile 
            // Přihlásíte se do t-zones. Klepnete na poslat SMS. Vlevo vyberete Postm@il SMS a přidáte emailovou schránku, do které když vám přijde email tak se vám přepošle na mobil. 
            // (doporučuju vytvořit novou)

            // O2 
            // Pošlete zprávu na tento email 00420xxxxxxxx­x@sms.cz.o2.com, kde za xxxxxxx dosadíte telefonní číslo. (mají to nejjednodušší :D)

            // priklad
            SMS.sendEmail("00420775656055@sms.cz.o2.com", "Zdar vole! Cumis co? Zorro.");
        }

        /// <summary>
        /// Odešle email na zadanou emailovou adresu.
        /// </summary>
        /// <param name="to">Email příjemce</param>
        /// <param name="msg_body">Zpráva</param>
        /// <returns>True pokud vše proběhne v pořádku, false pokud nastane chyba</returns>
        
    }
}
