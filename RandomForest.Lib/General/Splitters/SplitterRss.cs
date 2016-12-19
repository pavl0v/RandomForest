using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomForest.Lib.Numerical.ItemSet.Feature;
using RandomForest.Lib.General.Set.Feature;

namespace RandomForest.Lib.General.Set.Splitters
{
    class SplitterRss : ISplitter
    {
        public FeatureSplitValue Split(Set set, string resolutionFeatureName)
        {
            double totalIndex = set.GetRSS(resolutionFeatureName);
            if (totalIndex == 0)
                return null;

            double minIndex = totalIndex;

            var featureNameList = set.GetFeatureNames();
            var featureNames = featureNameList.Where(x => x != resolutionFeatureName);
            if (!featureNames.Any())
                throw new Exception();

            FeatureSplitValue res = null;

            foreach (string fn in featureNames)
            {
                set.SortItems(fn);

                int qty = set.Count();
                for (int k = 1; k < qty; k++)
                {
                    Set left = set.Clone();
                    Set right = set.Clone();
                    for (int i = 0; i < k; i++)
                        left.AddItem(set.GetItem(i));
                    for (int i = k; i < qty; i++)
                        right.AddItem(set.GetItem(i));

                    double leftIndex = left.GetRSS(resolutionFeatureName);
                    double rightIndex = right.GetRSS(resolutionFeatureName);

                    bool sumIndexIsLessThanMinIndex = leftIndex + rightIndex < minIndex;
                    bool sumIndexIsEqualToMinIndex = leftIndex + rightIndex == minIndex;

                    if (sumIndexIsLessThanMinIndex || (k == qty - 1 && sumIndexIsEqualToMinIndex))
                    {
                        if (sumIndexIsLessThanMinIndex)
                            minIndex = leftIndex + rightIndex;

                        Feature.Feature feature = set.GetFeature(fn);
                        object v = null;

                        if (feature.Type == FeatureType.Numerical)
                        {
                            double lv = left.GetItem(left.Count() - 1).GetValue(fn).GetValue<double>();
                            double rv = right.GetItem(0).GetValue(fn).GetValue<double>();
                            v = Math.Round(lv + (rv - lv) / 2, 5);
                        }
                        else
                        {
                            throw new NotImplementedException();
                            // v = "";
                        }
                        res = new FeatureSplitValue(fn, v);
                        res.Left = left;
                        res.Right = right;
                    }
                }
            }

            return res;
        }
    }
}
