using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomForest.Lib.General.Feature;

namespace RandomForest.Lib.General
{
    class Set
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
    }
}
