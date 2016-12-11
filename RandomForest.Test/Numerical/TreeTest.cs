using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.Numerical.ItemSet.Item;
using RandomForest.Lib.Numerical.Tree;
using RandomForest.Lib.Numerical.Tree.Import;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class TreeTest
    {
        [TestMethod]
        public void Resolve()
        {
            // arrange
            string name = "test-2x90-sin";
            ItemNumerical item = ItemNumerical.Create();
            item.AddValue("X", 1.45);
            item.AddValue("Y", 0);

            Tree tree = ImportFromJson.Read(string.Format(@"data\{0}.json", name));

            // act
            var val = tree.Resolve(item);

            // assert

        }
    }
}
