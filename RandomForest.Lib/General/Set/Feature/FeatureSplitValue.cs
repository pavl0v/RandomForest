using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomForest.Lib.General.Set.Item;

namespace RandomForest.Lib.General.Set.Feature
{
    class FeatureSplitValue : FeatureValue
    {
        private Set _left = null;
        private Set _right = null;

        public Set Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public Set Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public FeatureSplitValue(string name, object value)
            : base(name, value)
        {

        }
    }
}
