using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCLI.Models
{
    public class Settings
    {
        public static Settings Current { get; } = new Settings();
        public List<string> Usings { get; set; } = new List<string>()
        {
            "System",
            "System.Linq",
            "System.Text",
            "System.Threading.Tasks",
            "System.Collections.Generic"
        };
        public List<string> References { get; set; } = new List<string>()
        {
            "System.Linq"
        };
        public void Save()
        {
            var di = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CSCLI"));
            File.WriteAllText(Path.Combine(di.FullName, "Settings.json"), JSON_Helper.JSON.Encode(Settings.Current));
        }
        public void Load()
        {
            var di = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CSCLI"));
            var filePath = Path.Combine(di.FullName, "Settings.json");
            if (!File.Exists(filePath))
            {
                return;
            }
            var strContents = File.ReadAllText(filePath);
            var settings = JSON_Helper.JSON.Decode<Settings>(strContents);
            foreach (var prop in typeof(Settings).GetProperties())
            {
                if (prop.SetMethod == null)
                {
                    continue;
                }
                prop.SetValue(Settings.Current, prop.GetValue(settings));
            }
        }
    }
}
