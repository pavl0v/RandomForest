using RandomForest.Lib.Numerical.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RandomForest.App.ViewModels
{
    public class UCExcelModeViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private Dictionary<string, Func<string>> _validationDictionary = new Dictionary<string, Func<string>>();
        private Dictionary<string, bool> _errorsDictionary = new Dictionary<string, bool>();
        private ObservableCollection<NameValue> _nameValueList = new ObservableCollection<NameValue>();
        private int _numberOfTrees = 10;
        private int _maxNumberOfTainingItemsInCategory = 5;
        private float _trainingSubsetCountRatio = 0.6f;
        private string _trainingSet = string.Empty;
        private string _exportFolder = string.Empty;
        private string _resolutionFeatureName = string.Empty;
        private IForest _forest;
        private int _progress = 0;
        private string _result = string.Empty;
        private bool _isBtnGenerateEnable = false;
        private bool _isBtnResolveEnable = false;

        public event PropertyChangedEventHandler PropertyChanged;

        #region PROPERTIES

        public int NumberOfTrees
        {
            get { return _numberOfTrees; }
            set
            {
                if (_numberOfTrees == value) return;
                _numberOfTrees = value;
                OnPropertyChanged("NumberOfTrees");
            }
        }

        public int MaxNumberOfTainingItemsInCategory
        {
            get { return _maxNumberOfTainingItemsInCategory; }
            set
            {
                if (_maxNumberOfTainingItemsInCategory == value) return;
                _maxNumberOfTainingItemsInCategory = value;
                OnPropertyChanged("MaxNumberOfTainingItemsInCategory");
            }
        }

        public float TrainingSubsetCountRatio
        {
            get { return _trainingSubsetCountRatio; }
            set
            {
                if (_trainingSubsetCountRatio == value) return;
                _trainingSubsetCountRatio = value;
                OnPropertyChanged("TrainingSubsetCountRatio");
            }
        }

        public string TrainingSet
        {
            get { return _trainingSet; }
            set
            {
                if (_trainingSet == value) return;
                _trainingSet = value;
                OnPropertyChanged("TrainingSet");
            }
        }

        public string ExportFolder
        {
            get { return _exportFolder; }
            set
            {
                if (_exportFolder == value) return;
                _exportFolder = value;
                OnPropertyChanged("ExportFolder");
            }
        }

        public string ResolutionFeatureName
        {
            get { return _resolutionFeatureName; }
            set
            {
                if (_resolutionFeatureName == value) return;
                _resolutionFeatureName = value;
                OnPropertyChanged("ResolutionFeatureName");
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                if (_progress == value) return;
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public bool IsValid
        {
            get { return _errorsDictionary.Count == 0; }
        }

        public ObservableCollection<NameValue> NameValueList
        {
            get { return _nameValueList; }
        }

        public bool IsBtnGenerateEnable
        {
            get { return _isBtnGenerateEnable & IsValid; }
            set
            {
                if (_isBtnGenerateEnable == value) return;
                _isBtnGenerateEnable = value;
                OnPropertyChanged("IsBtnGenerateEnable");
            }
        }

        public bool IsBtnResolveEnable
        {
            get { return _isBtnResolveEnable; }
            set
            {
                if (_isBtnResolveEnable == value) return;
                _isBtnResolveEnable = value;
                OnPropertyChanged("IsBtnResolveEnable");
            }
        }

        public string Result
        {
            get { return _result; }
            set
            {
                if (_result == value) return;
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public ICommand BtnGenerateCommand { get; set; }
        public ICommand BtnResolveCommand { get; set; }

        #endregion

        public UCExcelModeViewModel()
        {
            _validationDictionary.Add("TrainingSet", Validate_TrainingSet);
            _validationDictionary.Add("ExportFolder", Validate_ExportFolder);
            _validationDictionary.Add("ResolutionFeatureName", Validate_ResolutionFeatureName);

            IsBtnGenerateEnable = true;
            BtnGenerateCommand = new RelayCommand(BtnGenerate_Click);
            BtnResolveCommand = new RelayCommand(BtnResolve_Click);
        }

        #region IDataErrorInfo implementation

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string propertyName]
        {
            get
            {
                string error = _validationDictionary[propertyName]();
                //OnPropertyChanged("IsValid");
                OnPropertyChanged("IsBtnGenerateEnable"); 
                return error;
            }
        }

        #endregion

        #region Validation

        private string Validate_TrainingSet()
        {
            if (string.IsNullOrWhiteSpace(TrainingSet))
            {
                if (!_errorsDictionary.ContainsKey("TrainingSet"))
                    _errorsDictionary.Add("TrainingSet", true);
                return "Trainig set path is not defined";
            }

            FileInfo fi = new FileInfo(TrainingSet);
            if (!fi.Exists)
            {
                if (!_errorsDictionary.ContainsKey("TrainingSet"))
                    _errorsDictionary.Add("TrainingSet", true);
                return "Trainig set path does not exist";
            }

            _errorsDictionary.Remove("TrainingSet");
            return string.Empty;
        }

        private string Validate_ExportFolder()
        {
            if (string.IsNullOrWhiteSpace(ExportFolder))
            {
                if (!_errorsDictionary.ContainsKey("ExportFolder"))
                    _errorsDictionary.Add("ExportFolder", true);
                return "Export folder path is not defined";
            }

            DirectoryInfo di = new DirectoryInfo(ExportFolder);
            if (!di.Exists)
            {
                if (!_errorsDictionary.ContainsKey("ExportFolder"))
                    _errorsDictionary.Add("ExportFolder", true);
                return "Export folder path does not exist";
            }

            _errorsDictionary.Remove("ExportFolder");
            return string.Empty;
        }

        private string Validate_ResolutionFeatureName()
        {
            if (string.IsNullOrWhiteSpace(ResolutionFeatureName))
            {
                if (!_errorsDictionary.ContainsKey("ResolutionFeatureName"))
                    _errorsDictionary.Add("ResolutionFeatureName", true);
                return "Resolution feature name is not defined";
            }

            _errorsDictionary.Remove("ResolutionFeatureName");
            return string.Empty;
        }

        #endregion

        private async void BtnGenerate_Click(object obj)
        {
            IsBtnGenerateEnable = false;
            IsBtnResolveEnable = false;

            ForestGrowParameters p = new ForestGrowParameters
            {
                ExportDirectoryPath = ExportFolder,
                ExportToJson = true,
                ResolutionFeatureName = ResolutionFeatureName,
                ItemSubsetCountRatio = TrainingSubsetCountRatio,
                TrainingDataPath = TrainingSet,
                MaxItemCountInCategory = MaxNumberOfTainingItemsInCategory,
                TreeCount = NumberOfTrees,
                SplitMode = SplitMode.GINI
            };

            if (_forest == null)
            {
                _forest = ForestFactory.Create();
                _forest.TreeBuildComplete += _forest_TreeBuildComplete;
                _forest.ForestGrowComplete += _forest_ForestGrowComplete;
            }

            try
            {
                int x = await _forest.GrowAsync(p);
                //int x =  _forest.Grow(p);
            }
            catch (Exception ex)
            {

            }
        }

        private void BtnResolve_Click(object obj)
        {
            Result = string.Empty;
            if (_forest == null)
                return;
            IItemNumerical item = _forest.CreateItem();
            int i = 0;
            foreach(NameValue nv in NameValueList)
            {
                item.SetValue(nv.Name, nv.Value);
                i++;
            }
            if (i == 0)
                return;
            double res = _forest.Resolve(item);
            Result = res.ToString();
        }

        private void _forest_ForestGrowComplete(object sender, EventArgs e)
        {
            Progress = 100;
            IsBtnGenerateEnable = true;
            IsBtnResolveEnable = true;

            App.Current.Dispatcher.Invoke(() => { NameValueList.Clear(); });
            IItemNumerical item = _forest.CreateItem();
            var names = _forest.GetFeatureNames();
            foreach (var name in names)
            {
                if (name == ResolutionFeatureName)
                    continue;
                App.Current.Dispatcher.Invoke(() =>
                {
                    NameValueList.Add(new NameValue { Name = name, Value = 0 });
                });
            }
        }

        private void _forest_TreeBuildComplete(object sender, EventArgs e)
        {
            float f = _forest.TreeCount() * 100f / NumberOfTrees;
            Progress = (int)Math.Round(f);
        }

        private void OnPropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
