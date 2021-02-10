using System;
using System.Text.RegularExpressions;
using ProfanityFilter.Interfaces;

namespace BP_OnlineDOD.Client.Helpers
{
    public static class TextTransformer
    {
        public static string Transform(string text)
        {
            text = text.Replace("\n", "<br/>");
            
            text = Regex.Replace(text, @"(((f|ht){1}tp[s]?://)[-a-zA-^Z0-9@:\%_\+.~#?&//=]+)", "<a target=\"_blank\" href=\"$1\">$1</a>");
            text = Regex.Replace(text, @"([[:space:]()[{}])(www.[-a-zA-Z0-9@:\%_\+.~#?&//=]+)", "$1<a target=\"_blank\" href=\"http://$2\">$2</a>");

            return text;
        }
    }
}
