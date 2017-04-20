using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveMusicCollection
{
	class Program
	{
		private static readonly string ROOT = @"D:\[1] Music";
		private static readonly string FILE = "list_of_files.txt";
		private static readonly bool WRITE_TO_FILE = true;
		private static readonly string[] FILE_PATTERN = new string[] { "*.mp3", "*.flac" };
		private static StringBuilder stack;
		private static int FileCounter;

		static void Main(string[] args)
		{
			string[] folders = Directory.GetDirectories(ROOT, "*", SearchOption.AllDirectories);
			stack = new StringBuilder(10000);

			foreach (var item in folders)
			{
				if (Directory.GetDirectories(item).Count() == 0)
					PrintFiles(item);
			}


			Print($"folders: {folders.Count()}");
			Print($"files: {FileCounter}");

			if (WRITE_TO_FILE)
				WriteToFile();

			Console.WriteLine("Konec programu");
			Console.ReadLine();
		}

		private static void PrintFiles(string item)
		{
			Print($"--- album: {item}");
			var files = Directory.EnumerateFiles(item, "*.*").ToList().Where(f => f.EndsWith("mp3") || f.EndsWith("flac"));

			foreach (var file in files)
			{
				Print($"\t: { Path.GetFileName(file) }");
				FileCounter++;
			}
		}

		private static void Print(string row)
		{
			if (WRITE_TO_FILE)
				stack.Append(row + Environment.NewLine);
			else
				Console.WriteLine(row);
		}

		private static void WriteToFile()
		{
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(FILE))
				file.WriteLine(stack.ToString());

		}
	}
}
