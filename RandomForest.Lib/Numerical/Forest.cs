using RandomForest.Lib.Numerical.Interfaces;
using RandomForest.Lib.Numerical.ItemSet;
using RandomForest.Lib.Numerical.ItemSet.Splitters;
using RandomForest.Lib.Numerical.Tree;
using RandomForest.Lib.Numerical.Tree.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical
{
    class Forest : IForest
    {
        private List<Tree.Tree> _trees;
        private TreeBuilder _treeBulder;
        private ItemNumericalSet _set;
        private IExportStrategy _exportStrategy;
        private ISplitter _splitter;

        public event EventHandler TreeBuildComplete;
        public event EventHandler ForestGrowComplete;

        public Forest()
        {
            _trees = new List<Tree.Tree>();
            _exportStrategy = new ExportToJson();
            _splitter = new SplitterRss();
        }

        public void ExportToJson(string directoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            if (!di.Exists)
                di.Create();
            else
                di.Clear();

            var nameGenerator = new NameGenerator();
            foreach (var tree in _trees)
            {
                string path = string.Format(@"{0}\{1}.json", di.FullName, nameGenerator.Generate("decision-tree-"));
                (_exportStrategy as ExportToJson).Export(tree, path);
            }
        }

        public void ExportToJsonTPL(string directoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            if (!di.Exists)
                di.Create();
            else
                di.Clear();

            var nameGenerator = new NameGenerator();
            Parallel.ForEach(_trees, (t) =>
            {
                string name = nameGenerator.Generate("decision-tree-");
                string path = string.Format(@"{0}\{1}.json", di.FullName, name);
                (_exportStrategy as ExportToJson).Export(t, path);
            });
        }

        private void _exportStrategy_ExportCompleted(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public int ImportFromJson(string directoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            if (!di.Exists)
                throw new DirectoryNotFoundException();

            _trees.Clear();
            FileInfo[] fis = di.GetFiles("decision-tree-*.json");
            foreach(var fi in fis)
            {
                Tree.Tree tree = Tree.Import.ImportFromJson.Read(fi.FullName);
                _trees.Add(tree);
            }
            return TreeCount();
        }

        public int ImportFromJsonTPL(string directoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            if (!di.Exists)
                throw new DirectoryNotFoundException();

            _trees.Clear();
            FileInfo[] fis = di.GetFiles("decision-tree-*.json");
            Parallel.ForEach(fis, (fi) => 
            {
                Tree.Tree tree = Tree.Import.ImportFromJson.Read(fi.FullName);
                _trees.Add(tree);
            });
            return TreeCount();
        }

        public int GenerateTrees(int count, string resolutionFeatureName, int maxItemCountInCategory, float itemSubsetCountRatio)
        {
            _trees.Clear();
            var nameGenerator = new NameGenerator();
            _treeBulder = new TreeBuilder(resolutionFeatureName, maxItemCountInCategory, nameGenerator, _splitter);
            for (int i = 0; i < count; i++)
            {
                ItemNumericalSet subset = _set.GetRandomSubset(itemSubsetCountRatio, true);
                TreeGenerative tg = _treeBulder.Build(subset);
                _trees.Add(tg.ToTree());
                nameGenerator.Reset();
            }
            return TreeCount();
        }

        public int GenerateTreesTPL(int treeCount, string resolutionFeatureName, int maxItemCountInCategory, float itemSubsetCountRatio)
        {
            _trees.Clear();

            Parallel.For(0, treeCount, (i) => 
            {
                NameGenerator nameGenerator = new NameGenerator();
                ItemNumericalSet subset = _set.GetRandomSubset(itemSubsetCountRatio, true);
                _treeBulder = new TreeBuilder(resolutionFeatureName, maxItemCountInCategory, nameGenerator, _splitter);
                _treeBulder.BuildComplete += OnTreeBuildComplete;
                TreeGenerative tg = _treeBulder.Build(subset);
                _treeBulder.BuildComplete -= OnTreeBuildComplete;
                _trees.Add(tg.ToTree());
            });

            var forestGrowComplete = ForestGrowComplete;
            if (forestGrowComplete != null)
                forestGrowComplete(this, EventArgs.Empty);

            return TreeCount();
        }

        public int InitializeItemSet(string pathToXlsx)
        {
            _set = ExcelParser.ParseItemNumericalSet(pathToXlsx);
            return ItemCount();
        }

        public int ItemCount()
        {
            return _set.Count();
        }

        public int TreeCount()
        {
            return _trees.Count();
        }

        public bool CheckConsistency()
        {
            var resolutionFeatureNames = _trees.Select(x => x.ResolutionFeatureName).Distinct().ToList();
            if (resolutionFeatureNames == null || resolutionFeatureNames.Count != 1)
                return false;
            if (resolutionFeatureNames[0] == null)
                throw new Exception();

            return true;
        }

        #region IForest implementation

        public double Resolve(IItemNumerical item)
        {
            if (_trees == null || _trees.Count == 0)
                throw new Exception();

            object obj = new object();

            double sum = 0;
            Parallel.ForEach(_trees, (t) => {
                double d = t.Resolve(item);
                lock(obj)
                {
                    sum = sum + d;
                }
            });

            return sum / _trees.Count;
        }

        public int Grow(ForestGrowParameters growParameters)
        {
            if (growParameters == null)
                throw new Exception();

            int qty = 0;

            switch(growParameters.SplitMode)
            {
                case SplitMode.GINI:
                    _splitter = new SplitterGini();
                    break;

                case SplitMode.RSS:
                    _splitter = new SplitterRss();
                    break;

                default:
                    _splitter = new SplitterRss();
                    break;
            }

            InitializeItemSet(growParameters.TrainingDataPath);
            qty = GenerateTreesTPL(
                growParameters.TreeCount, 
                growParameters.ResolutionFeatureName,
                growParameters.MaxItemCountInCategory,
                growParameters.ItemSubsetCountRatio);

            if (growParameters.ExportToJson)
                ExportToJsonTPL(growParameters.ExportDirectoryPath);

            return qty;
        }

        public async Task<int> GrowAsync(ForestGrowParameters growParameters)
        {
            return await Task<int>.Factory.StartNew(() => { return Grow(growParameters); });
        }

        public int Grow(string directoryPath)
        {
            int qty = ImportFromJsonTPL(directoryPath);
            if (!CheckConsistency())
                throw new Exception();
            return qty;
        }

        public List<string> GetFeatureNames()
        {
            return _set.GetFeatureNames();
        }

        public IItemNumerical CreateItem()
        {
            return _set.CreateItem();
        }

        #endregion

        private void OnTreeBuildComplete(object sender, EventArgs e)
        {
            var treeBuildComplete = TreeBuildComplete;
            if (treeBuildComplete != null)
                treeBuildComplete(this, EventArgs.Empty);
        }
    }
}
