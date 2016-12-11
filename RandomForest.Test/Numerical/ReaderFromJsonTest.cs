using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.Numerical.Tree;
using RandomForest.Lib.Numerical.Tree.Import;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class ReaderFromJsonTest
    {
        [TestMethod]
        public void Read()
        {
            // arrange
            string path = @"data\cross\decision-tree-001.json";

            // act
            Tree tree = ImportFromJson.Read(path);

            // assert
            Assert.IsNotNull(tree);
            Assert.IsNotNull(tree.Root);
        }
    }
}
