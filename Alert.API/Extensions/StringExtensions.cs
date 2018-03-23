using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alert.API.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string property)
        {
            if (!string.IsNullOrWhiteSpace(property))
            {
                char[] charProperty = property.ToCharArray();
                charProperty[0] = char.ToUpper(charProperty[0]);

                return new string(charProperty);
            }

            return property;
        }
    }
}
