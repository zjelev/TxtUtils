using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TxtUtils
{
    public class Spr73_6
    {
        public static void Without(string spravkaFile, string withoutFile)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (spravkaFile == String.Empty)
            {
                spravkaFile = "2021_SPR73_6_8330175520038.xml";
            }

            if (withoutFile == String.Empty)
            {
                withoutFile = "Without.txt";
            }
            
            string directory = Directory.GetCurrentDirectory();
            string spravkaFilePath = Path.Combine(directory, spravkaFile);
            string withoutFilePath = Path.Combine(directory, withoutFile);

            // XElement spravka = XElement.Load(spravkaFilePath);
            // IEnumerable<string> egns = spravka.Descendants("Item").Select(x => (string)x.Attribute("ident"));
            int countSkipped = 0;
            try
            {
                string withouts = File.ReadAllText(withoutFilePath);
                List<string> withoutss = withouts.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();

                List<Person> people = new List<Person>();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                using (XmlReader reader = XmlReader.Create(new StreamReader(spravkaFilePath, TxtUtils.Program.srcEncoding)))
                {
                    while (reader.ReadToFollowing("ident"))
                    {
                        string egn = (string)reader.ReadElementContentAs(typeof(string), null);
                        if (!withoutss.Contains(egn))
                        {
                            Person person = new Person(egn);
                            people.Add(person);
                        }
                        else
                        {
                            Console.WriteLine($"{egn} is already in the without list. Skipping");
                            countSkipped++;
                        }
                    }
                }
                sw.Stop();
                Console.WriteLine($"The file {withoutFilePath} has {withoutss.Count} EGNs");
                Console.WriteLine($"{people.Count} people are included in file {spravkaFilePath}, of which {countSkipped} are also in {withoutFilePath}");
                Console.WriteLine($"Operation took {sw.ElapsedTicks} ticks");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
