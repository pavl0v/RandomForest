using RandomForest.Lib.Numerical.ItemSet.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.ItemSet.Item
{
    class ItemNumericalComparer : IComparer<ItemNumerical>
    {
        public FeatureNumerical Feature { get; set; }

        public int Compare(ItemNumerical x, ItemNumerical y)
        {
            double xv = x.GetValue(Feature.Name);
            double yv = y.GetValue(Feature.Name);
            return xv.CompareTo(yv);
        }
    }
}
