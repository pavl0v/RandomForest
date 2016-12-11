using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomForest.Lib.Numerical.ItemSet.Feature;

namespace RandomForest.Lib.Numerical.ItemSet.Splitters
{
    class SplitterRss : ISplitter
    {
        public FeatureNumericalSplitValue Split(ItemNumericalSet set, string resolutionFeatureName)
        {
            double totalIndex = set.GetRSS(resolutionFeatureName);
            if (totalIndex == 0)
                return null;

            double minIndex = totalIndex;

            var featureNameList = set.GetFeatureNames();
            var featureNames = featureNameList.Where(x => x != resolutionFeatureName);
            if (!featureNames.Any())
                throw new Exception();

            FeatureNumericalSplitValue res = new FeatureNumericalSplitValue();

            foreach (string fn in featureNames)
            {
                set.SortItems(fn);

                int qty = set.Count();
                for (int k = 1; k < qty; k++)
                {
                    ItemNumericalSet left = new ItemNumericalSet(featureNameList);
                    ItemNumericalSet right = new ItemNumericalSet(featureNameList);
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

                        res.FeatureName = fn;
                        double lv = left.GetItem(left.Count() - 1).GetValue(fn);
                        double rv = right.GetItem(0).GetValue(fn);
                        res.FeatureValue = Math.Round(lv + (rv - lv) / 2, 5);
                        res.Left = left;
                        res.Right = right;
                    }
                }
            }

            return res;
        }
    }
}
