using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree.Import
{
    static class ImportFromJson
    {
        internal static Tree Read(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
                throw new FileNotFoundException();

            string json = string.Empty;
            using (var fs = fi.OpenRead())
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    json = sr.ReadToEnd();
                    sr.Close();
                }
                fs.Close();
            }

            var tree = JsonConvert.DeserializeObject<Tree>(json);

            return tree;
        }
    }
}
