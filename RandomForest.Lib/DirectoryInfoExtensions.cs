using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib
{
    static class DirectoryInfoExtensions
    {
        internal static void Clear(this DirectoryInfo di)
        {
            foreach(var fi in di.EnumerateFiles())
                fi.Delete();
        }
    }
}
