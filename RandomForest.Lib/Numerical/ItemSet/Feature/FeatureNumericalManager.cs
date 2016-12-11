using RandomForest.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.ItemSet.Feature
{
    class FeatureNumericalManager : RandomIndexer
    {
        Dictionary<string, FeatureNumerical> _features = new Dictionary<string, FeatureNumerical>();

        public event EventHandler<FeatureNumerical> FeatureAdded;
        public event EventHandler<string> FeatureRemoved;
        public event EventHandler FeaturesCleared;

        public bool Add(FeatureNumerical feature)
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

        public FeatureNumerical Get(string featureName)
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

        public List<string> GetFeatureNames(IEnumerable<string> ignore, float subcountRatio = 0)
        {
            List<string> res = new List<string>();
            List<string> ini = null;

            if (ignore != null && ignore.Count() > 0)
            {
                ini = new List<string>();
                foreach(var key in _features.Keys.ToList())
                {
                    if (!ignore.Contains(key))
                        ini.Add(key);
                }
            }
            else
            {
                ini = _features.Keys.ToList();
            }

            if (subcountRatio > 0)
            {
                var idxLst = GetRandomIndeces(ini.Count(), subcountRatio);
                foreach (int idx in idxLst)
                    res.Add(ini[idx]);
            }
            else
            {
                res = ini;
            }

            return res;
        }
    }
}
