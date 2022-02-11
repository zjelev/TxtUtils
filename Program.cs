using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace TxtToCsv {
    class Program {
        private static Encoding srcTedEncoding = Encoding.GetEncoding("windows-1251");
        private static Encoding destEncoding = Encoding.UTF8;
        static void Main(string[] args) {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string currentDirectory = Directory.GetCurrentDirectory();
            string[] directories = Directory.GetDirectories(currentDirectory);

            string[] tedFilesInCurrentDir = Directory.GetFiles(currentDirectory, "*.ted");
            string[] sapFilesInCurrentDir = Directory.GetFiles(currentDirectory, "SAP_??????.txt");
            List<string> tedFiles = new List<string>();
            List<string> sapFiles = new List<string>();

            tedFiles.AddRange(tedFilesInCurrentDir);
            sapFiles.AddRange(sapFilesInCurrentDir);

            foreach (var dir in directories) {
                string[] tedFilesInSubFolder = Directory.GetFiles(dir, "*.ted");
                string[] sapFilesInSubFolder = Directory.GetFiles(dir, "SAP_??????.txt");
                tedFiles.AddRange(tedFilesInSubFolder);
                sapFiles.AddRange(sapFilesInSubFolder);
            }
            int tedCounter = 0;
            foreach (var file in tedFiles) {
                try {
                    string[] rows = Paragraphs(file);
                    if (rows.Length == 38) {
                        string month = rows[0].Substring(75, 2);
                        string year = rows[0].Substring(78, 4);
                        var dirName = new DirectoryInfo(file).Parent.Name;
                        var parentDir = Path.GetDirectoryName(file);
                        if (parentDir.EndsWith("TxtToCsv")) {
                            File.WriteAllLines(file.Remove(file.Length - 4) + "-" + year + "-" + month + ".csv", rows, destEncoding);
                        } else {
                            File.WriteAllLines(parentDir + Path.DirectorySeparatorChar + "Paragraphs" + "-" + dirName + "-" + year + "-" + month + ".csv", rows, destEncoding);
                        }
                        tedCounter++;
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }

            int sapCounter = 0;
            foreach (var file in sapFiles) {
                try {
                    string[] rows = SapFile(file);
                    File.WriteAllLines(file.Remove(file.Length - 4) + ".csv", rows, destEncoding);
                    sapCounter++;
                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
            sw.Stop();
            Console.WriteLine($"{tedCounter} of {tedFiles.Count} Paragraph files and {sapCounter} of {sapFiles.Count} SAP files converted in {sw.ElapsedMilliseconds} ms. Press any key to exit");
            Console.ReadKey();
        }

        private static string[] Paragraphs(string file) {
            try {
                string[] lines = File.ReadAllLines(file, srcTedEncoding);

                if (lines.Length != 69 || lines[10][4] != 'ћ' || lines[1][77] != '.') {
                    throw new ArgumentException($"File {file} is corrupted. Skipping.");
                }

                string[] tempRows = new string[67];

                for (int i = 1; i < lines.Length - 1; i++) {
                    char[] row = lines[i].ToCharArray();

                    if (row.Length > 82) {
                        row[13] = row[24] = row[44] = row[63] = ';';
                        if (row.Length == 84) {
                            row[0] = row[83] = ' ';
                            if (row[4] == 'љ') {
                                row[4] = ';';
                            } else {
                                row[11] = ';';
                            }
                            row[42] = row[60] = ';';
                        }
                        if (row.Length == 83) {
                            if (row[4] == ' ') {
                                row[4] = ';';
                            } else {
                                row[11] = ';';
                            }
                            row[33] = row[53] = row[63] = row[72] = ';';
                        }
                    }

                    string charsStr = new string(row);
                    tempRows[i - 1] = charsStr;
                }
                string[] rows = new string[38];

                int l = 0;
                for (int i = 0; i < tempRows.Length; i++) {
                    if (!(tempRows[i].StartsWith('¬') || tempRows[i].StartsWith('њ') || tempRows[i].StartsWith(" -") || tempRows[i].StartsWith(" †")
                        || i == 1 || i == 3 || i == 20 || i == 28 || i == 38 || i == 46 || i == 57)) {
                        rows[l] = tempRows[i];
                        l++;
                    }
                }
                return rows;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return new string[0];
            }
        }

        private static string[] SapFile(string file) {
            try {
                string[] lines = File.ReadAllLines(file);
                string[] rows = new string[lines.GetLength(0) + 1];
                rows[0] = "сектор;отдел;звено;бригада;раб.№;кат.пенсия;кат.персонал;<>60;ВО;;код ZHRM;сума";
                for (int i = 0; i < lines.Length; i++) {
                    char[] ln = lines[i].ToCharArray();
                    char[] code = {ln[0], ln[1], ';', ln[2], ln[3], ';', ln[4], ln[5], ';', ln[6], ln[7], ';',
                         ln[8], ln[9], ln[10], ln[11], ';', ln[12], ';', ln[13], ';', ln[14], ';', ln[15], ln[16], ';', ln[17],
                         ';', ln[18], ln[19], ln[20], ';'};
                    string sum = "";
                    for (int j = 22; j < ln.Length; j++) {
                        sum += ln[j];
                    }
                    string row = new string(code) + sum;
                    rows[i + 1] = row;
                }
                return rows;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return new string[0];
            }
        }
    }
}
