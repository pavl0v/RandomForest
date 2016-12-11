using RandomForest.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Categorical
{
    class FeatureManager
    {
        Dictionary<string, FeatureBase> _features = new Dictionary<string, FeatureBase>();

        public event EventHandler<FeatureBase> FeatureAdded;
        public event EventHandler<string> FeatureRemoved;
        public event EventHandler FeaturesCleared;

        public bool Add<T>(Feature<T> feature)
        {
            _features.Add(feature.Name, feature);
            var featureAdded = FeatureAdded;
            if (featureAdded != null)
                featureAdded(this, feature);
            return true;
        }

        public bool Remove(string featureName)
        {
            _features.Remove(featureName);
            var featureRemoved = FeatureRemoved;
            if (featureRemoved != null)
                featureRemoved(this, featureName);
            return true;
        }

        public FeatureBase Get(string featureName)
        {
            if (_features.ContainsKey(featureName))
                return _features[featureName];
            return null;
        }

        public int Count()
        {
            return _features.Count;
        }

        public void Clear()
        {
            _features.Clear();
            var featuresCleared = FeaturesCleared;
            if (featuresCleared != null)
                featuresCleared(this, EventArgs.Empty);
        }

        public List<string> GetFeatureNames()
        {
            return _features.Keys.ToList();
        }
    }
}
