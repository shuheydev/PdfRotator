using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PdfTool.Helpers
{
    public static class ConfirmationHelper
    {
        public static bool OverWrite(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return true;
            }

            while (true)
            {
                Console.WriteLine($"The file '{Path.GetFileName(filePath)}' already exists.");
                Console.Write("Over write input pdf file? [y/n] : ");

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
