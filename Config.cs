using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace LegalHarvest
{
    public class HarvestConfig
    {
        public List<string> EnabledModules { get; set; }
        public bool CompressOutput { get; set; }
    }

    public static class Config
    {
        public static HarvestConfig Load(string path)
        {
            if (!File.Exists(path)) return new HarvestConfig { EnabledModules = new List<string>(), CompressOutput = false };
            return JsonConvert.DeserializeObject<HarvestConfig>(File.ReadAllText(path));
        }
    }
}
