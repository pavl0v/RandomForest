using RandomForest.Lib.Numerical.Tree.Export;
using RandomForest.Lib.Numerical.Tree.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree
{
    class TreeGenerative : TreeBase
    {
        private IExportStrategy _exportStrategy;
        private NodeGenerative _root;

        public NodeGenerative Root
        {
            get { return _root; }
            set { _root = value; }
        }

        public IExportStrategy ExportStrategy
        {
            get { return _exportStrategy; }
            set { _exportStrategy = value; }
        }

        public TreeGenerative(string resolutionFeatureName, int maxItemCountInCategory)
        {
            ResolutionFeatureName = resolutionFeatureName;
            MaxItemCountInCategory = maxItemCountInCategory;
        }

        public void Export(string path)
        {
            if (_exportStrategy == null)
                throw new Exception();
            _exportStrategy.Export(this, path);
        }

        public Tree ToTree()
        {
            Node.Node io = ToTreeRecursion(Root);
            Tree t = new Tree();
            t.FeatureNames = FeatureNames.ToList();
            t.ResolutionFeatureName = ResolutionFeatureName;
            t.MaxItemCountInCategory = MaxItemCountInCategory;
            t.Root = io;
            return t;
        }

        private static Node.Node ToTreeRecursion(NodeGenerative node)
        {
            Node.Node res = new Node.Node(node);

            if (node.Left != null)
                ToTreeRecursion(node.Left);

            if (node.Right != null)
                ToTreeRecursion(node.Right);

            return res;
        }
    }
}
