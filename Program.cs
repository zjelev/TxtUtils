using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TxtUtils
{
    public class Program
    {
        public static Encoding srcEncoding = Encoding.GetEncoding("windows-1251");
        public static Encoding destEncoding = Encoding.UTF8;
        static Stopwatch sw = new Stopwatch();

        static void Main(string[] args)
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string[] directories = Directory.GetDirectories(currentDirectory);

            string[] textFilesInCurrentDir = Directory.GetFiles(currentDirectory, "Par*");
            string[] sapFilesInCurrentDir = Directory.GetFiles(currentDirectory, "SAP_??????.txt");

            List<string> textFiles = new List<string>();
            List<string> sapFiles = new List<string>();

            textFiles.AddRange(textFilesInCurrentDir);
            sapFiles.AddRange(sapFilesInCurrentDir);

            foreach (var dir in directories)
            {
                string dirName = new DirectoryInfo(dir).Name;
                if (dirName != "bin" && dirName != ".vscode" && dirName != "obj")
                {
                    string[] textFilesInSubFolder = Directory.GetFiles(dir, "Par*");
                    string[] sapFilesInSubFolder = Directory.GetFiles(dir, "SAP_??????.txt");
                    textFiles.AddRange(textFilesInSubFolder);
                    sapFiles.AddRange(sapFilesInSubFolder);
                }
            }
            int textCounter = 0;

            foreach (var file in textFiles)
            {
                try
                {
                    string[] rows = TxtToCsv.Parag(file);
                    if (rows.Length == 38)
                    {
                        string month = rows[0].Substring(75, 2);
                        string year = rows[0].Substring(78, 4);
                        var dirName = new DirectoryInfo(file).Parent.Name;
                        var parentDir = Path.GetDirectoryName(file);
                        if (parentDir.EndsWith("TxtUtils"))
                        {
                            File.WriteAllLines(file.Remove(file.Length - 4) + "-" + year + "-" + month + ".csv", rows, destEncoding);
                        }
                        else
                        {
                            File.WriteAllLines(parentDir + Path.DirectorySeparatorChar + "Parag" + "-" + dirName + "-" + year + "-" + month + ".csv", rows, destEncoding);
                        }
                        textCounter++;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            int sapCounter = 0;
            foreach (var file in sapFiles)
            {
                try
                {
                    string[] rows = TxtToCsv.Sap(file);
                    File.WriteAllLines(file.Remove(file.Length - 4) + ".csv", rows, destEncoding);
                    sapCounter++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine($"{textCounter} of {textFiles.Count} Paragraph files and {sapCounter} of {sapFiles.Count} SAP files converted in {sw.ElapsedMilliseconds} ms. Press any key to exit");
            Console.ReadKey();

            var weightNotes = Directory.EnumerateFiles(currentDirectory, "*.txt", SearchOption.TopDirectoryOnly)
                .Where(filename => !Path.GetFileName(filename).StartsWith("SAP") && !Path.GetFileName(filename).StartsWith("ExcludesEGN"));

            foreach (var weightNote in weightNotes.ToList())
            {
                try
                {
                    string rows = WeightNote.TranslateToEn(weightNote);
                    File.WriteAllText(weightNote.Remove(weightNote.Length - 4) + "_EN.txt", rows, destEncoding);
                    // File.Delete(weightNote);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
