using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.ItemSet.Feature
{
    class FeatureNumerical
    {
        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
        }

        public FeatureNumerical(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
