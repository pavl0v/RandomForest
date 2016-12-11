using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.Numerical.Interfaces
{
    public static class ForestFactory
    {
        public static IForest Create()
        {
            return new Forest();
        }
    }
}
