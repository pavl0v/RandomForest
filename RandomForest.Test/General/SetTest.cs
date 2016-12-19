using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.General.Set.Feature;
using RandomForest.Lib.General.Set;
using RandomForest.Lib.General.Set.Item;
using System.Collections.Generic;

namespace RandomForest.Test.General
{
    [TestClass]
    public class SetTest
    {
        private Set _set;

        private void Fill_Set_ThreeItems()
        {
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));

            _set = new Set(manager);

            Item i1 = _set.CreateItem();
            i1.SetValue("C", "string1");
            i1.SetValue("N", 1);
            _set.AddItem(i1);

            Item i2 = _set.CreateItem();
            i2.SetValue("C", "string2");
            i2.SetValue("N", 2);
            _set.AddItem(i2);

            Item i3 = _set.CreateItem();
            i3.SetValue("C", "string3");
            i3.SetValue("N", 3);
            _set.AddItem(i3);
        }

        //[TestInitialize()]
        //public void Startup()
        //{

        //}

        [TestMethod]
        public void CreateItem_ReturnsItem()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));
            Set set = new Set(manager);

            // act
            Item i = set.CreateItem();

            // assert
            Assert.IsNotNull(i);
            Assert.AreEqual(true, i.HasValue("C"));
            Assert.AreEqual(true, i.HasValue("N"));
            Assert.AreEqual(0, set.Count());
        }

        [TestMethod]
        public void AddItem_OneItem_ReturnsOne()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));
            manager.Add(new Feature("N", FeatureType.Numerical));
            Set set = new Set(manager);
            Item i = set.CreateItem();

            // act
            set.AddItem(i);

            // assert
            Assert.AreEqual(1, set.Count());
        }

        [TestMethod]
        public void AddItem_ThreeItems_ReturnsThree()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act

            // assert
            Assert.AreEqual(3, _set.Count());
        }

        [TestMethod]
        public void GetItem_TreeItems_ReturnsItem()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act
            Item r1 = _set.GetItem(0);
            Item r2 = _set.GetItem(1);
            Item r3 = _set.GetItem(2);

            // assert
            Assert.AreEqual(3, _set.Count());

            Assert.IsNotNull(r1);
            Assert.IsNotNull(r2);
            Assert.IsNotNull(r3);

            Assert.AreEqual("string1", r1.GetValue("C").Value);
            Assert.AreEqual(1.0, r1.GetValue("N").Value);

            Assert.AreEqual("string2", r2.GetValue("C").Value);
            Assert.AreEqual(2.0, r2.GetValue("N").Value);

            Assert.AreEqual("string3", r3.GetValue("C").Value);
            Assert.AreEqual(3.0, r3.GetValue("N").Value);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetAverage_Categorical_ThrowException()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act
            double d = _set.GetAverage("C");

            // assert
        }

        [TestMethod]
        public void GetAverage_Numerical_ReturnsTwo()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act
            double d = _set.GetAverage("N");

            // assert
            Assert.AreEqual(3, _set.Count());
            Assert.AreEqual(2, d);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetRSS_Categorical_ThrowException()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act
            double d = _set.GetRSS("C");

            // assert
        }

        [TestMethod]
        public void GetRSS_Numerical_Returns()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act
            double d = _set.GetRSS("N");

            // assert
            Assert.AreEqual(2, d);
        }

        [TestMethod]
        public void GetGini_Categorical_ReturnsMax()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));

            _set = new Set(manager);

            Item i1 = _set.CreateItem();
            i1.SetValue("C", "A");
            _set.AddItem(i1);

            Item i2 = _set.CreateItem();
            i2.SetValue("C", "B");
            _set.AddItem(i2);

            Item i3 = _set.CreateItem();
            i3.SetValue("C", "C");
            _set.AddItem(i3);

            Item i4 = _set.CreateItem();
            i4.SetValue("C", "D");
            _set.AddItem(i4);

            // act
            double d = _set.GetGini("C");

            // assert
            Assert.AreEqual(0.75, d);
        }

        [TestMethod]
        public void GetGini_Categorical_ReturnsMin()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("C", FeatureType.Categorical));

            _set = new Set(manager);

            Item i1 = _set.CreateItem();
            i1.SetValue("C", "A");
            _set.AddItem(i1);

            Item i2 = _set.CreateItem();
            i2.SetValue("C", "A");
            _set.AddItem(i2);

            Item i3 = _set.CreateItem();
            i3.SetValue("C", "A");
            _set.AddItem(i3);

            Item i4 = _set.CreateItem();
            i4.SetValue("C", "A");
            _set.AddItem(i4);

            // act
            double d = _set.GetGini("C");

            // assert
            Assert.AreEqual(0, d);
        }

        [TestMethod]
        public void GetGini_Numerical_ReturnsMax()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("N", FeatureType.Numerical));

            _set = new Set(manager);

            Item i1 = _set.CreateItem();
            i1.SetValue("N", 1);
            _set.AddItem(i1);

            Item i2 = _set.CreateItem();
            i2.SetValue("N", 2);
            _set.AddItem(i2);

            Item i3 = _set.CreateItem();
            i3.SetValue("N", 3);
            _set.AddItem(i3);

            Item i4 = _set.CreateItem();
            i4.SetValue("N", 4);
            _set.AddItem(i4);

            // act
            double d = _set.GetGini("N");

            // assert
            Assert.AreEqual(0.75, d);
        }

        [TestMethod]
        public void GetGini_Numerical_ReturnsMin()
        {
            // arrange
            IFeatureManager manager = new FeatureManager();
            manager.Add(new Feature("N", FeatureType.Numerical));

            _set = new Set(manager);

            Item i1 = _set.CreateItem();
            i1.SetValue("N", 1);
            _set.AddItem(i1);

            Item i2 = _set.CreateItem();
            i2.SetValue("N", 1);
            _set.AddItem(i2);

            Item i3 = _set.CreateItem();
            i3.SetValue("N", 1);
            _set.AddItem(i3);

            Item i4 = _set.CreateItem();
            i4.SetValue("N", 1);
            _set.AddItem(i4);

            // act
            double d = _set.GetGini("N");

            // assert
            Assert.AreEqual(0, d);
        }

        [TestMethod]
        public void GetFeatureNames_Returns()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act
            List<string> names = _set.GetFeatureNames();

            // assert
            Assert.IsNotNull(names);
            Assert.AreEqual(names.Count, 2);
            CollectionAssert.AreEquivalent(names, new List<string> { "N", "C" });
        }

        [TestMethod]
        public void AddFeature_Returns()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act
            bool r = _set.AddFeature("X", FeatureType.Categorical);
            bool cc = _set.CheckConsistency();
            List<string> names = _set.GetFeatureNames();

            // assert
            Assert.IsTrue(r);
            Assert.IsTrue(cc);
            Assert.IsNotNull(names);
            Assert.AreEqual(names.Count, 3);
            CollectionAssert.AreEquivalent(names, new List<string> { "N", "C", "X" });
        }

        [TestMethod]
        public void RemoveFeature_Returns()
        {
            // arrange
            Fill_Set_ThreeItems();

            // act
            _set.AddFeature("X", FeatureType.Categorical);
            bool r = _set.RemoveFeature("C");
            bool cc = _set.CheckConsistency();
            List<string> names = _set.GetFeatureNames();

            // assert
            Assert.IsTrue(r);
            Assert.IsTrue(cc);
            Assert.IsNotNull(names);
            Assert.AreEqual(names.Count, 2);
            CollectionAssert.AreEquivalent(names, new List<string> { "N", "X" });
        }
    }
}
