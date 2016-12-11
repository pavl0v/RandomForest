using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib;
using RandomForest.Lib.Numerical;
using System.Collections.Generic;
using RandomForest.Lib.Numerical.ItemSet;
using RandomForest.Lib.Numerical.ItemSet.Item;
using RandomForest.Lib.Numerical.ItemSet.Feature;
using RandomForest.Lib.Numerical.ItemSet.Splitters;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class ItemNumericalSetTest
    {
        private ItemNumericalSet _set = null;

        private void FillItemSet_Features2_Items10()
        {
            string f1 = "F1";
            string f2 = "F2";

            _set = new ItemNumericalSet(new List<string> { f1, f2 });

            const int qty = 10;

            int[] a1 = new int[qty] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            float[] a2 = new float[qty] { 1f, 0.2f, 2.3f, 4.1f, 4f, 2.76f, 1.2f, 3f, 3.14f, 0f };
            for (int i = 0; i < qty; i++)
            {
                ItemNumerical item = _set.CreateItem();
                item.SetValue(f1, a1[i]);
                item.SetValue(f2, a2[i]);
                _set.AddItem(item);
            }
        }

        private void FillItemSet_Features2_Items15()
        {
            string f1 = "F1";
            string f2 = "F2";

            _set = new ItemNumericalSet(new List<string> { f1, f2 });

            const int qty = 15;

            //int[] a1 = new int[qty] { -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7 };
            //int[] a1 = new int[qty] { 0, 0, 0, 0, 0, 0, 0, 7, 8, 9, 10, 11, 12, 13, 14 };
            int[] a1 = new int[qty] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //int[] a1 = new int[qty] { -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11};
            //int[] a1 = new int[qty] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            //int[] a1 = new int[qty] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 0 };
            //int[] a1 = new int[qty] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 };
            float[] a2 = new float[qty] { 1f, 0.2f, 2.3f, 4.1f, 4f, 2.76f, 1.2f, 3f, 3.14f, 0f, 2.1f, 3.5f, 1.4f, 2.9f, 0.4f };
            for (int i = 0; i < qty; i++)
            {
                ItemNumerical item = _set.CreateItem();
                item.SetValue(f1, a1[i]);
                item.SetValue(f2, a2[i]);
                _set.AddItem(item);
            }
        }

        private void FillItemSet_Features4_Items15()
        {
            string f1 = "F1";
            string f2 = "F2";
            string f3 = "F3";
            string f4 = "F4";

            _set = new ItemNumericalSet(new List<string> { f1, f2, f3, f4 });

            const int qty = 15;

            int[] a1 = new int[qty] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 11, 12, 13, 14, 15 };
            float[] a2 = new float[qty] { 1f, 0.2f, 2.3f, 4.1f, 4f, 2.76f, 1.2f, 3f, 3.14f, 0f, 2.2f, 1.4f, 1.01f, 0.3f, 0.23f };
            float[] a3 = new float[qty] { 5f, 1.2f, 6.1f, 2.2f, 1f, 2.6f, 8.2f, 2f, 4.1f, 1.4f, 0.2f, 2.4f, 1.1f, 4.3f, 1.3f };
            int[] a4 = new int[qty] { 1, 2, 6, 3, 1, 0, 4, 4, 2, 5, 0, 3, 3, 1, 2 };
            for (int i = 0; i < qty; i++)
            {
                ItemNumerical item = _set.CreateItem();
                item.SetValue(f1, a1[i]);
                item.SetValue(f2, a2[i]);
                item.SetValue(f3, a3[i]);
                item.SetValue(f4, a4[i]);
                _set.AddItem(item);
            }
        }

        //[TestInitialize()]
        //public void Startup()
        //{

        //}

        #region RANDOM FOREST

        [TestMethod]
        public void GetRSS()
        {
            // arrange
            FillItemSet_Features2_Items10();

            // act
            double r = _set.GetRSS("F2");
            r = Math.Round(r, 4);

            // assert
            Assert.AreEqual(19.9682, r);
        }

        [TestMethod]
        public void GetGini()
        {
            // arrange
            FillItemSet_Features2_Items10();

            // act
            double r = _set.GetGini("F2");
            r = Math.Round(r, 4);

            // assert
            Assert.AreEqual(0.3689, r);
        }

        [TestMethod]
        public void GetAverage()
        {
            // arrange
            FillItemSet_Features2_Items10();

            // act
            double r = _set.GetAverage("F2");
            r = Math.Round(r, 4);

            // assert
            Assert.AreEqual(2.17, r);
        }

        [TestMethod]
        public void GetSeparateValue_ItemSetFeatures2Items5_ReturnsSeparateValue()
        {
            // arrange
            string f1 = "F1";
            string f2 = "F2";
            FeatureNumericalManager featureManager = new FeatureNumericalManager();
            featureManager.Add(new FeatureNumerical(f1));
            featureManager.Add(new FeatureNumerical(f2));
            _set = new ItemNumericalSet(featureManager);

            int[] a1 = new int[5] { 1, 2, 3, 4, 5 };
            float[] a2 = new float[5] { 1f, 0.2f, 2.3f, 4.1f, 4f };
            for (int i = 0; i < 5; i++)
            {
                ItemNumerical item = _set.CreateItem();
                item.SetValue(f1, a1[i]);
                item.SetValue(f2, a2[i]);
                _set.AddItem(item);
            }

            ISplitter splitter = new SplitterRss();

            // act
            FeatureNumericalValue sv = splitter.Split(_set, "F1");
            double fv = Math.Round(sv.FeatureValue, 2);

            // assert
            Assert.IsNotNull(sv);
            //Assert.AreEqual(3.15, fv);
            Assert.AreEqual(1.65, fv);
        }

        [TestMethod]
        public void GetSeparateValue_ItemSetFeatures4Items15_ReturnsSeparateValue()
        {
            // arrange
            FillItemSet_Features4_Items15();
            ISplitter splitter = new SplitterRss();

            // act
            FeatureNumericalValue sv = splitter.Split(_set, "F1");
            double fv = Math.Round(sv.FeatureValue, 2);

            // assert
            Assert.IsNotNull(sv);
        }

        [TestMethod]
        public void GetSeparateValue_ItemSetFeatures1Items1_ReturnsSeparateValue()
        {
            // arrange
            ISplitter splitter = new SplitterRss();
            ItemNumericalSet set = new ItemNumericalSet(new List<string>() { "F1" });
            ItemNumerical item = set.CreateItem();
            item.SetValue("F1", 1);
            set.AddItem(item);

            // act
            var sv = splitter.Split(set, "F1");

            // assert
            Assert.IsNull(sv);
        }

        [TestMethod]
        public void GetSeparateValue_ItemSetFeatures2Items1_ReturnsSeparateValue()
        {
            // arrange
            ISplitter splitter = new SplitterRss();
            ItemNumericalSet set = new ItemNumericalSet(new List<string>() { "F1", "F2" });
            ItemNumerical item = set.CreateItem();
            item.SetValue("F1", 1);
            item.SetValue("F2", 2);
            set.AddItem(item);

            // act
            var sv = splitter.Split(set, "F1");

            // assert
            Assert.IsNull(sv);
            //Assert.AreEqual(true, string.IsNullOrEmpty(sv.FeatureName));
        }

        #endregion

        #region MANAGE FEATURES

        [TestMethod]
        public void GetFeatureNames_ItemSetFeatures5_ReturnsListOf5()
        {
            // arrange
            List<string> featureNames = new List<string> { "F1", "F2", "F3", "F4", "F5" };
            ItemNumericalSet set = new ItemNumericalSet(featureNames);

            // act
            List<string> names = set.GetFeatureNames();

            // assert
            CollectionAssert.AreEqual(featureNames, names);
        }

        [TestMethod]
        public void AddFeature_ItemSetFeatures1Items1_ReturnsItemHasFeature()
        {
            // arrange
            List<string> featureNames = new List<string> { "F1" };
            ItemNumericalSet set = new ItemNumericalSet(featureNames);
            ItemNumerical i = set.CreateItem();
            i.SetValue("F1", 3.2);
            set.AddItem(i);

            // act
            bool r = set.AddFeature("F2");

            // assert
            Assert.AreEqual(true, r & i.HasValue("F2"));
        }

        [TestMethod]
        public void RemoveFeature_ItemSetFeatures2Items1__ReturnsItemHasNotFeature()
        {
            // arrange
            List<string> featureNames = new List<string> { "F1", "F2" };
            ItemNumericalSet set = new ItemNumericalSet(featureNames);
            ItemNumerical i = set.CreateItem();
            i.SetValue("F1", 3.2);
            i.SetValue("F2", 1.1);
            set.AddItem(i);

            // act
            bool r = set.RemoveFeature("F2");

            // assert
            Assert.AreEqual(true, r & !i.HasValue("F2"));
        }

        #endregion

        #region MANAGE ITEMS

        [TestMethod]
        public void CreateItem_ReturnsItem()
        {
            // arrange
            FeatureNumericalManager featureManager = new FeatureNumericalManager();
            featureManager.Add(new FeatureNumerical("F1"));
            featureManager.Add(new FeatureNumerical("F2"));
            ItemNumericalSet set = new ItemNumericalSet(featureManager);

            // act
            ItemNumerical item = set.CreateItem();

            // assert
            Assert.IsNotNull(item);
            Assert.AreEqual(true, item.HasValue("F1") & item.HasValue("F2"));
        }

        [TestMethod]
        public void Count_TwoItemsAdded_ReturnsTwo()
        {
            // arrange
            string f1 = "F1";
            string f2 = "F2";
            FeatureNumericalManager featureManager = new FeatureNumericalManager();
            featureManager.Add(new FeatureNumerical("F1"));
            featureManager.Add(new FeatureNumerical("F2"));
            ItemNumericalSet set = new ItemNumericalSet(featureManager);

            ItemNumerical i1 = set.CreateItem();
            i1.SetValue(f1, 3);
            i1.SetValue(f2, 4.7f);
            set.AddItem(i1);

            ItemNumerical i2 = set.CreateItem();
            i2.SetValue(f1, 1);
            i2.SetValue(f2, 2.1f);
            set.AddItem(i2);

            // act
            int qty = set.Count();

            // assert
            Assert.AreEqual(2, qty);
        }

        [TestMethod]
        public void AddItem()
        {
            // arrange
            List<string> featureNames = new List<string> { "F1", "F2", "F3" };
            ItemNumericalSet set = new ItemNumericalSet(featureNames);

            // act
            FeatureNumericalValue[] arr = new FeatureNumericalValue[3] {
                new FeatureNumericalValue { FeatureName="F1", FeatureValue = 1.1 },
                new FeatureNumericalValue { FeatureName="F2", FeatureValue = 1.2 },
                new FeatureNumericalValue { FeatureName="F3", FeatureValue = 1.3 }
            };
            ItemNumerical item = set.AddItem(arr);

            // assert
            Assert.IsNotNull(item);
            Assert.AreEqual(true, item.HasValue("F1") & item.HasValue("F2") & item.HasValue("F3"));
            Assert.AreEqual(1.3, item.GetValue("F3"));
        }

        [TestMethod]
        public void GetRandomSubset_9from15_Returns9()
        {
            // arrange
            FillItemSet_Features4_Items15();
            float subcountRatio = 0.6f;

            // act
            ItemNumericalSet subset = _set.GetRandomSubset(subcountRatio, false);

            // assert
            Assert.IsNotNull(subset);
            Assert.AreEqual(9, subset.Count());
        }

        #endregion

        [TestMethod]
        public void CheckConsistency_ItemSetFeatures2Items10_ReturnsTrue()
        {
            // arrange
            FillItemSet_Features2_Items10();

            // act
            bool r = _set.CheckConsistency();

            //assert
            Assert.AreEqual(true, r);
        }

        [TestMethod]
        public void Gini()
        {
            // arrange
            FillItemSet_Features2_Items15();

            // act 
            double gini = _set.GetGini2("F1");

            // assert
        }
    }
}
