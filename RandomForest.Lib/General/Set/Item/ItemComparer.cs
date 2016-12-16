using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.General.Set.Item
{
    class ItemComparer : IComparer<Item>
    {
        public Feature.Feature Feature { get; set; }

        public int Compare(Item x, Item y)
        {
            FeatureValue xv = x.GetValue(Feature.Name);
            FeatureValue yv = y.GetValue(Feature.Name);

            if (Feature.Type == General.Set.Feature.FeatureType.Numerical)
            {
                double dx = Convert.ToDouble(xv.Value);
                double dy = Convert.ToDouble(yv.Value);
                return dx.CompareTo(dy);
            }

            string sx = Convert.ToString(xv.Value);
            string sy = Convert.ToString(yv.Value);
            return sx.CompareTo(sy);
        }
    }
}
