using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cross.Cutting.Helper
{
    public static class ObjectHelper
    {
        public static bool IsObjectEmpty<T>(T obj)
        {
            if (obj == null)
                return true;

            foreach (var property in typeof(T).GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null && !value.Equals(GetDefault(property.PropertyType)))
                    return false;
            }

            return true;
        }

        private static object GetDefault(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}