using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.Numerical;
using RandomForest.Lib.Numerical.Tree;
using System.Collections.Generic;
using RandomForest.Lib;
using RandomForest.Lib.Numerical.ItemSet;
using RandomForest.Lib.Numerical.ItemSet.Item;
using RandomForest.Lib.Numerical.ItemSet.Splitters;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class TreeBuilderTest
    {
        private ItemNumericalSet _set = null;

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

        private void FillItemSet_ExcelParser()
        {
            //_set = ExcelParser.ParseItemNumericalSet(@"data\test-15-4.xlsx");
            //_set = ExcelParser.ParseItemNumericalSet(@"data\test-1000-4.xlsx");
            _set = ExcelParser.ParseItemNumericalSet(@"data\test-3x1000-cross.xlsx");
        }

        [TestMethod]
        public void Build_RSS()
        {
            // arrange
            FillItemSet_Features4_Items15();
            ISplitter splitter = new SplitterRss();
            TreeBuilder builder = new TreeBuilder("F1", 5, new NameGenerator(), splitter);

            // act
            TreeGenerative tree = builder.Build(_set);

            // assert
            Assert.IsNotNull(tree);
            Assert.IsTrue(tree.FeatureNames.Count > 0);
            Assert.AreEqual(4, tree.FeatureNames.Count);
        }

        [TestMethod]
        public void Build_Gini()
        {
            // arrange
            FillItemSet_Features4_Items15();
            ISplitter splitter = new SplitterGini();
            TreeBuilder builder = new TreeBuilder("F1", 5, new NameGenerator(), splitter);

            // act
            TreeGenerative tree = builder.Build(_set);

            // assert
            Assert.IsNotNull(tree);
            Assert.IsTrue(tree.FeatureNames.Count > 0);
            Assert.AreEqual(4, tree.FeatureNames.Count);
        }

        [TestMethod]
        public void Build_ExcelParser_RSS()
        {
            // arrange
            FillItemSet_ExcelParser();
            ISplitter splitter = new SplitterRss();
            TreeBuilder builder = new TreeBuilder("Z", 10, new NameGenerator(), splitter);

            // act
            TreeGenerative tree = builder.Build(_set);

            // assert
            Assert.IsNotNull(tree);
            Assert.IsTrue(tree.FeatureNames.Count > 0);
        }

        [TestMethod]
        public void Build_ExcelParser_Gini()
        {
            // arrange
            FillItemSet_ExcelParser();
            ISplitter splitter = new SplitterGini();
            TreeBuilder builder = new TreeBuilder("Z", 10, new NameGenerator(), splitter);

            // act
            TreeGenerative tree = builder.Build(_set);

            // assert
            Assert.IsNotNull(tree);
            Assert.IsTrue(tree.FeatureNames.Count > 0);
        }
    }
}
