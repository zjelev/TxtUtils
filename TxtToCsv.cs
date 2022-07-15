using System;
using System.IO;

namespace TxtUtils {
    public static class TxtToCsv {
        
        public static string[] Parag(string file) {
            try {
                string[] lines = File.ReadAllLines(file, TxtUtils.Program.srcEncoding);

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

        public static string[] Sap(string file) {
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