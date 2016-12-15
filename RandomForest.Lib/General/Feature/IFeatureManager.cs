using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.General.Feature
{
    interface IFeatureManager
    {
        event EventHandler<Feature> FeatureAdded;
        event EventHandler<string> FeatureRemoved;
        event EventHandler FeaturesCleared;

        bool Add(Feature feature);
        bool Remove(string featureName);
        bool Exist(string featureName);
        Feature Get(string featureName);
        int Count();
        void Clear();
        List<string> GetFeatureNames();
        Type GetFeatureType(string featureName);
    }
}
