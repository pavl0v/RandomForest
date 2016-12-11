using RandomForest.Lib.Numerical.Tree.Node;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Tree.Export
{
    class ExportToXlsx : IExportStrategy
    {
        public event EventHandler<string> ExportCompleted;
        private List<string> _featureNames;
        private int _rowNo = 2;
        private bool _isColored = false;
        private string _resolutionFeatureName;

        public void Export(TreeGenerative tree, string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists)
            {
                fi.Delete();
            }

            _featureNames = tree.Root.Set.GetFeatureNames();
            _resolutionFeatureName = tree.ResolutionFeatureName;

            using (OfficeOpenXml.ExcelPackage xls = new OfficeOpenXml.ExcelPackage(fi))
            {
                OfficeOpenXml.ExcelWorksheet sheet = xls.Workbook.Worksheets.Add("Classified");
                for (int j = 0; j < _featureNames.Count; j++)
                    sheet.Cells[1, j + 1].Value = _featureNames[j];
                sheet.Cells[1, _featureNames.Count + 1].Value = "Category";
                sheet.Cells[1, _featureNames.Count + 2].Value = string.Format("Delta({0})", _resolutionFeatureName);
                ExportRecursion(tree.Root, sheet);
                xls.Save();
            }

            var exportCompleted = ExportCompleted;
            if (exportCompleted != null)
                exportCompleted(this, fi.Name);
        }

        private void ExportRecursion(NodeGenerative node, OfficeOpenXml.ExcelWorksheet sheet)
        {
            if (node == null)
                return;

            if (node.IsTerminal)
            {
                _isColored = !_isColored;
                foreach (var item in node.Set.Items())
                {
                    for (int i = 0; i < _featureNames.Count; i++)
                    {
                        sheet.Cells[_rowNo, i + 1].Value = item.GetValue(_featureNames[i]);
                    }
                    sheet.Cells[_rowNo, _featureNames.Count + 1].Value = node.Category;
                    double d = node.Average - item.GetValue(_featureNames[0]);
                    sheet.Cells[_rowNo, _featureNames.Count + 2].Value = d;
                    if(d<0)
                    {
                        sheet.Cells[_rowNo, _featureNames.Count + 2].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                    }
                    else if(d>0)
                    {
                        sheet.Cells[_rowNo, _featureNames.Count + 2].Style.Font.Color.SetColor(System.Drawing.Color.Green);
                    }
                    if (_isColored)
                    {
                        sheet.Cells[_rowNo, 1, _rowNo, _featureNames.Count + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        sheet.Cells[_rowNo, 1, _rowNo, _featureNames.Count + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }
                    _rowNo++;
                }
                return;
            }

            ExportRecursion(node.Left, sheet);
            ExportRecursion(node.Right, sheet);
        }
    }
}
