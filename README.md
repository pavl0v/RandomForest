# RandomForest

## Wiki

<https://github.com/pavl0v/RandomForest/wiki>

## Usage

```cs
    using RandomForest.Lib.Numerical.Interfaces;
    
    // Parameters to "grow" the forest
    ForestGrowParameters p = new ForestGrowParameters
    {
        ExportDirectoryPath = [folder to save json tree files],
        ExportToJson = true,
        ResolutionFeatureName = [name of the feature to classify training items by],
        ItemSubsetCountRatio = [ratio to get a number of training items to form subset and generate tree],
        TrainigDataPath = [path to training data MS Excel file],
        MaxItemCountInCategory = [max number of items in a terminal node],
        TreeCount = [number of trees to generate],
        SplitMode = [RSS or Gini]
    };
    
    // Method to "grow" the forest
    IForest forest = ForestFactory.Create();
    forest.GrowAsync(p);
    
    // Method to make a prediction for an item
    IItemNumerical item = forest.CreateItem();
    item.SetValue("X", 1.23);
    item.SetValue("Y", 2.05);
    double z = forest.Resolve(item);
```