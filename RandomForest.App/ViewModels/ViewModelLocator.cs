namespace RandomForest.App.ViewModels
{
    /**
     * More about ViewModel Locator pattern:
     * https://msdn.microsoft.com/en-us/magazine/jj991965.aspx
     * https://msdn.microsoft.com/ru-ru/library/hh821028.aspx
     */

    class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return new MainWindowViewModel();
            }
        }

        public UCExcelModeViewModel ExcelModeViewModel
        {
            get
            {
                return new UCExcelModeViewModel();
            }
        }

        static ViewModelLocator()
        {

        }
    }
}
