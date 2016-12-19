using RandomForest.Lib.General.Set.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.General.Set.Item
{
    class Item
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();
        private IFeatureManager _featureManager;

        public Item(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public bool HasValue(string featureName)
        {
            if (_values.ContainsKey(featureName))
                return true;

            return false;
        }

        public void SetValue(string featureName, object featureValue)
        {
            if (!_featureManager.Exist(featureName))
                throw new Exception("Unknown feature name.");

            if (!HasValue(featureName))
                throw new Exception("There is no feature with specified name.");

            if (featureValue == null)
                throw new Exception("Feature value is null.");

            Feature.Feature feature = _featureManager.Get(featureName);
            SetValue(feature, featureValue);
        }

        public void AddValue(string featureName, object featureValue)
        {
            if (HasValue(featureName))
            {
                SetValue(featureName, featureValue);
                return;
            }

            if (!_featureManager.Exist(featureName))
                throw new Exception("Unknown feature name.");

            Feature.Feature feature = _featureManager.Get(featureName);
            _values.Add(feature.Name, null);
            SetValue(feature, featureValue);
        }

        public void RemoveValue(string featureName)
        {
            if (HasValue(featureName))
                _values.Remove(featureName);
        }

        public FeatureValue GetValue(string featureName)
        {
            FeatureValue fv;

            if (HasValue(featureName))
            {
                Type t = _featureManager.GetFeatureType(featureName);
                fv = new FeatureValue(featureName, _values[featureName], t);
                return fv;
            }

            throw new ArgumentException();
        }

        public List<string> GetFeatureNames()
        {
            return _values.Keys.ToList();
        }

        private void SetValue(Feature.Feature feature, object featureValue)
        {
            if (featureValue == null)
                throw new ArgumentNullException();

            string temp = featureValue.ToString().Trim();

            switch (feature.Type)
            {
                case FeatureType.Categorical:
                    _values[feature.Name] = temp;
                    break;

                case FeatureType.Numerical:
                    double d = 0;
                    if (double.TryParse(temp, out d))
                        _values[feature.Name] = d;
                    else
                        throw new Exception("Invalid feature value.");
                    break;
            }
        }
    }
}
