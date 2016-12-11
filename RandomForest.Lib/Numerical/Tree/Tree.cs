using RandomForest.Lib.Numerical.Interfaces;
using RandomForest.Lib.Numerical.ItemSet.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree
{
    class Tree : TreeBase
    {
        private Node.Node _root;
        public Node.Node Root
        {
            get { return _root; }
            set { _root = value; }
        }

        public double Resolve(IItemNumerical item)
        {
            return ResolveRecursion(item, _root);
        }

        private double ResolveRecursion(IItemNumerical item, Node.Node node)
        {
            double res = 0;

            if (node == null)
                throw new Exception();

            if (node.IsTerminal)
                return node.Average;

            if (!item.HasValue(node.FeatureName))
                throw new Exception();

            var v = item.GetValue(node.FeatureName);

            if (v < node.FeatureValue)
            {
                res = ResolveRecursion(item, node.Less);
            }
            else
            {
                res = ResolveRecursion(item, node.Greater);
            }

            return res;
        }
    }
}
