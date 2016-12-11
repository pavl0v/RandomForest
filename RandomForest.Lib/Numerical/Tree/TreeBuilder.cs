using Newtonsoft.Json;
using RandomForest.Lib.Numerical.ItemSet;
using RandomForest.Lib.Numerical.ItemSet.Splitters;
using RandomForest.Lib.Numerical.Tree.Node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree
{
    class TreeBuilder
    {
        private int _maxItemCountInCategory = 5;
        private NameGenerator _categoryNameGenerator;
        private string _resolutionFeatureName;
        private ISplitter _splitter;

        public event EventHandler BuildComplete;

        public TreeBuilder(string resolutionFeatureName, int maxItemCountInCategory, NameGenerator categoryNameGenerator, ISplitter splitter)
        {
            _resolutionFeatureName = resolutionFeatureName;
            _maxItemCountInCategory = maxItemCountInCategory;
            _categoryNameGenerator = categoryNameGenerator;
            _splitter = splitter;
        }

        public TreeGenerative Build(ItemNumericalSet set)
        {
            _categoryNameGenerator.Reset();
            var buildComplete = BuildComplete;
            TreeGenerative tree = new TreeGenerative(_resolutionFeatureName, _maxItemCountInCategory);
            tree.FeatureNames = set.GetFeatureNames();
            tree.Root = new NodeGenerative();
            tree.Root.Set = set;
            tree.Root.Average = tree.Root.Set.GetAverage(_resolutionFeatureName);
            BuildRecursion(tree.Root);
            if (buildComplete != null)
                buildComplete(this, EventArgs.Empty);
            return tree;
        }

        private void BuildRecursion(NodeGenerative node)
        {
            node.Average = node.Set.GetAverage(_resolutionFeatureName);
            if (node.Set.Count() <= _maxItemCountInCategory)
            {
                node.IsTerminal = true;
                node.Category = _categoryNameGenerator.Generate("C");
                return;
            }

            //var sv = node.Set.GetSeparateValue(_resolutionFeatureName);
            var sv = _splitter.Split(node.Set, _resolutionFeatureName);
            if (sv == null)
            {
                node.IsTerminal = true;
                node.Category = _categoryNameGenerator.Generate("C");
                return;
            }

            node.FeatureName = sv.FeatureName;
            node.FeatureValue = sv.FeatureValue;

            if (sv.Left != null)
            {
                NodeGenerative left = new NodeGenerative();
                left.Parent = node;
                left.Set = sv.Left;
                node.Left = left;
                BuildRecursion(left);
            }
            if(sv.Right != null)
            {
                NodeGenerative right = new NodeGenerative();
                right.Parent = node;
                right.Set = sv.Right;
                node.Right = right;
                BuildRecursion(right);
            }
        }
    }
}
