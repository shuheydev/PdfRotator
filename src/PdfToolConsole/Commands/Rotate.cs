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
        public void All([Option(0, "pdf file path")] string filePath,
                        [Option(1, "degree")] int degree,
                        [Option("o", "output file path")] string output = "")
        {
            Console.WriteLine($"rotate all.({degree})");

            //check file exist?
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"'{filePath}': No such file.");
                return;
            }

            if (string.IsNullOrEmpty(output))
            {
                if (ConfirmationHelper.OverWrite() == false)
                {
                    Console.WriteLine("Exit");
                    return;
                }
                else
                {
                    output = filePath;
                }
            }

            try
            {
                var pdf = new Pdf(filePath);

                int pageCount = pdf.Count();

                for (int pageNum = 1; pageNum <= pageCount; pageNum++)
                {
                    pdf.Rotate(pageNum, degree);
                }

                pdf.Write(output);
            }
            catch
            {
                Console.WriteLine("Somthing wrong.");
            }
        }

        /// <summary>
        /// routing command: rotate pages
        /// </summary>
        /// <param></param>
        public void Pages([Option(0, "pdf file path")] string filePath,
                          [Option(1, "pageNum1:degree1,pageNum2:degree2,...")] string[] directions,
                          [Option("o", "output file path")] string output = "")
        {
            Console.WriteLine($"rotate pages.({filePath},{string.Join("_", directions)})");

            //check file exist?
            if (File.Exists(filePath) == false)
            {
                Console.WriteLine($"'{filePath}': No such file.");
                return;
            }

            if (ValidateDirection(directions) == false)
            {
                Console.WriteLine("invalid direction");
                return;
            }

            if (string.IsNullOrEmpty(output))
            {
                if (ConfirmationHelper.OverWrite() == false)
                {
                    Console.WriteLine("Exit");
                    return;
                }
                else
                {
                    output = filePath;
                }
            }

            try
            {
                var pdf = new Pdf(filePath);

                foreach (var d in directions.Select(d => TrimDirection(d)))
                {
                    var pageAndAngle = d.Split(':');
                    var pages = PageNumberHelper.ToInt(pageAndAngle[0]);
                    var angle = int.Parse(pageAndAngle[1]);

                    foreach (var p in pages)
                    {
                        pdf.Rotate(p, angle);
                    }
                }

                pdf.Write(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something wrong.");
            }
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
