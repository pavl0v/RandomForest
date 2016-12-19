using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using RandomForest.Lib.General.Set;

namespace RandomForest.Test.General
{
    [TestClass]
    public class ExcelParserTest
    {
        [TestMethod]
        public void Parse()
        {
            // arrange
            FileInfo fi = new FileInfo(@"data\test-gen-4x10.xlsx");
            if (!fi.Exists)
                Assert.Fail();

            // act
            Set set = ExcelParser.Parse(fi.FullName, 1);

            // assert
            Assert.IsNotNull(set);
            Assert.AreEqual(10, set.Count());
            Assert.AreEqual(true, set.CheckConsistency());
        }
    }
}
