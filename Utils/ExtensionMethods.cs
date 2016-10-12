using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Data;

namespace Utils.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool HasValueLength(this string s)
        {
            return (s != null && s.Trim().Length > 0);
        }
    }
}
