using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.General.Set.Feature;
using System.Collections.Generic;

namespace RandomForest.Test.General
{
    [TestClass]
    public class FeatureManagerTest
    {
        //[TestInitialize()]
        //public void Startup()
        //{

        //}

        [TestMethod]
        public void Count_NoFeaturesAdded_ReturnsZero()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();

            // act
            int n = manager.Count();

            // assert
            Assert.AreEqual(0, n);
        }

        [TestMethod]
        public void Count_TwoFeaturesAdded_ReturnsTwo()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            int n = manager.Count();

            // assert
            Assert.AreEqual(2, n);
        }

        [TestMethod]
        public void Clear_TwoFeaturesAdded_ReturnsZero()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            int n1 = manager.Count();
            manager.Clear();
            int n2 = manager.Count();

            // assert
            Assert.AreEqual(2, n1);
            Assert.AreEqual(0, n2);
        }

        [TestMethod]
        public void GetFeature_TwoFeaturesAdded_ReturnsFeature()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            Feature feature = manager.Get("N");

            // assert
            Assert.IsNotNull(feature);
            Assert.AreEqual("N", feature.Name);
            Assert.AreEqual(FeatureType.Numerical, feature.Type);
        }

        [TestMethod]
        public void GetFeature_TwoFeaturesAdded_ReturnsNull()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            Feature feature = manager.Get("Z");

            // assert
            Assert.IsNull(feature);
        }

        [TestMethod]
        public void FeatureExist_TwoFeaturesAdded_ReturnsTrue()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            bool r = manager.Exist("C");

            // assert
            Assert.AreEqual(true, r);
        }

        [TestMethod]
        public void FeatureExist_TwoFeaturesAdded_ReturnsFalse()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            bool r = manager.Exist("Z");

            // assert
            Assert.AreEqual(false, r);
        }

        [TestMethod]
        public void GetFeatureNames_TwoFeaturesAdded_ReturnsEqual()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));
            List<string> lst = new List<string> { "C", "N" };

            // act
            List<string> names = manager.GetFeatureNames();

            // assert
            Assert.IsNotNull(names);
            Assert.AreEqual(2, names.Count);
            CollectionAssert.AreEqual(lst, names);
        }

        [TestMethod]
        public void GetFeatureValueType_TwoFeaturesAdded_ReturnsType()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            Type tn = manager.GetFeatureType("N");
            Type tc = manager.GetFeatureType("C");

            // assert
            Assert.AreEqual(typeof(double), tn);
            Assert.AreEqual(typeof(string), tc);
            Assert.AreNotEqual(typeof(int), tn);
        }

        [TestMethod]
        public void IsFeatureNumerical_TwoFeaturesAdded_ReturnsTrue()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            bool r1 = manager.IsFeatureNumerical("C");
            bool r2 = manager.IsFeatureNumerical("N");

            // assert
            Assert.AreEqual(false, r1);
            Assert.AreEqual(true, r2);
        }

        [TestMethod]
        public void IsFeatureCategorical_TwoFeaturesAdded_ReturnsTrue()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            bool r1 = manager.IsFeatureCategorical("C");
            bool r2 = manager.IsFeatureCategorical("N");

            // assert
            Assert.AreEqual(true, r1);
            Assert.AreEqual(false, r2);
        }

        [TestMethod]
        public void Remove_TwoFeaturesAdded_ReturnsTrue()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            // act
            bool r1 = manager.Remove("C");
            bool r2 = manager.Remove("Z");

            // assert
            Assert.AreEqual(true, r1);
            Assert.AreEqual(false, r2);
            Assert.AreEqual(1, manager.Count());
        }
    }
}
