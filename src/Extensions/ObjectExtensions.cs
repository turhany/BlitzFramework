using System;
using System.Linq;
using System.Reflection;

namespace BlitzFramework.Extensions
{
    internal static class ObjectExtensions
    {
        public static PropertyInfo GetPropertyInfo(this object item, string propertyName)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var type = item.GetType();

            var propertyInfo = type
                .GetProperties(BindingFlags.NonPublic |
                               BindingFlags.Public |
                               BindingFlags.Instance |
                               BindingFlags.Static).FirstOrDefault(l => l.Name == propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentOutOfRangeException(propertyName);
            }

            return propertyInfo;
        }
        public static bool HasProperty(this object item, string propertyName) => item.GetType().GetProperty(propertyName) != null;
        public static T SetPropertyValue<T>(this object item, string propertyName, T value)
        {
            var propertyInfo = item.GetPropertyInfo(propertyName);
            propertyInfo.SetValue(item, value);

            return value;
        }
    }
}
