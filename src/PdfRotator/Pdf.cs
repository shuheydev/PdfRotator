using iTextSharp.text.pdf;
using System;
using System.IO;

namespace PdfRotator
{
    public class Pdf
    {
        public void Write(PdfReader reader, string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfStamper stamper = new PdfStamper(reader, fs);
                stamper.Close();
            }
        }

        public PdfReader Rotate(PdfReader reader, int pageNumber)
        {
            PdfDictionary page = reader.GetPageN(pageNumber);
            PdfNumber rotate = page.GetAsNumber(PdfName.Rotate);

            page.Put(PdfName.Rotate, new PdfNumber(rotate == null ? 180 : (rotate.IntValue + 180) % 360));

            return reader;
        }

        public int Count(PdfReader reader)
        {
            int pagesCount = reader.NumberOfPages;
            return pagesCount;
        }

        public PdfReader Read(string path)
        {
            PdfReader reader = new PdfReader(path);
            return reader;
        }
    }
}
