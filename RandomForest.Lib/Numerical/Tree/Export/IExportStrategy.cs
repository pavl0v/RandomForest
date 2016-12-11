using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree.Export
{
    interface IExportStrategy
    {
        event EventHandler<string> ExportCompleted;
        void Export(TreeGenerative tree, string path);
    }
}
