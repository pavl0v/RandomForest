using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Categorical
{
    abstract class FeatureBase
    {
        public abstract string Name { get; }
        public abstract object DefaultValue { get; }
        public abstract Type GetFeatureType();
        public abstract bool IsNumerical { get; }
    }
}
