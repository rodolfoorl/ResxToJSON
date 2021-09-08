using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ResxToJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            var listFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.resx", SearchOption.AllDirectories).ToList();

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
                }
                catch { continue; }
            }
        }
    }
}
