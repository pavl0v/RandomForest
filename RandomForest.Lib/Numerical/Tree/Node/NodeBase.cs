using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree.Node
{
    abstract class NodeBase
    {
        public bool IsTerminal { get; set; }
        public string Category { get; set; }
        public string FeatureName { get; set; }
        public double FeatureValue { get; set; }
        public double Average { get; set; }
    }
}
