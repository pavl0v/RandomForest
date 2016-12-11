using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.Numerical;
using System.IO;
using RandomForest.Lib.Numerical.ItemSet.Item;
using RandomForest.Lib.Numerical.Interfaces;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class ForestTest
    {
        private void CleanDir(DirectoryInfo di)
        {
            foreach (FileInfo fi in di.GetFiles())
                fi.Delete();
        }

        private void GenerateJson(int count, string exportPath)
        {
            //int count = 40;
            //string exportPath = @"data\temp\json";
            string path = @"data\test-4x1000.xlsx";
            Forest forest = new Forest();
            int itemCount = forest.InitializeItemSet(path);
            DirectoryInfo di = new DirectoryInfo(exportPath);
            if (!di.Exists)
                di.Create();
            else
                CleanDir(di);
            forest.GenerateTrees(count, "F3", 10, 0.1f);
            forest.ExportToJsonTPL(exportPath);
        }

        [TestMethod]
        public void InitializeItemSet_Path()
        {
            // arrange
            string path = @"data\test-3x50.xlsx";
            Forest forest = new Forest();

            // act
            int itemCount = forest.InitializeItemSet(path);

            // assert
            Assert.AreEqual(50, itemCount);
        }

        [TestMethod]
        public void GenerateTrees_ItemSetInitialized_Returns15()
        {
            // arrange
            string path = @"data\test-3x50.xlsx";
            Forest forest = new Forest();

            // act
            int itemCount = forest.InitializeItemSet(path);
            int treeCount = forest.GenerateTrees(15, "Z", 5, 0.5f);

            // assert
            Assert.AreEqual(50, itemCount);
            Assert.AreEqual(15, treeCount);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GenerateTrees_ItemSetNotInitialized_Exception()
        {
            // arrange
            Forest forest = new Forest();

            // act
            int treeCount = forest.GenerateTrees(15, "Z", 5, 0.5f);

            // assert
            Assert.AreEqual(15, treeCount);
        }

        [TestMethod]
        public void ExportToJsonTPL()
        {
            // arrange
            int count = 40;
            string path = @"data\test-4x1000.xlsx";
            string exportPath = @"temp\1";
            Forest forest = new Forest();

            // act
            int itemCount = forest.InitializeItemSet(path);
            DirectoryInfo di = new DirectoryInfo(exportPath);
            if (!di.Exists)
                di.Create();
            else
                CleanDir(di);
            int treeCount = forest.GenerateTrees(count, "F3", 50, 0.2f);
            forest.ExportToJsonTPL(exportPath);

            // assert
            Assert.AreEqual(1000, itemCount);
            Assert.AreEqual(count, treeCount);
        }

        [TestMethod]
        public void ImportFromJson()
        {
            // arrange
            string importPath = @"data\forest40";
            Forest forest = new Forest();

            // act
            int treeCount = forest.ImportFromJsonTPL(importPath);
            bool consistency = forest.CheckConsistency();

            // assert
            Assert.AreEqual(40, treeCount);
            Assert.AreEqual(true, consistency);
        }

        [TestMethod]
        public void Resolve()
        {
            // arrange
            string importPath = @"data\forest40";
            Forest forest = new Forest();
            forest.ImportFromJsonTPL(importPath);
            ItemNumerical item = ItemNumerical.Create();
            item.AddValue("F1", 41);
            item.AddValue("F2", 42);
            item.AddValue("F3", 47);
            item.AddValue("F4", 46);

            // act
            double v = forest.Resolve(item);
            double d = item.GetValue("F3") - v;

            // assert
        }

        [TestMethod]
        public void Grow_TrainingDataXlsx_RSS()
        {
            // arrange
            IForest forest = new Forest();
            ForestGrowParameters parameters = new ForestGrowParameters
            {
                ExportDirectoryPath = @"temp\cross-rss",
                ExportToJson = true,
                ResolutionFeatureName = "Z",
                ItemSubsetCountRatio = 0.3f,
                TrainingDataPath = @"data\test-3x1000-cross.xlsx",
                MaxItemCountInCategory = 10,
                TreeCount = 50,
                SplitMode = SplitMode.RSS
            };

            ItemNumerical item1 = ItemNumerical.Create();
            item1.AddValue("X", 1.1);
            item1.AddValue("Y", 2.1);
            item1.AddValue("Z", 0);

            ItemNumerical item2 = ItemNumerical.Create();
            item2.AddValue("X", 1.9);
            item2.AddValue("Y", 3.1);
            item2.AddValue("Z", 0);

            // act
            int treeCount = forest.Grow(parameters);
            var d1 = Math.Round(forest.Resolve(item1), 0);
            var d2 = Math.Round(forest.Resolve(item2), 0);

            // assert
            Assert.AreEqual(50, treeCount);
            Assert.AreEqual(2, d1);
            Assert.AreEqual(1, d2);
        }

        [TestMethod]
        public void Grow_TrainingDataXlsx_Gini()
        {
            // arrange
            IForest forest = new Forest();
            ForestGrowParameters parameters = new ForestGrowParameters
            {
                ExportDirectoryPath = @"temp\cross-gini",
                ExportToJson = true,
                ResolutionFeatureName = "Z",
                ItemSubsetCountRatio = 0.3f,
                TrainingDataPath = @"data\test-3x1000-cross.xlsx",
                MaxItemCountInCategory = 10,
                TreeCount = 50,
                SplitMode = SplitMode.GINI
            };

            ItemNumerical item1 = ItemNumerical.Create();
            item1.AddValue("X", 1.1);
            item1.AddValue("Y", 2.1);
            item1.AddValue("Z", 0);

            ItemNumerical item2 = ItemNumerical.Create();
            item2.AddValue("X", 1.9);
            item2.AddValue("Y", 3.1);
            item2.AddValue("Z", 0);

            // act
            int treeCount = forest.Grow(parameters);
            var d1 = Math.Round(forest.Resolve(item1), 0);
            var d2 = Math.Round(forest.Resolve(item2), 0);

            // assert
            Assert.AreEqual(50, treeCount);
            Assert.AreEqual(2, d1);
            Assert.AreEqual(1, d2);
        }

        [TestMethod]
        public void Grow_TrainingDataJson()
        {
            // arrange
            IForest forest = new Forest();

            ItemNumerical item1 = ItemNumerical.Create();
            item1.AddValue("X", 1.1);
            item1.AddValue("Y", 2.1);
            item1.AddValue("Z", 0);

            ItemNumerical item2 = ItemNumerical.Create();
            item2.AddValue("X", 1.9);
            item2.AddValue("Y", 3.1);
            item2.AddValue("Z", 0);

            // act
            int treeCount = forest.Grow(@"data\cross");
            var d1 = Math.Round(forest.Resolve(item1), 0);
            var d2 = Math.Round(forest.Resolve(item2), 0);

            // assert
            Assert.AreEqual(50, treeCount);
            Assert.AreEqual(2, d1);
            Assert.AreEqual(1, d2);
        }
    }
}
