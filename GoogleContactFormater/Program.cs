using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleContactFormater
{
    class Program
    {
        private static string FileName;
        private static List<string> NewFile;
        static void Main(string[] args)
        {
            Console.WriteLine("|-------------------|");
            Console.WriteLine("|    G O O G L E    |");
            Console.WriteLine("|  C O N T A C T S  |");
            Console.WriteLine("|    R E A D E R    |");
            Console.WriteLine("|       V 1.1       |");
            Console.WriteLine("|     2014/12/03    |");
            Console.WriteLine("| hromek@hotmail.cz |");
            Console.WriteLine("|-------------------|");
            Console.WriteLine("");
            Console.Write("Zadej jmeno souboru: ");
            FileName = Console.ReadLine();
            NewFile = new List<string>(1000);

            bool c = ReadFile(FileName);

            if (c)
            {
                Console.WriteLine("Vytvarim novy soubor...");
                bool b = CreateNewFile();
                if (b)
                {
                    Console.WriteLine("Hotovo.");
                }
                else
                {
                    Console.WriteLine("Nastala chyba.");
                }
            }
            else { Console.WriteLine("Nastala chyba."); }
            Console.ReadLine();
        }

        private static List<string> Lines = new List<string>(200000);
        private static bool ReadFile(string FileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(FileName))
                {
                    string line;
                    do
                    {
                        line = sr.ReadLine();
                        Lines.Add(line);
                    }
                    while (line != null);
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Soubor se nepodarilo precist:");
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        private static string RemoveNewLine(string text)
        {
            string result = string.Empty;

            foreach (char c in text)
            {
                if (c == '\r')
                {
                    result += " ";
                    continue;
                }
                else if (c == '\n')
                {
                    result += " ";
                    continue;
                }
                else { result += c; }
            }

            return result.Trim() + "|";
        }

        private static bool CreateNewFile()
        {
            string NewFileName = string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd"), FileName);
            string[] arr = Lines[0].Split(',');
            int NoOfColumn = arr.Length;

            int counter = 0;
            string newRow = string.Empty;
            string[] array = new string[10000];
            foreach (string item in Lines)
            {
                if (string.IsNullOrEmpty(item)) { counter++; continue; }
                array = item.Split(',');

                newRow += RemoveNewLine(item);

                if (array.Length > 1)
                {
                    counter += array.Length;
                }
                
                if (counter >= NoOfColumn)
                {
                    NewFile.Add(newRow);
                    counter = 0;
                    newRow = string.Empty;
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(NewFileName,false, Encoding.UTF8))
                {
                    foreach (string item in NewFile)
                    {
                        writer.WriteLine(item);
                        //writer.WriteLine("-----------------------");
                        //writer.WriteLine(Environment.NewLine);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Vytvareni souboru selhalo:");
                Console.WriteLine(e.ToString());
                File.Delete(NewFileName);
                return false;
            }
            Console.WriteLine("Soubor '" +NewFileName+ "' vytvoren." );
            return true;
        }
    }
}
