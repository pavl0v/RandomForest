using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree
{
    abstract class TreeBase
    {
        public List<string> FeatureNames { get; set; }
        public string ResolutionFeatureName { get; set; }
        public int MaxItemCountInCategory { get; set; }
    }
}
