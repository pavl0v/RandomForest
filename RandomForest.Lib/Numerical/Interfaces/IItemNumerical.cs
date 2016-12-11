using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Interfaces
{
    public interface IItemNumerical
    {
        void SetValue(string featureName, double featureValue);
        double GetValue(string featureName);
        bool HasValue(string featureName);
    }
}
