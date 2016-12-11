using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace RandomForest.App.CustomControls.NumericUpDown
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class IntNumericUpDown : NumericUpDownBase//, INotifyPropertyChanged
    {
        private Regex _regex;
        private Regex _regex2;

        #region Value

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(IntNumericUpDown), new PropertyMetadata(0, PropertyChangedCallback));

        //public event PropertyChangedEventHandler PropertyChanged;

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //PropertyChangedEventHandler h = (d as IntNumericUpDown).PropertyChanged;
            //if (h != null)
            //{
            //    h(d, new PropertyChangedEventArgs("Value"));
            //}
            (d as IntNumericUpDown).tbValue.Text = e.NewValue.ToString();
        }

        #endregion

        public IntNumericUpDown()
        {
            InitializeComponent();

            _regex = new Regex(@"^\d|-$");
            _regex2 = new Regex(string.Format(@"^[-+]?[0-9]*$"));
            tbValue.PreviewTextInput += TbValue_PreviewTextInput;
            //tbValue.TextChanged += TbValue_TextChanged;
        }

        private void TbValue_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TbValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!_regex.Match(e.Text).Success)
            {
                e.Handled = true;
                return;
            }

            var tb = (TextBox)sender;
            int ci = tb.CaretIndex;
            var text = ProcessSelection(tb.Text, e.Text, ci, tb.SelectionStart, tb.SelectionLength);

            if (!_regex2.Match(text).Success)
            {
                e.Handled = true;
                return;
            }
            if (text.Trim() == "-")
            {
                e.Handled = false;
                return;
            }

            int v = Convert.ToInt32(text);

            if(IsLessThanMin(v) || IsGreaterThanMax(v))
            {
                e.Handled = true;
                tb.Text = Value.ToString();
                tb.CaretIndex = ci + 1;
                return;
            }

            e.Handled = true;
            Value = v;
            tb.CaretIndex = ci + 1;
        }

        private bool IsGreaterThanMax(int value)
        {
            if (value <= MaxValue)
                return false;

            Value = MaxValue;
            return true;
        }

        private bool IsLessThanMin(int value)
        {
            if (value >= MinValue)
                return false;

            Value = MinValue;
            return true;
        }

        private void OnBtnUpClick(object sender, RoutedEventArgs e)
        {
            var v = Value;
            v = v + 1;
            if (IsGreaterThanMax(v))
                return;
            IsLessThanMin(v);

            Value = v;
            //tbValue.Text = Value.ToString();
        }

        private void OnBtnDownClick(object sender, RoutedEventArgs e)
        {
            var v = Value;
            v = v - 1;
            if (IsLessThanMin(v))
                return;
            IsGreaterThanMax(v);

            Value = v;
            //tbValue.Text = Value.ToString();
        }
    }
}
