using RandomForest.Lib.Numerical.ItemSet.Feature;
using RandomForest.Lib.Numerical.ItemSet.Splitters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.ItemSet
{
    static class ExcelParser
    {
        public static ItemNumericalSet ParseItemNumericalSet(string path, int sheetNo = 1)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
                throw new FileNotFoundException();


            OfficeOpenXml.ExcelPackage xls = new OfficeOpenXml.ExcelPackage(fi);
            var sheet = xls.Workbook.Worksheets[sheetNo];

            int cols = sheet.Dimension.End.Column;
            int rows = sheet.Dimension.End.Row;

            List<string> featureNames = new List<string>();
            for (int j = 1; j <= cols; j++)
            {
                var fn = sheet.Cells[1, j].Value;
                if (fn == null)
                    throw new Exception();
                string str = fn.ToString();
                if (string.IsNullOrEmpty(str))
                    throw new Exception();
                featureNames.Add(str);
            }

            ItemNumericalSet set = new ItemNumericalSet(featureNames);

            for (int i = 2; i <= rows; i++)
            {
                FeatureNumericalValue[] arr = new FeatureNumericalValue[cols];
                for (int j = 1; j <= cols; j++)
                {
                    var v = sheet.Cells[i, j].Value;
                    arr[j - 1] = new FeatureNumericalValue { FeatureName = featureNames[j - 1], FeatureValue = Convert.ToDouble(v) };
                }
                set.AddItem(arr);
            }

            return set;
        }
    }
}
