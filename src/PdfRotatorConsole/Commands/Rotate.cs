using ConsoleAppFramework;
using PdfRotator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfRotatorConsole.Commands
{
    public class Rotate : ConsoleAppBase
    {
        /// <summary>
        /// routing command: rotate all
        /// </summary>
        [Command("all", "rotate all page.")]
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
                if (ConfirmOverWrite() == false)
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
        /// <param name="args"></param>
        [Command("pages", "rotate target page.")]
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
                if (ConfirmOverWrite() == false)
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
                    var pageAndDegree = d.Split(':').Select(e => int.Parse(e)).ToList();
                    pdf.Rotate(pageAndDegree[0], pageAndDegree[1]);
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
            bool result = directions.Select(d => TrimDirection(d))
                .All(d => Regex.IsMatch(d, @"^\d+:(-)?\d+$"));

            return result;
        }

        private string TrimDirection(string direction)
        {
            return Regex.Replace(direction, @"\s+", "");
        }

        private bool ConfirmOverWrite()
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
