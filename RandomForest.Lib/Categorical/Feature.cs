using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Categorical
{
    class Feature<T> : FeatureBase
    {
        private string _name = string.Empty;

        public override string Name
        {
            get { return _name; }
        }

        public override object DefaultValue
        {
            get { return default(T); }
        }

        public override bool IsNumerical
        {
            get
            {
                int df = 0;
                string str = string.Empty;
                if (DefaultValue != null)
                    str = DefaultValue.ToString();
                return int.TryParse(str, out df);
            }
        }

        public Feature(string name)
        {
            _name = name;
        }

        public override Type GetFeatureType()
        {
            return typeof(T);
        }

        public override string ToString()
        {
            return _name + ":" + GetFeatureType().ToString();
        }
    }
}
