using ConsoleAppFramework;
using PdfTool;
using PdfTool.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PdfToolConsole.Commands
{
    public class Rotate : ConsoleAppBase
    {
        /// <summary>
        /// routing command: rotate all
        /// </summary>
        [Command("all", "Rotate all pages.")]
        public void All([Option(0, "pdf file path")] string filePath,
                        [Option(1, "angle")] int angle,
                        [Option("o", "output file path")] string output = "")
        {
            //check file exist?
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"The file '{filePath}' not found.");
                return;
            }

            if (string.IsNullOrEmpty(output))
            {
                output = filePath;
            }

            if (ConfirmationHelper.OverWrite(output) == false)
            {
                Console.WriteLine("Exit");
                return;
            }

            try
            {
                using var pdf = new Pdf(filePath);

                int pageCount = pdf.Count();

                for (int pageNum = 1; pageNum <= pageCount; pageNum++)
                {
                    pdf.Rotate(pageNum, angle);
                }

                pdf.Write(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        /// <summary>
        /// routing command: rotate pages
        /// </summary>
        /// <param></param>
        [Command("pages", "Rotate the specified pages.")]
        public void Pages([Option(0, "pdf file path")] string filePath,
                          [Option(1, "pageNum1:angle1,pageNum2:angle2,...")] string[] directions,
                          [Option("o", "output file path")] string output = "")
        {
            //check file exist?
            if (File.Exists(filePath) == false)
            {
                Console.WriteLine($"The file '{filePath}' not found.");
                return;
            }

            if (ValidateDirection(directions) == false)
            {
                Console.WriteLine("invalid direction");
                return;
            }

            if (string.IsNullOrEmpty(output))
            {
                output = filePath;
            }

            if (ConfirmationHelper.OverWrite(output) == false)
            {
                Console.WriteLine("Exit");
                return;
            }

            try
            {
                using var pdf = new Pdf(filePath);

                foreach (var d in directions)
                {
                    var trimed = TrimDirection(d);

                    (string page, string angle) = SplitPageAndAngle(trimed);

                    var pageInt = PageNumberHelper.ToInt(page);
                    var angleInt = int.Parse(angle);

                    foreach (var p in pageInt)
                    {
                        pdf.Rotate(p, angleInt);
                    }
                }

                pdf.Write(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        private (string page, string angle) SplitPageAndAngle(string pageAndAngle)
        {
            var splitted = pageAndAngle.Split(':');

            var page = splitted[0];
            var angle = splitted[1];

            return (page, angle);
        }

        private bool ValidateDirection(string[] directions)
        {
            string validPattern = @"^(\d+|\d+-\d+):(-)?\d+$";

            bool result = directions.Select(d => TrimDirection(d))
                .All(d => Regex.IsMatch(d, validPattern));

            return result;
        }

        private string TrimDirection(string direction)
        {
            return Regex.Replace(direction, @"\s+", "");
        }
    }
}
