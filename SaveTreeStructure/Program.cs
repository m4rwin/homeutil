using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace SaveTreeStructure
{
    public class Program
    {
        public static string Recipient { get; set; }
        public static DriveInfo[] HardDrive { get; set; }
        public static List<string> TreeStructure { get; set; }
        public static string[] BannedFolders = { "C:\\Windows", "C:\\Program Files", "C:\\Program Files (x86)", "C:\\$Recycle.Bin", "C:\\cygwin" };
        public static string[] BannedWords = { "appdata", "all users", "microsoft", "drivers", "programdata", "dropbox", "onedrive"};

        static void Main(string[] args)
        {
            TreeStructure = new List<string>(50000);
            TreeStructure.Add(string.Format("*** SaveTreeStructure ***"));
            TreeStructure.Add(string.Format("Datum zpracovani: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
            TreeStructure.Add(string.Format("Jmeno pocitace: {0}", Environment.MachineName));
            TreeStructure.Add(string.Format(""));

            Console.Write("Zadej emailovou adresu pro zaslani vysledku: ");
            Recipient = Console.ReadLine();

            HardDrive = DriveInfo.GetDrives();

            PrintDriveInfo();

            foreach (DriveInfo drive in HardDrive)
            {
                Console.Write(string.Format("Chces zpracovat disk {0} [y/n]: ", drive.Name));
                string response = Console.ReadLine();

                if (response.Equals("y"))
                {
                    Console.WriteLine(string.Format("Zpracovavam disk {0} ...", drive.Name));
                    string[] root = Directory.GetDirectories(drive.Name);
                    foreach (string folder in root)
                    {
                        ProcessFolder(folder);
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Disk {0} nebyl zpracovan.", drive.Name));
                }
            }

            string filename = string.Format("[{0}] Tree_Structure.txt", DateTime.Now.ToString("yyyy-MM-dd"));
            File.AppendAllLines(filename, TreeStructure);
            Console.WriteLine(string.Format("Vysledek byl ulozen do souboru {0}.", filename));

            Console.WriteLine("Sending email...");
            SendFileByEmail(filename);
            Console.WriteLine("Email sent.");

            Console.WriteLine("\nEnd of process.");
            Console.ReadLine();
        }

        private static void PrintDriveInfo()
        {
            List<DriveInfo> newListOfDrive = new List<DriveInfo>();
            foreach (DriveInfo d in HardDrive)
            {
                if (d.IsReady)
                {
                    Console.WriteLine(string.Format("[{0}] {1} {2}", d.DriveType, d.Name, d.VolumeLabel));
                    if (d.DriveType == DriveType.Fixed) { newListOfDrive.Add(d); }
                }
                else
                {
                    Console.WriteLine(string.Format("{0} is not ready.", d.Name));
                }
            }
            HardDrive = newListOfDrive.ToArray();
        }

        private static void ProcessFolder(string folder)
        {
            if (NotProcessing(folder)) 
            {
                TreeStructure.Add(folder + " nebyla dale zpracovavana.");
                return; 
            }
            
            TreeStructure.Add(folder);

            string[] folders;

            try
            {
                folders = Directory.GetDirectories(folder);
            }
            catch (Exception) { return; }


            if (folders.Length > 0)
            {
                foreach (string f in folders)
                {
                    ProcessFolder(f);
                }
            }
        }

        private static bool NotProcessing(string folder)
        {
            foreach (string banned in BannedFolders)
            {
                if (folder.Equals(banned)) { return true; }
            }

            foreach (string banned2 in BannedWords)
            {
                if (folder.ToLower().Contains(banned2)) { return true; }
            }

            return false;
        }

        private static void SendFileByEmail(string filename)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;

            mail.IsBodyHtml = true;
            mail.From = new MailAddress("hromek@hotmail.cz");
            mail.To.Add(Recipient);
            mail.Subject = string.Format("Záloha adresářové struktury - {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            mail.Body = "Zpráva zaslána programem SaveTreeStructure [Martin Hromek, 2014]<br />V příloze je přiložen soubor s výpisem adresářové struktury.<br /><br />Přeji hezký den<br />Martin Hromek";
            

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(filename);
            mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.Credentials = new System.Net.NetworkCredential("hromek@hotmail.cz", "marwinfreeman1");
            SmtpServer.EnableSsl = true;

            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
