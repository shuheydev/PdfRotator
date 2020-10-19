using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;
using PdfRotator;

namespace PdfRotatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string targetPath = Path.Combine(@"C:\repos\PdfRotator\src\PdfRotator.Tests\TestData", "PdfRotateTest.pdf");

            var pdf = new Pdf(targetPath);

            int pagesCount = pdf.Count();

            pdf.Rotate(3, 90);

            string outputPath = Path.Combine(@"C:\repos\PdfRotator\src\PdfRotator.Tests\TestData", "output.pdf");

            pdf.Write(outputPath);
        }
    }
}
