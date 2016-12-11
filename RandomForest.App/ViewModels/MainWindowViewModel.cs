using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.App.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Mode _selectedMode = Mode.Excel;
        public Mode SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                if (_selectedMode != value)
                {
                    _selectedMode = value;
                    OnPropertyChanged("SelectedMode");
                }
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var properyChanged = PropertyChanged;
            if (properyChanged != null)
                properyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
