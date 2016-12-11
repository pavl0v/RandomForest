using RandomForest.Lib.Numerical.Tree.Node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree.Export
{
    class ExportToTxt : IExportStrategy
    {
        public event EventHandler<string> ExportCompleted;

        public void Export(TreeGenerative tree, string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }

            using (StreamWriter sw = new StreamWriter(fi.FullName))
            {
                sw.WriteLine(string.Format("[{0}]", tree.Root.Set.Count()));
                ExportRecursion(sw, tree.Root, 1);
                sw.Close();
            }

            var exportCompleted = ExportCompleted;
            if (exportCompleted != null)
                exportCompleted(this, fi.Name);
        }

        private void ExportRecursion(StreamWriter sw, NodeGenerative node, int tabs)
        {
            StringBuilder tsb = new StringBuilder();
            for (int i = 0; i < tabs; i++)
                tsb.Append('\t');

            string conditionL = string.Empty;
            string conditionR = string.Empty;
            conditionL = string.Format("\t{0}<{1}", node.FeatureName, Math.Round(node.FeatureValue, 2));
            conditionR = string.Format("\t{0}>{1}", node.FeatureName, Math.Round(node.FeatureValue, 2));

            StringBuilder sb = new StringBuilder();

            if (node.Left != null)
            {
                sb.Append(string.Format("{2}[{1}]{0}", conditionL, node.Left.Set.Count(), tsb.ToString()));
                sw.WriteLine(sb.ToString());
                ExportRecursion(sw, node.Left, ++tabs);
            }
            tabs--;

            sb = new StringBuilder();
            if (node.Right != null)
            {
                sb.Append(string.Format("{2}[{1}]{0}", conditionR, node.Right.Set.Count(), tsb.ToString()));
                sw.WriteLine(sb.ToString());
                ExportRecursion(sw, node.Right, ++tabs);
            }
            tabs--;
        }
    }
}
