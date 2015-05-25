using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SendingSMS
{
    public class SMS
    {
        public static bool sendEmail(string to, string msg_body)
        {
            try
            {
                // přihlášení se k smtp od google gmail
                var client = new SmtpClient("smtp.live.com", 587)
                {
                    Credentials = new NetworkCredential("hromek@hotmail.cz", "marwinfreeman1"),
                    EnableSsl = true,
                    //UseDefaultCredentials  = true
                };

                client.SendCompleted += client_SendCompleted;

                // odeslání emailu (od koho, komu, předmět, zpráva)
                client.Send("hromek@hotmail.cz", to, "m4rwin tool", msg_body);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string s = "";
        }
    }
}
