using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib
{
    class NameGenerator
    {
        private const int digits = 3;
        private int _i = 0;
        private object _lock = new object();

        internal string Generate(string prefix)
        {
            string res = string.Empty;

            lock(_lock)
            {
                _i = _i + 1;
                 res = prefix + GetDigits();
            }

            return res;
        }

        internal void Reset()
        {
            lock (_lock)
            {
                _i = 0;
            }
        }

        private string GetDigits()
        {
            string str = _i.ToString();
            if (str.Length >= digits)
                return str;
            StringBuilder sb = new StringBuilder();
            sb.Append('0', digits - str.Length);
            sb.Append(str);
            return sb.ToString();
        }
    }
}
