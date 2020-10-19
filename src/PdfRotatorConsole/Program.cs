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

            string targetPath = Path.Combine(@"C:\repos\PdfRotator\src\TestData", "PdfRotateTest.pdf");

            var pdf = new Pdf();
            var reader = pdf.Read(targetPath);

            int pagesCount = pdf.Count(reader);

            reader = pdf.Rotate(reader, 3);

            string outputPath = Path.Combine(@"C:\repos\PdfRotator\src\TestData", "output.pdf");

            pdf.Write(reader, outputPath);
        }
    }
}
