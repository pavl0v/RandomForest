using RandomForest.Lib.Numerical.ItemSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree.Node
{
    class NodeGenerative : NodeBase
    {
        public ItemNumericalSet Set { get; set; }
        public NodeGenerative Parent { get; set; }
        public NodeGenerative Left { get; set; }
        public NodeGenerative Right { get; set; }
    }
}
