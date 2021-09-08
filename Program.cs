using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ResxToJSON
{
    class Program
    {
        static string Divisor = "---------------------";

        static void Main(string[] args)
        {
            int Errors = 0;
            var listFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.resx", SearchOption.AllDirectories).ToList();

            Console.WriteLine($"Arquivos encontrados: {listFiles.Count}");
            Console.WriteLine(Divisor);

            foreach (var file in listFiles)
            {
                try
                {
                    string xmlText = File.ReadAllText(file);

                    var jsonText = "{" + Environment.NewLine;

                    var listData = XElement.Parse(xmlText)
                                           .Elements("data")
                                           .ToList();

                    listData.ForEach(delegate (XElement data)
                    {
                        var name = data.Attribute("name").Value;
                        var valor = data.Element("value").Value
                                     .Trim()
                                     .Replace('"', '\'');

                        jsonText += $"\t\"{name}\": \"{valor}\",{Environment.NewLine}";
                    });

                    jsonText += "}";

                    File.WriteAllText(file.Replace(".resx", ".json"), jsonText);
                    Console.WriteLine($"{file} - OK");
                }
                catch
                {
                    Console.WriteLine($"{file} - Error");
                    continue;
                }
            }

            Console.WriteLine(Divisor);
            Console.WriteLine($"{Errors} Arquivos com falha");
            Console.WriteLine($"{listFiles.Count - Errors} Arquivos com êxito");
            Console.Read();
        }
    }
}
