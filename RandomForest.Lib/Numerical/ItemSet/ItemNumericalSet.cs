using RandomForest.Lib.Numerical.ItemSet.Feature;
using RandomForest.Lib.Numerical.ItemSet.Item;
using RandomForest.Lib.Numerical.ItemSet.Splitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.ItemSet
{
    class ItemNumericalSet : RandomIndexer
    {
        private FeatureNumericalManager _featureManager;
        private List<ItemNumerical> _items = new List<ItemNumerical>();

        public ItemNumericalSet(FeatureNumericalManager featureManager)
        {
            _featureManager = featureManager;
            _featureManager.FeatureAdded += _featureManager_FeatureAdded;
            _featureManager.FeatureRemoved += _featureManager_FeatureRemoved;
        }

        public ItemNumericalSet(IEnumerable<string> featureNames) 
            : this(new FeatureNumericalManager())
        {
            foreach (var name in featureNames)
                _featureManager.Add(new FeatureNumerical(name));
        }

        private void _featureManager_FeatureRemoved(object sender, string e)
        {
            foreach (var i in _items)
                i.RemoveValue(e);
        }

        private void _featureManager_FeatureAdded(object sender, FeatureNumerical e)
        {
            foreach (var i in _items)
                i.AddValue(e.Name);
        }

        public ItemNumerical CreateItem()
        {
            ItemNumerical item = ItemNumerical.Create();
            var names = _featureManager.GetFeatureNames();
            foreach (var name in names)
            {
                FeatureNumerical feature = _featureManager.Get(name);
                item.AddValue(name);
            }
            return item;
        }

        public void AddItem(ItemNumerical item)
        {
            _items.Add(item);
        }

        public ItemNumerical AddItem(FeatureNumericalValue[] values)
        {
            ItemNumerical item = CreateItem();

            foreach(var v in values)
            {
                if (item.HasValue(v.FeatureName))
                    item.SetValue(v.FeatureName, v.FeatureValue);
                else
                    throw new ArgumentException();
            }
            _items.Add(item);

            return item;
        }

        public ItemNumerical GetItem (int index)
        {
            return _items[index];
        }

        public int Count()
        {
            return _items.Count;
        }

        public double GetRSS(string featureName)
        {
            double res = 0;

            double avg = GetAverage(featureName);
            foreach (var item in _items)
                res += Math.Pow(item.GetValue(featureName) - avg, 2);

            return res;
        }

        public double GetGini(string featureName)
        {
            int n = Count();
            double sum_i = 0;
            double sum_ij = 0;

            foreach (var i in _items)
            {
                sum_i += i.GetValue(featureName);
                foreach (var j in _items)
                {
                    sum_ij += Math.Abs(i.GetValue(featureName) - j.GetValue(featureName));
                }
            }

            if (sum_i == 0)
                return 1;

            double res = sum_ij / (2 * n * sum_i);

            return res;
        }

        public double GetGini2(string featureName)
        {
            int n = Count();
            if (n == 0)
                return 1;

            Dictionary<double, int> dic = new Dictionary<double, int>();

            foreach (var i in _items)
            {
                double v = i.GetValue(featureName);
                if (dic.ContainsKey(v))
                    dic[v]++;
                else
                    dic.Add(v, 1);
            }

            double sum = 0;
            foreach (var kv in dic)
            {
                double p = (kv.Value * 1.0) / n;
                sum += Math.Pow(p, 2);
            }

            double res = 1 - sum;

            // min = 0
            // max = 1 - 1 / n

            //double max = 1 - 1.0 / n;
            //res = res / max;

            return Math.Round(res, 5);
        }

        public double GetAverage(string featureName)
        {
            double res = 0;

            double sum = 0;
            foreach (var item in _items)
                sum += Convert.ToDouble(item.GetValue(featureName));
            res = sum / _items.Count;

            return res;
        }

        public void SortItems(string featureName)
        {
            _items.Sort(new ItemNumericalComparer() { Feature = _featureManager.Get(featureName) });
        }

        public List<string> GetFeatureNames()
        {
            return _featureManager.GetFeatureNames();
        }

        public bool AddFeature(string featureName)
        {
            return _featureManager.Add(new FeatureNumerical(featureName));
        }

        public bool RemoveFeature(string featureName)
        {
            return _featureManager.Remove(featureName);
        }

        public bool CheckConsistency()
        {
            List<string> featureNames = _featureManager.GetFeatureNames();
            foreach(var name in featureNames)
            {
                foreach(var item in _items)
                {
                    if (!item.HasValue(name))
                        throw new Exception();
                }
            }

            return true;
        }

        public IEnumerable<ItemNumerical> Items()
        {
            for (int i = 0; i < _items.Count; i++)
                yield return _items[i];
        }

        public ItemNumericalSet GetRandomSubset(float subcountRatio, bool withReplacement)
        {
            var idxLst = GetRandomIndeces(Count(), subcountRatio, withReplacement);
            ItemNumericalSet res = new ItemNumericalSet(_featureManager);
            foreach(int idx in idxLst)
                res.AddItem(_items[idx]);

            return res;
        }

        public double GetMin(string featureName)
        {
            SortItems(featureName);
            return _items[0].GetValue(featureName);
        }

        public double GetMax(string featureName)
        {
            SortItems(featureName);
            return _items[Count() - 1].GetValue(featureName);
        }

        public ItemNumericalSet Clone()
        {
            FeatureNumericalManager fm = new FeatureNumericalManager();
            List<string> names = _featureManager.GetFeatureNames();
            foreach (string n in names)
            {
                FeatureNumerical f = _featureManager.Get(n);
                FeatureNumerical nf = new FeatureNumerical(f.Name);
                fm.Add(nf);
            }
            ItemNumericalSet res = new ItemNumericalSet(fm);
            return res;
        }
    }
}
