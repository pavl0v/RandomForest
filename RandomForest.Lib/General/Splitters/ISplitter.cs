using RandomForest.Lib.General.Set.Feature;

namespace RandomForest.Lib.General.Set.Splitters
{
    interface ISplitter
    {
        FeatureSplitValue Split(Set set, string resolutionFeatureName);
    }
}
