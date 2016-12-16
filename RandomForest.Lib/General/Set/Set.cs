using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomForest.Lib.General.Set.Feature;
using RandomForest.Lib.General.Set.Item;

namespace RandomForest.Lib.General.Set
{
    class Set : RandomIndexer
    {
        private FeatureManager _featureManager;
        private List<Item.Item> _items = new List<Item.Item>();

        public Set(FeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public Item.Item CreateItem()
        {
            Item.Item item = new Item.Item(_featureManager);
            var names = _featureManager.GetFeatureNames();
            foreach (var name in names)
            {
                Feature.Feature feature = _featureManager.Get(name);
                item.AddValue(name, null);
            }
            return item;
        }

        public void AddItem(Item.Item item)
        {
            _items.Add(item);
        }

        public Item.Item AddItem(Item.FeatureValue[] values)
        {
            Item.Item item = new Item.Item(_featureManager);

            foreach (var v in values)
            {
                if (item.HasValue(v.Name))
                    item.SetValue(v.Name, v.Value);
                else
                    throw new ArgumentException();
            }
            _items.Add(item);

            return item;
        }

        public Item.Item GetItem(int index)
        {
            return _items[index];
        }

        public int Count()
        {
            return _items.Count;
        }

        public double GetAverage(string featureName)
        {
            if (!_featureManager.IsFeatureNumerical(featureName))
                throw new Exception();

            double res = 0;

            double sum = 0;
            foreach (var item in _items)
                sum += Convert.ToDouble(item.GetValue(featureName));
            res = sum / _items.Count;

            return res;
        }

        public double GetRSS(string featureName)
        {
            if (!_featureManager.IsFeatureNumerical(featureName))
                throw new Exception();

            double res = 0;

            double avg = GetAverage(featureName);
            foreach (var item in _items)
            {
                Item.FeatureValue fv = item.GetValue(featureName);
                double d = fv.GetValue<double>();
                res += Math.Pow(d - avg, 2);
            }

            return res;
        }

        public double GetGini(string featureName)
        {
            double res = 0;

            if (_featureManager.IsFeatureNumerical(featureName))
                res = GetGiniNumerical(featureName);
            else
                res = GetGiniCategorical(featureName);

            return 0;
        }

        private double GetGiniNumerical(string featureName)
        {
            int n = Count();
            if (n == 0)
                return 1;

            Dictionary<double, int> dic = new Dictionary<double, int>();
            foreach (var i in _items)
            {
                Item.FeatureValue fv = i.GetValue(featureName);
                double v = Convert.ToDouble(fv.Value);
                if (dic.ContainsKey(v))
                    dic[v]++;
                else
                    dic.Add(v, 1);
            }

            double p = 0;
            foreach (var kv in dic)
            {
                double pro = (kv.Value * 1.0) / n;
                p = p + pro * (1 - pro);
            }

            return Math.Round(p, 5);
        }

        private double GetGiniCategorical(string featureName)
        {
            int n = Count();
            if (n == 0)
                return 1;

            Dictionary<string, int> dic = new Dictionary<string, int>();
            foreach (var i in _items)
            {
                Item.FeatureValue fv = i.GetValue(featureName);
                string v = Convert.ToString(fv.Value);
                if (dic.ContainsKey(v))
                    dic[v]++;
                else
                    dic.Add(v, 1);
            }

            double p = 0;
            foreach (var kv in dic)
            {
                double pro = (kv.Value * 1.0) / n;
                p = p + pro * (1 - pro);
            }

            return Math.Round(p, 5);
        }

        public void SortItems(string featureName)
        {
            _items.Sort(new ItemComparer() { Feature = _featureManager.Get(featureName) });
        }

        public List<string> GetFeatureNames()
        {
            return _featureManager.GetFeatureNames();
        }

        public bool AddFeature(string featureName, FeatureType featureType)
        {
            return _featureManager.Add(new Feature.Feature(featureName, featureType));
        }

        public bool RemoveFeature(string featureName)
        {
            return _featureManager.Remove(featureName);
        }

        public bool CheckConsistency()
        {
            List<string> featureNames = _featureManager.GetFeatureNames();
            foreach (var name in featureNames)
            {
                foreach (var item in _items)
                {
                    if (!item.HasValue(name))
                        throw new Exception();
                }
            }

            return true;
        }

        public IEnumerable<Item.Item> Items()
        {
            for (int i = 0; i < _items.Count; i++)
                yield return _items[i];
        }

        public Set GetRandomSubset(float subcountRatio, bool withReplacement)
        {
            var idxLst = GetRandomIndeces(Count(), subcountRatio, withReplacement);
            Set res = new Set(_featureManager);
            foreach (int idx in idxLst)
                res.AddItem(_items[idx]);

            return res;
        }
    }
}
