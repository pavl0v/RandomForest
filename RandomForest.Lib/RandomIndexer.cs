using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib
{
    class RandomIndexer
    {
        internal protected List<int> GetRandomIndeces(int count, float subcountRatio, bool withReplacement = false)
        {
            int subcount = (int)Math.Round(count * subcountRatio, 0);
            if (subcount >= count)
                throw new Exception();

            Random rnd = new Random();
            List<int> idxLst = new List<int>();
            int k = 0;
            while (k < subcount)
            {
                int idx = rnd.Next(count);
                if (!withReplacement && idxLst.Contains(idx))
                    continue;
                idxLst.Add(idx);
                k++;
            }

            return idxLst;
        }
    }
}
