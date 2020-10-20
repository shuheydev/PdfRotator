using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConsoleAppFramework;
using PdfTool;

namespace PdfToolConsole.Commands
{
    public class Select : ConsoleAppBase
    {
        public void Pages([Option(0, "pdf file path")] string filePath,
                          [Option(1, "num1,num2,num3,...")] int[] pageNumbers,
                          [Option("o", "c")] string output = "")
        {
            Console.WriteLine("Hello");
            Console.WriteLine(filePath);
            Console.WriteLine(string.Join("_", pageNumbers));
            Console.WriteLine(output);

            //check file exist?
            if (File.Exists(filePath) == false)
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

                pdf.Select(pageNumbers);

                pdf.Write(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something wrong.");
            }
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
