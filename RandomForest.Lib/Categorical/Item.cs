using RandomForest.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Categorical
{
    class Item
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        private Item()
        {

        }

        public static Item Create()
        {
            return new Item();
        }

        public void AddValue(string featureName, object featureValue = null)
        {
            if (HasValue(featureName))
                return;
            _values.Add(featureName, featureValue);
        }

        public void SetValue(string featureName, object featureValue)
        {
            if (HasValue(featureName))
                _values[featureName] = featureValue;
            else
                throw new ArgumentException();
        }

        public Item Clone()
        {
            Item res = Create();
            foreach(var v in _values)
                res._values.Add(v.Key, v.Value);

            return res;
        }

        public object GetValue(string featureName)
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
    }
}
