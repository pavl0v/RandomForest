using RandomForest.Lib.Numerical.ItemSet.Feature;

namespace RandomForest.Lib.Numerical.ItemSet.Splitters
{
    interface ISplitter
    {
        FeatureNumericalSplitValue Split(ItemNumericalSet set, string resolutionFeatureName);
    }
}
