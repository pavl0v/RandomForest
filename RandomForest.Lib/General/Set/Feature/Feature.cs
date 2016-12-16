using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.General.Set.Feature
{
    class Feature
    {
        #region FIELDS

        private string _name = string.Empty;
        private FeatureType _type = FeatureType.Numerical;

        #endregion

        #region PROPERTIES

        public string Name
        {
            get { return _name; }
        }

        public FeatureType Type
        {
            get { return _type; }
        }

        #endregion

        public Feature(string name, FeatureType type)
        {
            _name = name;
            _type = type;
        }
    }
}
