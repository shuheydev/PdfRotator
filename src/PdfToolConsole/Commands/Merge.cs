using ConsoleAppFramework;
using PdfTool;
using PdfTool.Helpers;
using System;
using System.IO;
using System.Linq;

namespace PdfToolConsole.Commands
{
    public class Merge : ConsoleAppBase
    {
        [Command("files", "Merge pdf files")]
        public void Files([Option(0, "pdf file path1,path2,path3,....")] string[] files,
                          [Option("o", "output file path")] string output = "")
        {
            if (files.Length < 2)
            {
                Console.WriteLine("Specify at least 2 files");
                return;
            }

            //check file exist?
            bool isAllFileExist = true;
            foreach (var file in files)
            {
                if (File.Exists(file) == false)
                {
                    Console.WriteLine($"The file '{file}' not found.");
                    isAllFileExist = false;
                }
            }
            if (isAllFileExist == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(output))
            {
                string baseFilePath = files[0];
                output = baseFilePath;
            }

            if (ConfirmationHelper.OverWrite(output) == false)
            {
                Console.WriteLine("Exit");
                return;
            }

            try
            {
                var pdfs = files.Select(f => new Pdf(f));

                var basePdf = pdfs.ElementAt(0);//Take first pdf as base.
                var otherPdfs = pdfs.Skip(1).ToArray();//Take 2nd and later.

                using var merged = basePdf.Merge(otherPdfs);

                merged.Write(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
