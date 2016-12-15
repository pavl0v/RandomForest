using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomForest.Lib.General.Item
{
    class FeatureValue
    {
        private string _name;
        private object _value;
        private Type _type;

        public string Name
        {
            get { return _name; }
        }

        public object Value
        {
            get { return _value; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public FeatureValue(string name, object value, Type type)
        {
            _name = name;
            _value = value;
            _type = type;
        }

        public FeatureValue(string name, object value)
            : this(name, value, null)
        {

        }

        public T GetValue<T>()
        {
            return (T)_value;
        }
    }
}
