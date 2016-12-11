using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RandomForest.App.Views
{
    /// <summary>
    /// Interaction logic for UCExcelMode.xaml
    /// </summary>
    public partial class UCExcelMode : UserControl
    {
        private string _initialDirectory = string.Empty;

        public UCExcelMode()
        {
            InitializeComponent();
            _initialDirectory = System.IO.Path.GetDirectoryName(
                System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            _initialDirectory += "\\Data";
        }

        private void btnExportFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = tbExportFolder.Text;
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    tbExportFolder.Text = dialog.FileName;
            }
        }

        private void btnTrainingSet_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.Filters.Add(new CommonFileDialogFilter("Microsoft Excel", "xlsx"));
                dialog.InitialDirectory = _initialDirectory;
                //dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    tbTrainingSet.Text = dialog.FileName;
                    _initialDirectory = System.IO.Path.GetDirectoryName(dialog.FileName);
                }
            }
        }
    }
}
