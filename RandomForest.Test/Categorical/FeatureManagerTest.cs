using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib;
using System.Collections.Generic;
using RandomForest.Lib.Categorical;

namespace RandomForest.Test.Categorical
{
    [TestClass]
    public class FeatureManagerTest
    {
        [TestMethod]
        public void Add_OneFeature_ReturnsTrue()
        {
            // arrange
            FeatureBase featureBase = null;
            FeatureManager featureManager = new FeatureManager();
            featureManager.FeatureAdded += (s, e) => 
            {
                featureBase = e as FeatureBase;
            };
            Feature<int> feature = new Feature<int>("F");

            // act
            bool r = featureManager.Add(feature);

            // assert
            Assert.AreEqual(true, r);
            Assert.IsNotNull(featureBase);
            Assert.AreEqual("F", featureBase.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Add_TwoFeaturesWithTheSameName_ReturnsException()
        {
            // arrange
            FeatureManager featureManager = new FeatureManager();
            Feature<int> feature1 = new Feature<int>("F");
            Feature<int> feature2 = new Feature<int>("F");

            // act
            featureManager.Add(feature1);
            featureManager.Add(feature2);

            // assert
        }

        [TestMethod]
        public void Add_TwoFeaturesWithDifferentNames_ReturnsTrue()
        {
            // arrange
            FeatureManager featureManager = new FeatureManager();
            Feature<int> feature1 = new Feature<int>("F1");
            Feature<int> feature2 = new Feature<int>("F2");

            // act
            bool r1 = featureManager.Add(feature1);
            bool r2 = featureManager.Add(feature2);

            // assert
            Assert.AreEqual(true, r1 & r2);
        }

        [TestMethod]
        public void Remove_Feature_ReturnsTrue()
        {
            // arrange
            string name = string.Empty;
            FeatureManager featureManager = new FeatureManager();
            featureManager.FeatureRemoved += (s, e) => 
            {
                name = e;
            };
            featureManager.Add(new Feature<int>("F1"));

            // act
            bool r1 = featureManager.Remove("F1");

            // assert
            Assert.AreEqual(true, r1);
            Assert.AreEqual("F1", name);
        }

        [TestMethod]
        public void Get_FeatureName_ReturnsFeature()
        {
            // arrange
            FeatureManager featureManager = new FeatureManager();
            featureManager.Add(new Feature<int>("F1"));

            // act
            FeatureBase feature = featureManager.Get("F1");
            Type type = feature.GetFeatureType();

            // assert
            Assert.IsNotNull(feature);
            Assert.AreEqual(feature.Name, "F1");
            Assert.AreEqual(typeof(int), type);
        }

        [TestMethod]
        public void Count_TwoFeatures_ReturnsTwo()
        {
            // arrange
            FeatureManager featureManager = new FeatureManager();
            featureManager.Add(new Feature<int>("F1"));
            featureManager.Add(new Feature<double>("F2"));

            // act
            int qty = featureManager.Count();

            // assert
            Assert.AreEqual(2, qty);
        }

        [TestMethod]
        public void Clear_TwoFeatures_ReturnsZero()
        {
            // arrange
            FeatureManager featureManager = new FeatureManager();
            featureManager.Add(new Feature<int>("F1"));
            featureManager.Add(new Feature<double>("F2"));

            // act
            featureManager.Clear();
            int qty = featureManager.Count();

            // assert
            Assert.AreEqual(0, qty);
        }

        [TestMethod]
        public void GetFeatureNames_TwoFeatures_ReturnsListOfTwoStrings()
        {
            // arrange
            FeatureManager featureManager = new FeatureManager();
            featureManager.Add(new Feature<int>("F1"));
            featureManager.Add(new Feature<double>("F2"));

            // act
            List<string> names = featureManager.GetFeatureNames();

            // assert
            Assert.AreEqual(true, names.Contains("F1") & names.Contains("F2"));
        }
    }
}
