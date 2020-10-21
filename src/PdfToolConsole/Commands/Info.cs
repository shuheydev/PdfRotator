using ConsoleAppFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PdfTool;

namespace PdfToolConsole.Commands
{
    public class Info : ConsoleAppBase
    {
        [Command("info count","Show page count of the pdf file.")]
        public void Count([Option(0, "")] string filePath)
        {
            //check file exist?
            if (File.Exists(filePath) == false)
            {
                Console.WriteLine($"The file '{filePath}' not found.");
                return;
            }

            try
            {
                var pdf = new Pdf(filePath);

                int pageCount = pdf.Count();

                Console.WriteLine($"'{Path.GetFileName(filePath)}' has {pageCount} pages.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

        }
    }
}
