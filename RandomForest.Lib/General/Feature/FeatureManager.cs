using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.General.Feature
{
    class FeatureManager : IFeatureManager
    {
        public event EventHandler<Feature> FeatureAdded;
        public event EventHandler<string> FeatureRemoved;
        public event EventHandler FeaturesCleared;

        private Dictionary<string, Feature> _features = new Dictionary<string, Feature>();

        public bool Add(Feature feature)
        {
            _features.Add(feature.Name, feature);

            var featureAdded = FeatureAdded;
            if (featureAdded != null)
                featureAdded(this, feature);

            return true;
        }

        public void Clear()
        {
            _features.Clear();

            var featuresCleared = FeaturesCleared;
            if (featuresCleared != null)
                featuresCleared(this, EventArgs.Empty);
        }

        public int Count()
        {
            return _features.Count;
        }

        public Feature Get(string featureName)
        {
            if (_features.ContainsKey(featureName))
                return _features[featureName];

            return null;
        }

        public bool Exist(string featureName)
        {
            if (_features.ContainsKey(featureName))
                return true;

            return false;
        }

        public List<string> GetFeatureNames()
        {
            return _features.Keys.ToList();
        }

        public Type GetFeatureType(string featureName)
        {
            if (!Exist(featureName))
                throw new ArgumentException();

            Type type;
            Feature feature = Get(featureName);
            switch(feature.Type)
            {
                case FeatureType.Categorical:
                    type = typeof(string);
                    break;

                case FeatureType.Numerical:
                    type = typeof(double);
                    break;

                default:
                    type = typeof(double);
                    break;
            }

            return type;
        }

        public bool IsFeatureNumerical(string featureName)
        {
            if (!Exist(featureName))
                throw new ArgumentException();

            Feature feature = Get(featureName);
            if (feature.Type == FeatureType.Numerical)
                return true;

            return false;
        }

        public bool IsFeatureCategorical(string featureName)
        {
            if (!Exist(featureName))
                throw new ArgumentException();

            Feature feature = Get(featureName);
            if (feature.Type == FeatureType.Categorical)
                return true;

            return false;
        }

        public bool Remove(string featureName)
        {
            _features.Remove(featureName);

            var featureRemoved = FeatureRemoved;
            if (featureRemoved != null)
                featureRemoved(this, featureName);

            return true;
        }
    }
}
