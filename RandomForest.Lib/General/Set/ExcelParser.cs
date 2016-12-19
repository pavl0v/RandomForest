using RandomForest.Lib.General.Set.Feature;
using RandomForest.Lib.General.Set.Item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.General.Set
{
    static class ExcelParser
    {
        public static Set Parse(string path, int sheetNo = 1)
        {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists)
                throw new FileNotFoundException();

            List<string> featureNames = new List<string>();
            FeatureManager fm = new FeatureManager();
            Set set;

            int cols = 0;
            int rows = 0;

            using (OfficeOpenXml.ExcelPackage xls = new OfficeOpenXml.ExcelPackage(fi))
            {
                var sheet = xls.Workbook.Worksheets[sheetNo];

                cols = sheet.Dimension.End.Column;
                rows = sheet.Dimension.End.Row;

                for (int j = 1; j <= cols; j++)
                {
                    var fn = sheet.Cells[1, j].Value;
                    if (fn == null)
                        throw new Exception();

                    string str = fn.ToString().ToLower();
                    if (string.IsNullOrEmpty(str))
                        throw new Exception();

                    if (!(str.StartsWith("c#") || str.StartsWith("n#")))
                        throw new Exception("Unable to define feature type. Feature name must start with 'c#' or 'n#'.");

                    string featureName = str.Substring(2, str.Length - 2);
                    if (str.StartsWith("c#"))
                        fm.Add(new Feature.Feature(featureName, FeatureType.Categorical));
                    else
                        fm.Add(new Feature.Feature(featureName, FeatureType.Numerical));

                    featureNames.Add(featureName);
                }

                set = new Set(fm);

                for (int i = 2; i <= rows; i++)
                {
                    Item.Item item = new Item.Item(fm);
                    for (int j = 1; j <= cols; j++)
                    {
                        var v = sheet.Cells[i, j].Value;
                        item.AddValue(featureNames[j - 1], v);
                    }
                    set.AddItem(item);
                }

            }

            return set;
        }
    }
}
