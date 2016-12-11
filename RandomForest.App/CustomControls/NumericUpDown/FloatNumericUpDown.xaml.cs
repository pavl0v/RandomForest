using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for FloatNumericUpDown.xaml
    /// </summary>
    public partial class FloatNumericUpDown : NumericUpDownBase
    {
        private Regex _regex;
        private Regex _regex2;
        private float _delta = 0;
        private CultureInfo _ci;

        #region Value

        public float Value
        {
            get { return (float)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(float), typeof(FloatNumericUpDown), new PropertyMetadata(-1f, PropertyChangedCallback));

        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FloatNumericUpDown).tbValue.Text = e.NewValue.ToString();
        }

        #endregion

        #region Precision

        public int Precision
        {
            get { return (int)GetValue(PrecisionProperty); }
            set { SetValue(PrecisionProperty, value); }
        }

        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register("Precision", typeof(int), typeof(FloatNumericUpDown), new PropertyMetadata(0, PrecisionPropertyChangedCallback));

        static void PrecisionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < 1)
                (d as FloatNumericUpDown).Precision = 1;
            (d as FloatNumericUpDown).SetDelta();
        }

        #endregion

        public FloatNumericUpDown()
        {
            InitializeComponent();

            _ci = CultureInfo.CurrentCulture; // new CultureInfo("en-US");
            var s = _ci.NumberFormat.NumberDecimalSeparator;
            _regex = new Regex(string.Format(@"^\d|{0}|-$", _ci.NumberFormat.NumberDecimalSeparator));
            _regex2 = new Regex(string.Format(@"^[-+]?[0-9]*\{0}?[0-9]*$", _ci.NumberFormat.NumberDecimalSeparator));
            
            tbValue.PreviewTextInput += TbValue_PreviewTextInput;
            //tbValue.TextChanged += TbValue_TextChanged;
        }

        public void SetDelta()
        {
            _delta = 1 / (float)Math.Pow(10, Precision);
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
            if(!ProcessPrecision(text))
            {
                e.Handled = true;
                return;

            }
            if (text.Trim() == "-")
            {
                e.Handled = false;
                return;
            }

            if (text.Trim() == _ci.NumberFormat.NumberDecimalSeparator)
                text = "0" + text;

            float v = Convert.ToSingle(text, _ci);

            if (IsLessThanMin(v) || IsGreaterThanMax(v))
            {
                e.Handled = true;
                tb.Text = Value.ToString();
                tb.CaretIndex = ci + 1;
                return;
            }

            if (e.Text != _ci.NumberFormat.NumberDecimalSeparator)
                //tb.Text = tb.Text + _ci.NumberFormat.NumberDecimalSeparator;
                e.Handled = true;
            Value = v;
            tb.CaretIndex = ci + 1;
        }

        private bool IsGreaterThanMax(float value)
        {
            if (value <= MaxValue)
                return false;

            Value = MaxValue;

            return true;
        }

        private bool IsLessThanMin(float value)
        {
            if (value >= MinValue)
                return false;

            Value = MinValue;

            return true;
        }

        private bool ProcessPrecision(string s)
        {
            int idx = s.IndexOf(_ci.NumberFormat.NumberDecimalSeparator);
            if (idx == -1)
                return true;
            if(s.Length-1 - idx > Precision)
                return false;

            return true;
        }

        private void OnBtnUpClick(object sender, RoutedEventArgs e)
        {
            var v = Value;
            v = v + _delta;
            if (IsGreaterThanMax(v))
                return;
            IsLessThanMin(v);

            Value = (float)Math.Round(v, Precision);
            tbValue.Text = Value.ToString("G", _ci);
        }

        private void OnBtnDownClick(object sender, RoutedEventArgs e)
        {
            var v = Value;
            v = v - _delta;
            if (IsLessThanMin(v))
                return;
            IsGreaterThanMax(v);

            Value = (float)Math.Round(v, Precision);
            tbValue.Text = Value.ToString("G", _ci);
        }
    }
}
