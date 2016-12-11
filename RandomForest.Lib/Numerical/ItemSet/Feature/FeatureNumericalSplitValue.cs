using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.ItemSet.Feature
{
    class FeatureNumericalSplitValue : FeatureNumericalValue
    {
        private ItemNumericalSet _left = null;
        private ItemNumericalSet _right = null;

        public ItemNumericalSet Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public ItemNumericalSet Right
        {
            get { return _right; }
            set { _right = value; }
        }
    }
}
