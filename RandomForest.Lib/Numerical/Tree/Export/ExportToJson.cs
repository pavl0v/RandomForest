using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree.Export
{
    class ExportToJson : IExportStrategy
    {
        public event EventHandler<string> ExportCompleted;

        public void Export(TreeGenerative tree, string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }

            string json = JsonConvert.SerializeObject(tree.ToTree(), Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(fi.FullName))
            {
                sw.WriteLine(json);
                sw.Close();
            }

            var exportCompleted = ExportCompleted;
            if (exportCompleted != null)
                exportCompleted(this, fi.Name);
        }

        public void Export(Tree tree, string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }

            string json = JsonConvert.SerializeObject(tree, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(fi.FullName))
            {
                sw.WriteLine(json);
                sw.Close();
            }

            var exportCompleted = ExportCompleted;
            if (exportCompleted != null)
                exportCompleted(this, fi.Name);
        }
    }
}
