using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfTool.Helpers
{
    public static class PageNumberHelper
    {
        public static int[] ToInt(string[] pageNumbers)
        {
            var result = new List<int>();
            foreach (string num in pageNumbers)
            {
                result.AddRange(ToInt(num));
            }

            return result.ToArray();
        }

        public static int[] ToInt(string pageNumber)
        {
            var result = new List<int>();

            if (Regex.IsMatch(pageNumber, @"^\d+$"))
            {
                result.Add(int.Parse(pageNumber));
            }
            else if (Regex.IsMatch(pageNumber, @"^(\d+)-(\d+)$"))
            {
                var match = Regex.Match(pageNumber, @"^(?<begin>\d+)-(?<end>\d+)$");
                int begin = int.Parse(match.Groups["begin"].Value);
                int end = int.Parse(match.Groups["end"].Value);

                for (int i = begin; i <= end; i++)
                {
                    result.Add(i);
                }
            }

            return result.ToArray();
        }
    }
}
