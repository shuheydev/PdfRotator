using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;

namespace RotatePdf
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Shuhei_Nishizawa_不動産売買契約書.PDF");


            var reader = Read(path);

            int pagesCount = Count(reader);
            Console.WriteLine(pagesCount);

            PdfDictionary page = reader.GetPageN(3);
            PdfNumber rotate = page.GetAsNumber(PdfName.ROTATE);

            page.Put(PdfName.ROTATE, new PdfNumber(rotate == null ? 180 : (rotate.IntValue + 180) % 360));

            string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "output.pdf");

            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfStamper stamper = new PdfStamper(reader, fs);
                stamper.Close();
            }
        }

        static int Count(PdfReader reader)
        {
            int pagesCount = reader.NumberOfPages;
            return pagesCount;
        }

        static PdfReader Read(string path)
        {
            PdfReader reader = new PdfReader(path);
            return reader;
        }
    }


}
