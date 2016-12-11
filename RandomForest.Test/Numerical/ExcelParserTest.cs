using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomForest.Lib.Numerical;
using System.IO;
using RandomForest.Lib.Numerical.ItemSet;

namespace RandomForest.Test.Numerical
{
    [TestClass]
    public class ExcelParserTest
    {
        private int _itemsQty = 50;

        private bool GenerateExcelDocumentF3()
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(@"temp\test.xlsx");
            if (fi.Exists)
            {
                try
                {
                    fi.Delete();
                }
                catch
                {
                    return false;
                }
            }
            OfficeOpenXml.ExcelPackage xls = new OfficeOpenXml.ExcelPackage(fi);
            var sheet = xls.Workbook.Worksheets.Add("test");
            sheet.Cells["A1"].Value = "X";
            sheet.Cells["B1"].Value = "Y";
            sheet.Cells["C1"].Value = "Z";
            int min = 0;
            int max = 50;
            Random rnd = new Random();
            for (int i = 1; i < _itemsQty + 1; i++)
            {
                sheet.Cells[i + 1, 1].Value = Math.Round(rnd.NextDouble() * (max - min) + min, 2);
                sheet.Cells[i + 1, 2].Value = Math.Round(rnd.NextDouble() * (max - min) + min, 2);
                sheet.Cells[i + 1, 3].Value = Math.Round(rnd.NextDouble() * (max - min) + min, 2);
            }
            xls.Save();
            return true;
        }

        private bool GenerateExcelDocumentF4()
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(@"temp\test.xlsx");
            if(fi.Exists)
            {
                try
                {
                    fi.Delete();
                }
                catch
                {
                    return false;
                }
            }
            OfficeOpenXml.ExcelPackage xls = new OfficeOpenXml.ExcelPackage(fi);
            var sheet = xls.Workbook.Worksheets.Add("test");
            sheet.Cells["A1"].Value = "F1";
            sheet.Cells["B1"].Value = "F2";
            sheet.Cells["C1"].Value = "F3";
            sheet.Cells["D1"].Value = "F4";
            int min = 0;
            int max = 50;
            Random rnd = new Random();
            for (int i = 1; i < _itemsQty + 1; i++)
            {
                sheet.Cells[i + 1, 1].Value = Math.Round(rnd.NextDouble() * (max - min) + min, 2);
                sheet.Cells[i + 1, 2].Value = Math.Round(rnd.NextDouble() * (max - min) + min, 2);
                sheet.Cells[i + 1, 3].Value = Math.Round(rnd.NextDouble() * (max - min) + min, 2);
                sheet.Cells[i + 1, 4].Value = Math.Round(rnd.NextDouble() * (max - min) + min, 2);
            }
            xls.Save();
            return true;
        }

        [TestMethod]
        public void FillItemNumericalSet_ExcelDocument_()
        {
            // arrange
            GenerateExcelDocumentF3();
            FileInfo fi = new FileInfo(@"temp\test.xlsx");
            //FileInfo fi = new FileInfo(@"data\test-4x1000.xlsx");
            if (!fi.Exists)
                Assert.Fail();

            // act
            ItemNumericalSet set = ExcelParser.ParseItemNumericalSet(fi.FullName, 1);

            // assert
            Assert.IsNotNull(set);
            Assert.AreEqual(_itemsQty, set.Count());
            Assert.AreEqual(true, set.CheckConsistency());
        }
    }
}
