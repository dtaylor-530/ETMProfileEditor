using System;
using System.Collections.Generic;
using System.Text;

namespace ETMProfileEditor.Common
{
    public class StringHelper
    {
        public static string GetString(IEnumerable<char> characters)
        {
            var sb = new StringBuilder();
            foreach (var item in characters)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }
    }
}
