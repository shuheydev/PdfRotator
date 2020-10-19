using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace PdfRotator
{
    public class Pdf
    {
        private PdfReader _pdfReader;

        public Pdf(string filePath)
        {
            this.Read(filePath);
        }

        public void Write(string outputPath)
        {
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfStamper stamper = new PdfStamper(_pdfReader, fs);
                stamper.Close();
            }
        }

        public int Rotate(int pageNumber, int rotateDegree)
        {
            var page = _pdfReader.GetPageN(pageNumber);
            var rotate = page.GetAsNumber(PdfName.Rotate);

            page.Put(PdfName.Rotate, new PdfNumber(rotate == null ? rotateDegree : (rotate.IntValue + rotateDegree) % 360));

            return page.GetAsNumber(PdfName.Rotate).IntValue;
        }

        public int Count()
        {
            int pagesCount = _pdfReader.NumberOfPages;
            return pagesCount;
        }

        private int Read(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"'{path}' not found.");

            _pdfReader = new PdfReader(path);
            return _pdfReader.NumberOfPages;
        }

        public int GetPageRotate(int pageNumber)
        {
            var page = _pdfReader.GetPageN(pageNumber);
            var rotate = page.GetAsNumber(PdfName.Rotate);

            return rotate is null ? 0 : rotate.IntValue;
        }
    }
}
