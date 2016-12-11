using RandomForest.Lib;
using RandomForest.Lib.Numerical.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.ItemSet.Item
{
    class ItemNumerical : IItemNumerical
    {
        private Dictionary<string, double> _values = new Dictionary<string, double>();

        private ItemNumerical()
        {

        }

        public static ItemNumerical Create()
        {
            return new ItemNumerical();
        }

        public void AddValue(string featureName, double featureValue = 0)
        {
            if (HasValue(featureName))
            {
                SetValue(featureName, featureValue);
                return;
            }
            _values.Add(featureName, featureValue);
        }

        public void RemoveValue(string featureName)
        {
            if (HasValue(featureName))
                _values.Remove(featureName);
        }

        public void SetValue(string featureName, double featureValue)
        {
            if (HasValue(featureName))
                _values[featureName] = featureValue;
            else
                throw new ArgumentException();
        }

        public ItemNumerical Clone()
        {
            ItemNumerical res = Create();
            foreach(var v in _values)
                res._values.Add(v.Key, v.Value);

            return res;
        }

        public double GetValue(string featureName)
        {
            if(HasValue(featureName))
                return _values[featureName];
            throw new ArgumentException();
        }

        public bool HasValue(string featureName)
        {
            if (_values.ContainsKey(featureName))
                return true;
            return false;
        }

        public List<string> GetFeatureNames()
        {
            return _values.Keys.ToList();
        }
    }
}
