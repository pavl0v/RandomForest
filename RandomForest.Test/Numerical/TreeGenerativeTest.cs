using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.Numerical;
using RandomForest.Lib.Numerical.Tree;
using RandomForest.Lib.Numerical.Tree.Export;
using RandomForest.Lib.Numerical.ItemSet;
using System.IO;
using RandomForest.Lib.Numerical.ItemSet.Splitters;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class TreeGenerativeTest
    {
        [TestMethod]
        public void Export_ExportToXlsx()
        {
            // arrange
            ISplitter splitter = new SplitterRss();
            ItemNumericalSet set = ExcelParser.ParseItemNumericalSet(@"data\test-3x50.xlsx");
            TreeBuilder builder = new TreeBuilder("Z", 10, new Lib.NameGenerator(), splitter);
            TreeGenerative tree = builder.Build(set);
            tree.ExportStrategy = new ExportToXlsx();
            DirectoryInfo di = new DirectoryInfo("temp");
            if (!di.Exists)
                di.Create();

            // act
            tree.Export(@"temp\classified.xlsx");

            // assert
        }

        [TestMethod]
        public void Export_ExportToJson()
        {
            // arrange
            string name = "test-2x90-sin";
            ISplitter splitter = new SplitterRss();
            ItemNumericalSet set = ExcelParser.ParseItemNumericalSet(string.Format(@"data\{0}.xlsx", name));
            TreeBuilder builder = new TreeBuilder("Y", 5, new Lib.NameGenerator(), splitter);
            TreeGenerative tree = builder.Build(set);
            tree.ExportStrategy = new ExportToJson();
            DirectoryInfo di = new DirectoryInfo("temp");
            if (!di.Exists)
                di.Create();

            // act
            tree.Export(string.Format(@"temp\{0}.json", name));

            // assert
        }

        [TestMethod]
        public void Export_ExportToTxt()
        {
            // arrange
            ISplitter splitter = new SplitterRss();
            ItemNumericalSet set = ExcelParser.ParseItemNumericalSet(@"data\test-4x15.xlsx");
            TreeBuilder builder = new TreeBuilder("F1", 5, new Lib.NameGenerator(), splitter);
            TreeGenerative tree = builder.Build(set);
            tree.ExportStrategy = new ExportToTxt();
            DirectoryInfo di = new DirectoryInfo("temp");
            if (!di.Exists)
                di.Create();

            // act
            tree.Export(@"temp\test-4x15.txt");

            // assert
        }
    }
}
