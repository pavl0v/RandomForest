using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.General.Set.Feature;
using RandomForest.Lib.General.Set.Item;
using System.Collections.Generic;

namespace RandomForest.Test.General
{
    [TestClass]
    public class ItemTest
    {
        [TestMethod]
        public void HasValue_ReturnsFalse()
        {
            // arrange
            IFeatureManager fm = new FeatureManager();
            fm.Add(new Feature("C", FeatureType.Categorical));
            Item i = new Item(fm);

            // act
            bool r = i.HasValue("C");

            // assert
            Assert.IsFalse(r);
        }

        [TestMethod]
        public void HasValue_ReturnsTrue()
        {
            // arrange
            IFeatureManager fm = new FeatureManager();
            fm.Add(new Feature("C", FeatureType.Categorical));
            Item i = new Item(fm);
            i.AddValue("C", "text");

            // act
            bool r = i.HasValue("C");

            // assert
            Assert.IsTrue(r);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddValue_InvalidFeatureName_ThrowException()
        {
            // arrange
            IFeatureManager fm = new FeatureManager();
            fm.Add(new Feature("C", FeatureType.Categorical));
            Item i = new Item(fm);
            i.AddValue("X", "text");

            // act

            // assert
        }

        [TestMethod]
        public void RemoveValue_ReturnsFalse()
        {
            // arrange
            IFeatureManager fm = new FeatureManager();
            fm.Add(new Feature("C", FeatureType.Categorical));
            Item i = new Item(fm);

            // act
            bool r = i.RemoveValue("X");

            // assert
            Assert.IsFalse(r);
        }

        [TestMethod]
        public void RemoveValue_ReturnsTrue()
        {
            // arrange
            IFeatureManager fm = new FeatureManager();
            fm.Add(new Feature("C", FeatureType.Categorical));
            Item i = new Item(fm);

            // act
            bool r = i.RemoveValue("C");

            // assert
            Assert.IsFalse(r);
        }

        [TestMethod]
        public void GetValue_Returns()
        {
            // arrange
            IFeatureManager fm = new FeatureManager();
            fm.Add(new Feature("C", FeatureType.Categorical));
            Item i = new Item(fm);
            i.SetValue("C", "test");

            // act
            string v = i.GetValue("C").Value.ToString();

            // assert
            Assert.AreEqual("test", v);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetValue_InvalidFeatureName_ThrowException()
        {
            // arrange
            IFeatureManager fm = new FeatureManager();
            fm.Add(new Feature("C", FeatureType.Categorical));
            Item i = new Item(fm);
            i.SetValue("C", "test");

            // act
            string v = i.GetValue("X").Value.ToString();

            // assert
        }

        [TestMethod]
        public void GetFeatureNames_Returns()
        {
            // arrange
            IFeatureManager fm = new FeatureManager();
            fm.Add(new Feature("C", FeatureType.Categorical));
            fm.Add(new Feature("N", FeatureType.Numerical));
            Item i = new Item(fm);
            i.SetValue("C", "test");
            i.SetValue("N", 1.13);

            // act
            List<string> names = i.GetFeatureNames();

            // assert
            Assert.IsNotNull(names);
            Assert.AreEqual(2, names.Count);
            CollectionAssert.AreEqual(new List<string> { "C", "N" }, names);
        }
    }
}
