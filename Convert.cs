using System;
using System.IO;
using System.Text;
//using Excel = Microsoft.Office.Interop.Excel;

namespace TxtToCsv
{
    public class Convert
    {
        // https://devblogs.microsoft.com/buckh/converting-a-text-file-from-one-encoding-to-another/
        public static void ConvertFileEncoding(string sourcePath, string destPath, string srcEncoding, string dstEncoding) //(String[] args)
        {
            // Print a simple usage statement if the number of arguments is incorrect.
            // if (args.Length != 4)
            // {
            //     Console.WriteLine("Usage: {0} inputFile outputFile inputEncoding outputEncoding",
            //                       Path.GetFileName(Environment.GetCommandLineArgs()[0]));
            //     Environment.Exit(1);
            // }
            Encoding sourceEncoding = Encoding.GetEncoding(srcEncoding);
            Encoding destEncoding = Encoding.GetEncoding(dstEncoding);
            String parent = Path.GetDirectoryName(Path.GetFullPath(destPath));
            if (!Directory.Exists(parent))
            {
                Directory.CreateDirectory(parent);
            }
            // If the source and destination encodings are the same, just copy the file.
            if (sourceEncoding == destEncoding)
            {
                File.Copy(sourcePath, destPath, true);
                return;
            }
            // Convert the file.
            String tempName = null;
            try
            {
                tempName = Path.GetTempFileName();
                using (StreamReader sr = new StreamReader(sourcePath, sourceEncoding, false))
                {
                    using (StreamWriter sw = new StreamWriter(tempName, false, destEncoding))
                    {
                        int charsRead;
                        char[] buffer = new char[128 * 1024];
                        while ((charsRead = sr.ReadBlock(buffer, 0, buffer.Length)) > 0)
                        {
                            sw.Write(buffer, 0, charsRead);
                        }
                    }
                }
                File.Delete(destPath);
                File.Move(tempName, destPath);
            }
            finally
            {
                File.Delete(tempName);
            }
        }

        //https://stackoverflow.com/questions/29086612/convert-text-file-to-excel
        public static void ConvertToCsv(string[] sourcefile, string destfile)
        {
            using (StreamWriter csvfile = new StreamWriter(destfile))
            {
                string[] lines, cells;
                lines = sourcefile; //File.ReadAllLines(sourcefile);

                for (int i = 0; i < lines.Length; i++)
                {
                    cells = lines[i].Split(new Char[] { '\t', ';', ' ' });
                    for (int j = 0; j < cells.Length; j++)
                        csvfile.Write(cells[j] + ",");
                    csvfile.WriteLine();
                }
            }
        }

        public static void ConvertToXlsx(string sourcefile, string destfile)
        {
            // int i, j;
            // Excel.Application xlApp;
            // Excel.Workbook xlWorkBook;
            // Excel._Worksheet xlWorkSheet;
            // object misValue = System.Reflection.Missing.Value;
            // string[] lines, cells;
            // lines = File.ReadAllLines(sourcefile);
            // xlApp = new Excel.Application();
            // xlApp.DisplayAlerts = false;
            // xlWorkBook = xlApp.Workbooks.Add();
            // xlWorkSheet = (Excel._Worksheet)xlWorkBook.ActiveSheet;
            // for (i = 0; i < lines.Length; i++)
            // {
            //     cells = lines[i].Split(new Char[] { '\t', ';' });
            //     for (j = 0; j < cells.Length; j++)
            //         xlWorkSheet.Cells[i + 1, j + 1] = cells[j];
            // }
            // xlWorkBook.SaveAs(destfile, Excel.XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            // xlWorkBook.Close(true, misValue, misValue);
            // xlApp.Quit();
        }
    }
}