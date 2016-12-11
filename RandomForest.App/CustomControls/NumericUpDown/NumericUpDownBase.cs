using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RandomForest.App.CustomControls.NumericUpDown
{
    public abstract class NumericUpDownBase : UserControl
    {
        #region MaxValue

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(IntNumericUpDown), new PropertyMetadata(int.MaxValue));

        #endregion

        #region MinValue

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(IntNumericUpDown), new PropertyMetadata(int.MinValue));

        #endregion

        protected string ProcessSelection(string text, string input, int caretIndex, int selectionStart, int selectionLength)
        {
            string res = string.Empty;

            var txt = text;
            string l = string.Empty;
            string r = string.Empty;
            if (selectionLength > 0)
            {
                int x1 = selectionStart;
                int x2 = x1 + selectionLength;
                l = x1 == 0 ? "" : txt.Substring(0, x1);
                r = txt.Substring(x2, txt.Length - x2);
                text = l + input + r;
                return text;
            }
            else
                return text.Insert(caretIndex, input);
        }
    }
}
