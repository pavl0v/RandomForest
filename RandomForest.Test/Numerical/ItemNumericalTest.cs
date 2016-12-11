using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib;
using RandomForest.Lib.Numerical;
using System.Collections.Generic;
using RandomForest.Lib.Numerical.ItemSet.Feature;
using RandomForest.Lib.Numerical.ItemSet;
using RandomForest.Lib.Numerical.ItemSet.Item;
using RandomForest.Lib.Numerical.ItemSet.Splitters;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class ItemNumericalTest
    {
        [TestMethod]
        public void Clone_Item_ReturnsClone()
        {
            // arrange
            string f1 = "F1";
            string f2 = "F2";
            FeatureNumericalManager featureManager = new FeatureNumericalManager();
            featureManager.Add(new FeatureNumerical(f1));
            featureManager.Add(new FeatureNumerical(f2));
            ItemNumericalSet set = new ItemNumericalSet(featureManager);
            ItemNumerical item = set.CreateItem();
            item.SetValue(f1, 5);
            item.SetValue(f2, 3.8f);

            // act
            ItemNumerical clone = item.Clone();
            item.SetValue(f1, 10);

            // assert
            Assert.AreEqual(5, clone.GetValue(f1));
            Assert.AreEqual(3.8f, clone.GetValue(f2));
        }

        [TestMethod]
        public void GetFeatureNames_Item_ReturnsListOfNames()
        {
            // arrange
            List<string> featureNames = new List<string> { "F1", "F2" };
            ItemNumericalSet set = new ItemNumericalSet(featureNames);
            ItemNumerical i = set.CreateItem();
            set.AddItem(i);

            // act
            List<string> names = i.GetFeatureNames();

            // assert
            CollectionAssert.AreEqual(featureNames, names);
        }
    }
}
