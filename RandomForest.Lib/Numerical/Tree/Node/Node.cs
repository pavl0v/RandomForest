using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree.Node
{
    class Node : NodeBase
    {
        public int ExamItems { get; set; }
        public Node Less { get; set; }
        public Node Greater { get; set; }

        public Node()
        {

        }

        public Node(NodeGenerative node)
        {
            IsTerminal = node.IsTerminal;
            Category = node.Category;
            FeatureName = node.FeatureName;
            FeatureValue = node.FeatureValue;
            Average = node.Average;
            ExamItems = node.Set.Count();
            Less = node.Left == null ? null : new Node(node.Left);
            Greater = node.Right == null ? null : new Node(node.Right);
        }
    }
}
