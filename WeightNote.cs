using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TxtUtils
{
    public class WeightNote
    {
        public static string TranslateToEn(string note)
        {
            try
            {
                string[] inputFile = File.ReadAllLines(note, Program.srcEncoding);
  
                string number = inputFile[1].Substring(inputFile[1].IndexOf("N ") + 2, 3);
                string date = inputFile[1].Substring(inputFile[1].IndexOf("Дата ") + 5, 8);
                string receiver = inputFile[2].Substring(inputFile[2].IndexOf("Фирма-получател: ") + 17, 35);
                string supplier = inputFile[2].Substring(inputFile[2].IndexOf("Фирма-доставчик: ") + 17, 5); 
                string driverNames = inputFile[2].Substring(inputFile[2].IndexOf("Предал: ") + 8, 12);
                string truckNum = inputFile[5].Substring(19, 8);
                string netWeight = inputFile[5].Substring(30, 5);
                string grossWeight = inputFile[5].Substring(30, 5);
                string grossStatus = inputFile[5].Substring(30, 5);
                string grossDate = inputFile[5].Substring(30, 5);
                string grossHour = inputFile[5].Substring(30, 5);
                string tareWeight = inputFile[5].Substring(30, 5);
                string tareStatus = inputFile[5].Substring(30, 5);
                string tareDate = inputFile[5].Substring(30, 5);
                string tareHour = inputFile[5].Substring(30, 5);

                string template = File.ReadAllText("WeightNote.template");

                Dictionary<string, string> paramKvp = new Dictionary<string, string>();
                paramKvp["@NOM_D"] = number;
                paramKvp["@DATA_DOKUM"] = date;
                paramKvp["@DATA_DOK"] = date;
                paramKvp["@VAG_P"] = truckNum;
                paramKvp["@I_NETO"] = netWeight;
                paramKvp["@BRUTO_T"] = grossWeight;
                paramKvp["@BRUTO_S"] = grossStatus;
                paramKvp["@BRUTO_DAY"] = grossDate;
                paramKvp["@BRUTO_4A"] = grossDate;
                paramKvp["@TARA_T"] = grossDate;
                paramKvp["@TARA_S"] = grossDate;
                paramKvp["@TARA_DAY"] = grossDate;
                paramKvp["@TARA_4"] = grossDate;
                
                // var regex = @"@\w+";
                // var parameters = Regex.Matches(template, regex);

                // int count = 0;
                // foreach (Match parameter in parameters)
                // {
                //     if(!paramKvp.ContainsKey(parameter.Value))
                //     {
                //         paramKvp.Add(parameter.Value, values[count]);
                //         count++;
                //     }
                // }
                
                foreach (var kvp in paramKvp)
                {
                    template = template.Replace(kvp.Key, kvp.Value);
                }

                return template;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }

}