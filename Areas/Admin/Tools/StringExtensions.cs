using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Areas.Admin.Tools
{
    public static class StringExtensions
    {
        public static string GetFromSpecific(this string text, string startAt)
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.LastIndexOf(startAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(charLocation, text.Length);
                }
            }

            return String.Empty;
        }
    }
}
