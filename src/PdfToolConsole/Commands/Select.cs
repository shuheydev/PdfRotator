using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConsoleAppFramework;
using PdfTool;
using PdfTool.Helpers;

namespace PdfToolConsole.Commands
{
    public class Select : ConsoleAppBase
    {
        [Command("pages", "Select the specified pages from pdf and save to file.")]
        public void Pages([Option(0, "pdf file path")] string filePath,
                          [Option(1, "num1,num2,num3,...")] string[] pageNumbers,
                          [Option("o", "output file")] string output = "")
        {
            //check file exist?
            if (File.Exists(filePath) == false)
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

                pdf.Select(PageNumberHelper.ToInt(pageNumbers));

                pdf.Write(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

    }
}
