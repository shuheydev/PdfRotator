using System;
using System.Text.RegularExpressions;

namespace PdfTool.Helpers
{
    public static class ConfirmationHelper
    {
        public static bool OverWrite()
        {
            Console.Write("Over write input pdf file? [y/n] : ");

            while (true)
            {
                string input = Console.ReadLine().Trim();

                if (Regex.IsMatch(input, @"^(y|yes)$", RegexOptions.IgnoreCase))
                {
                    return true;
                }
                if (Regex.IsMatch(input, @"^(n|no)$", RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }
        }
    }
}
