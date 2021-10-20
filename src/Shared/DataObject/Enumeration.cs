using System;
using System.ComponentModel;

namespace CodeChallenge.DataObjects
{
    public static class Enumeration
    {
        public static string ToDescriptionString(Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToResponseCode(Enum val, int digit = 2)
        {
            return Convert.ToInt32(val).ToString().PadLeft(digit, '0');
        }
    }
}
